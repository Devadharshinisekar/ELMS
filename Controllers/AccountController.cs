using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using ELMSApplication;
using Serilog;
using ELMSApplication.Models;
using ELMSApplication.Filters;
using System.Security.Claims;

namespace ELMSApplication.Controllers;
public class AccountController : Controller
{
    //private string connectionString = "your connection string here";
    private readonly IConfiguration _configuration;
    public AccountController(IConfiguration configuration)//Manager.ConnectionStrings["DefaultConnection"].ConnectionString;
    {
        _configuration =configuration;
    }

    //[ServiceFilter(typeof(AuthorizationFilter))]
    [HttpGet]
    public IActionResult Login()
    {
        ClaimsPrincipal claimUser=HttpContext.User;
        if(claimUser.Identity.IsAuthenticated){
        return RedirectToAction("Login","Account");
        }
    return View();
    }

    [HttpPost]
    public async Task<ActionResult> Login([Bind("EmployeeId,Password")] Employee employee)
    {
       // using (SqlConnection connection = new SqlConnection("DefaultConnection"))
        //{
          if (ModelState.IsValid) 
            {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");
            SqlConnection connection=new SqlConnection(connectionString);
            SqlCommand command = new SqlCommand("select * from Employee where EmployeeId=@EmployeeId and Password=@Password;", connection);
            //command.CommandType = CommandType.StoredProcedure;

            SqlParameter paramEmployeeId = new SqlParameter("@EmployeeId", employee.EmployeeId);
            SqlParameter paramPassword = new SqlParameter("@Password", employee.Password);

            command.Parameters.Add(paramEmployeeId);
            command.Parameters.Add(paramPassword);

            connection.Open();

            SqlDataReader reader = command.ExecuteReader();

            if (reader.HasRows)
            {
                reader.Read();

                //Employee user = new Employee();
                employee.Id = (long)reader.GetInt64(0);
                employee.EmployeeName = reader.GetString(1);
                employee.EmployeeId=reader.GetString(2);
                employee.Password=reader.GetString(3);
                employee.IsAdmin = reader.GetBoolean(4);
                //var Role=Convert.ToString(employee.IsAdmin);
                reader.Close();
                connection.Close();
                Log.Information("Employee Login Triggered");
                //FormsAuthentication.SetAuthCookie(user.Username, false);
                HttpContext.Session.SetString("EmployeeNames",employee.EmployeeName);
                HttpContext.Session.SetString("EmployeeID",employee.EmployeeId);
                HttpContext.Session.SetString("Admin", employee.IsAdmin.ToString());
                List<Claim> claims = new List<Claim>(){
                    new Claim(ClaimTypes.NameIdentifier,employee.EmployeeId),
                    new Claim(ClaimTypes.Role,employee.IsAdmin.ToString())
                };
                ClaimsIdentity claimsIdentity=new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                AuthenticationProperties properties=new AuthenticationProperties(){
                    AllowRefresh =true,
                    IsPersistent=employee.KeepLoggedIn
                };
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,new ClaimsPrincipal(claimsIdentity),properties);
                //ViewBag.Email=model.Email;
                return RedirectToAction("Index", "Leave");
              
            }
            else
            {
                ViewData["msg"] = "Invalid EmployeeId or password";
                //ViewBag.ErrorMessage = "Invalid username or password";
                return View();
            }
        }
        return View();
    }
}

