using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ELMSApplication;
using Serilog;
using ELMSApplication.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace ELMSApplication.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ELMSApplicationContext _context;

        public EmployeeController(ELMSApplicationContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
              return _context.Employee != null ? 
                          View(await _context.Employee.ToListAsync()) :
                          Problem("Entity set 'ELMSApplicationContext.Employee'  is null.");
        }

        public IActionResult Login()
        {
            ClaimsPrincipal claimUser=HttpContext.User;
            if(claimUser.Identity.IsAuthenticated)
                return RedirectToAction("Index","Leave");
            return View();
        }

        // POST: Employee/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("EmployeeId,Password")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                var tempUser = _context.Employee.FirstOrDefault(u=>u.EmployeeId == employee.EmployeeId && u.Password == employee.Password);
                
                if(tempUser!=null)
                {
                Log.Information("Employee Login Triggered");
               // TempData["EmployeeName"] = tempUser.EmployeeName;
                //TempData["EmployeeID"] = tempUser.EmployeeId;
                //TempData["Admin"] = tempUser.IsAdmin.ToString();
                 HttpContext.Session.SetString("EmployeeName",tempUser.EmployeeName);
                 HttpContext.Session.SetString("EmployeeID",tempUser.EmployeeId);
                 HttpContext.Session.SetString("Admin", tempUser.IsAdmin.ToString());
                List<Claim> claims=new List<Claim>(){
                    new Claim(ClaimTypes.NameIdentifier,employee.EmployeeId)
                    //new Claim(ClaimTypes.Role,"Other Properties")
                };
                ClaimsIdentity claimsIdentity=new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

                AuthenticationProperties properties=new AuthenticationProperties(){
                    AllowRefresh =true,
                    IsPersistent=true //employee.KeepLoggedIn

                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),properties);
                return RedirectToAction("Index","Leave");//"/Leave/Index");

                }
                else{
                    ModelState.AddModelError("","Invalid login credentials");
                }
            }
            ViewData["ValidateMessage"]="user not found";
            return View();//(employee);
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeName,EmployeeId,Password,IsAdmin")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,EmployeeName,EmployeeId,Password,IsAdmin")] Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
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
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Employee == null)
            {
                return NotFound();
            }

            var employee = await _context.Employee
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Employee == null)
            {
                return Problem("Entity set 'ELMSApplicationContext.Employee'  is null.");
            }
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(long id)
        {
          return (_context.Employee?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
