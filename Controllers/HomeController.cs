using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using ELMSApplication.Models;
using ELMSApplication.Filters;
namespace ELMSApplication.Controllers;
 [ServiceFilter(typeof(ExceptionLogFilter))]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
       
        return View();
    }
    public IActionResult Privacy()
    {  
       // throw new Exception("Testing Error");
        return View();
    } 
    /*public IActionResult LogOut()
    {  
       // throw new Exception("Testing Error");
        return View();
    } */
   /*public IActionResult Login(string EmployeeID, string Password){
    using (SqlConnection sqlconnection=new SqlConnection("DefaultConnection")){
        string sql=$"select * from Employee Where EmployeeID ='{EmployeeID}' and Password ='{Password}' order by Employee";
        SqlCommand  command=new SqlCommand(sql,sqlconnection);
        sqlconnection.Open();
        using SqlDataReader dataReader=command.ExecuteReader();{
            if(dataReader.Read()){
                return RedirectToAction("Index","Leave");
            }
        }
        sqlconnection.Close();
    }
    ViewBag.Result="Success";
    return View();
   }*/
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
