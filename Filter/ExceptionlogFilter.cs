// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.Filters;

// namespace ELMSApplication.Filters{
//     public class ExceptionLogFilter : ExceptionFilterAttribute,IExceptionFilter{
//         public override void OnException(ExceptionContext context)
//         {
//             var exceptionMessage=context.Exception.Message;
//             var StackTrace=context.Exception.StackTrace;
//             var controllerName=context.RouteData.Values["Controller"].ToString();
//             var actionName=context.RouteData.Values["action"].ToString();
//              var Message="Date :"+DateTime.Now.ToString("dd-MM-yyyy hh:mm tt")+"Error Message:"+exceptionMessage+Environment.NewLine+"Stack Trace:"+StackTrace;
//              context.Result = new RedirectResult("/Error/Index");
//            base.OnException(context);
//         }
//     }
// }

using System;
using System.Data;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using ELMSApplication.Models;
using ELMSApplication.Controllers;
using System.Net.Sockets;

namespace ELMSApplication.Filters
{
    public class ExceptionLogFilter : ExceptionFilterAttribute, IExceptionFilter
    {
         private readonly ILogger<ExceptionLogFilter> _logger;
          public ExceptionLogFilter(ILogger<ExceptionLogFilter> logger)
    {
        _logger = logger;
    }
        public override void OnException(ExceptionContext context)
        {
            if (context.Exception is DbUpdateException)
            {
                _logger.LogError(context.Exception, "An error occurred while updating the database");

                
                var result = new ViewResult{ViewName="Error"}; // Initialize the result variable with the appropriate type


                result.TempData = new TempDataDictionary(context.HttpContext, context.HttpContext.RequestServices.GetRequiredService<ITempDataProvider>());

                result.TempData["Message"] = "An error occurred while updating the error";

                context.Result = result;
                context.ExceptionHandled = true;
        
            }
            else if(context.Exception is HttpRequestException || context.Exception is SocketException)
            {
                _logger.LogError(context.Exception,"An error occued while accessing the web API");
                var result = new ViewResult{ViewName="Error"}; // Initialize the result variable with the appropriate type


                result.TempData = new TempDataDictionary(context.HttpContext, context.HttpContext.RequestServices.GetRequiredService<ITempDataProvider>());

                result.TempData["Message"] = "An error occurred while accessing the Admin page";

                context.Result = result;
                context.ExceptionHandled = true;
            }
            else if (context.Exception is SqlException)
            {
               _logger.LogError(context.Exception, "An error occurred while connecting to database");

                
                var result = new ViewResult{ViewName= "Error"}; // Initialize the result variable with the appropriate type


                result.TempData = new TempDataDictionary(context.HttpContext, context.HttpContext.RequestServices.GetRequiredService<ITempDataProvider>());

                result.TempData["Message"] = "An error occurred while connecting to server";

                context.Result = result;
                context.ExceptionHandled = true;
            }
            else
            {
                var exceptionMessage = context.Exception.Message;
                var StackTrace = context.Exception.StackTrace;
                var controllerName = context.RouteData.Values["Controller"].ToString();
                var actionName = context.RouteData.Values["action"].ToString();
                var Message = "Date :" + DateTime.Now.ToString("dd-MM-yyyy hh:mm tt") + "Error Message:" + exceptionMessage + Environment.NewLine + "Stack Trace:" + StackTrace;
                context.Result = new RedirectResult("/Error/Index");
                base.OnException(context);
            }
        }
    }
}