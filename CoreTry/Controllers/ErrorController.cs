using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTry.Controllers
{
    public class ErrorController : Controller
    {
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        [Route("Error/{statusCode}")]
        public IActionResult Index(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();
            switch (statusCode)
            {
                case 404:
                    /* ViewBag.ErrorMessage = "Sorry, the resource you requested could not found";
                     ViewBag.QS = statusCodeResult.OriginalPath;
                     ViewBag.Path = statusCodeResult.OriginalQueryString;*/
                    _logger.LogWarning($"404 Error Occured. Path = {statusCodeResult.OriginalPath}"
                       + $" and QueryString = {statusCodeResult.OriginalQueryString}");

                    break;
            }
            return View("NotFound");
        }
        [AllowAnonymous]
        [Route("Error")]
        public IActionResult Error()
        {
            var exceptionDetials = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            //exceptionDetials.Error;
            _logger.LogError($"The path {exceptionDetials.Path} thres an"
                + " exception {exceptionDetials.Error}");
            /*ViewBag.ExceptionPath = exceptionDetials.Path;
            ViewBag.ExceptionMessage = exceptionDetials.Error.Message;
            ViewBag.Stacktrace = exceptionDetials.Error.StackTrace;*/
            return View("Error");

        }
    }
}
