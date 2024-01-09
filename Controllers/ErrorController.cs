using Microsoft.AspNetCore.Mvc;
namespace  ELMSApplication.Controllers{
    public class ErrorController : Controller{
        public IActionResult Index(){
            return View();
        }
    }
}