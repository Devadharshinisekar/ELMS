using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using ELMSApplication;

namespace ELMSApplication.Controllers
{
    [Authorize]
    public class LeaveController : Controller
    {
        private readonly ELMSApplicationContext _context;
       
        public LeaveController(ELMSApplicationContext context)
        {
            _context = context;
        }

        // GET: Leave

        public async Task<IActionResult> Index()
        {

           // string? employeeId = TempData["EmployeeID"] as string;
           // string? admin = TempData["Admin"] as string;
             string? employeeId =  HttpContext.Session.GetString("EmployeeID");
            string? admin = HttpContext.Session.GetString("Admin");
             var leave = from l in _context.Leave
                   select l;
            if(_context.Leave != null)
            {
                if(admin.Equals("False"))
                {                   
                       leave = leave.Where(l => l.EmployeeId.Equals(employeeId));
                }
                
            }
            else{
                    Problem("Entity set 'ELMSApplicationContext.Leave'  is null.");
            }
            //TempData.Keep("Admin");
           
           // TempData.Keep("EmployeeName");
            //TempData.Keep("EmployeeID");            
            return View(await leave.AsNoTracking().ToListAsync());        
                          
        }
        public async Task<IActionResult> Logout(){
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login","Employee");
        }

        // GET: Leave/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Leave == null)
            {
                return NotFound();
            }

            var leave = await _context.Leave
                .FirstOrDefaultAsync(m => m.LeaveId == id);
            if (leave == null)
            {
                return NotFound();
            }
            //TempData.Keep("Admin");
            //TempData.Keep("EmployeeName");
            //TempData.Keep("EmployeeID");  
            return View(leave);
        }

        // GET: Leave/Create
        public IActionResult Create()
        {
            //TempData.Keep("Admin");
            //TempData.Keep("EmployeeName");
            //TempData.Keep("EmployeeID");  
            return View();
        }

        // POST: Leave/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaveId,EmployeeId,StartDate,EndDate,LeaveType,Status")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                try{
                    _context.Add(leave);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex){ throw;}                 
                return RedirectToAction(nameof(Index));
            }       
            //TempData.Keep("Admin");    
            return View(leave);
        }

        // GET: Leave/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Leave == null)
            {
                return NotFound();
            }

            var leave = await _context.Leave.FindAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            return View(leave);
        }

        // POST: Leave/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaveId,EmployeeId,StartDate,EndDate,LeaveType,Status")] Leave leave)
        {
            if (id != leave.LeaveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leave);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveExists(leave.LeaveId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
           // TempData.Keep("Admin");
         //   TempData.Keep("EmployeeName");
           // TempData.Keep("EmployeeID");  
            return View(leave);
        }

        // GET: Leave/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Leave == null)
            {
                return NotFound();
            }

            var leave = await _context.Leave
                .FirstOrDefaultAsync(m => m.LeaveId == id);
            if (leave == null)
            {
                return NotFound();
            }            

            return View(leave);
        }

        // POST: Leave/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Leave == null)
            {
                return Problem("Entity set 'ELMSApplicationContext.Leave'  is null.");
            }
            var leave = await _context.Leave.FindAsync(id);
            if (leave != null)
            {
                _context.Leave.Remove(leave);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

         public async Task<IActionResult> ApprovedAsync(int id)
         {
            var leave = await _context.Leave.FindAsync(id);
            leave.Status ="Approved";
            _context.Update(leave);
                    await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         } 

         public async Task<IActionResult> RejectedAsync(int id)
         {
            var leave = await _context.Leave.FindAsync(id);
            leave.Status ="Rejected";
            _context.Update(leave);
                    await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
         } 

        private bool LeaveExists(int id)
        {
          return (_context.Leave?.Any(e => e.LeaveId == id)).GetValueOrDefault();
        }
    }
}
