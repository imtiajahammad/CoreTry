using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CoreTry.Models;
using CoreTry.Security;
using CoreTry.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CoreTry.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ILogger _logger;
        private readonly IDataProtector protector;

        public HomeController(IEmployeeRepository employeeRepository,
                                IHostingEnvironment hostingEnvironment,
                                ILogger<HomeController> logger,
                               IDataProtectionProvider dataProtectionProvider,
                              DataProtectionPurposeStrings dataProtectionPurposeStrings)
        {
            _employeeRepository = employeeRepository;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
            this.protector = dataProtectionProvider.CreateProtector(
               dataProtectionPurposeStrings.EmployeeIdRouteValue);
        }
        //[Route("")]
        //[Route("~/Home")]
        //[Route("Home")]
        //[Route("Home/index")]
        //[Route("[action]")]//[Route("index")]
        //[Route("~/")]//tilda works independently, will not take any route from class above
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Index()
        {
            //var model = _employeeRepository.GetEmployee(1);
            var model = _employeeRepository.GetAllEmployees().AsEnumerable()
                            .Select(e=>
                            {
                                // Encrypt the ID value and store in EncryptedId property
                                e.EncryptedId = protector.Protect(e.Id.ToString());
                                return e;
                            });
            //return Json(_employeeRepository.GetEmployee(1).Name);
            return View(model);
        }




        //[Route("Home/Get/{id?}")]
        //[Route("[action]/{id?}")]//[Route("Get/{id?}")]
        //[Route("{id}")]
        [HttpGet]
        [AllowAnonymous]
        public ViewResult Get(string id)
        {
            //throw new Exception("Error in Details View");
            _logger.LogTrace("LogTrace log");
            _logger.LogDebug("LogDebug log");
            _logger.LogInformation("LogInformation log");
            _logger.LogWarning("LogWarning log");
            _logger.LogError("LogError log");
            _logger.LogCritical("LogCritical log");
            /*Employee model = _employeeRepository.GetEmployee(id);
            ViewData["PageTitle"] = "Details View";
            ViewData["Employee"] = model;
            ViewBag.something = "something from home/details";*/
            int employeeId= Convert.ToInt32(protector.Unprotect(id));
            Employee employee = _employeeRepository.GetEmployee(employeeId);
            if (employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", employeeId);
            }

            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(employeeId),
                PageTitle = "Employee Details"
            };

            //return View(model);
            //return View("MyViews/Test.cshtml");
            //return View("../../MyViews/Test");
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFilename = null;
                if (model.Photo != null)
                {
                    string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "images");
                    uniqueFilename = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFilename);
                    model.Photo.CopyTo(new FileStream(filePath, FileMode.Create));
                }
                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFilename
                };

                _employeeRepository.Add(newEmployee);
                return RedirectToAction("Get", new { id = newEmployee.Id });
            }
            return View();
        }
    }
}