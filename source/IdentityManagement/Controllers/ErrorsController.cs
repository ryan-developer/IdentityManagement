using IdentityManagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Net;

namespace IdentityManagement.Controllers
{
    [Route("errors")]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorsController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public ErrorsController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Index(int? code = null)
        {
            return View(
                new ErrorViewModel { 
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                    StatusCode = code
                });
        }


        [HttpGet("access-denied")]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
