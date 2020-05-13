using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net;

using aspnetapp.Models;
using Filters;
using Datadog.Trace;

namespace aspnetapp.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [ExampleFilterAttributeAsync]
        public IActionResult Index()
        {
            return View();
        }

        [ExampleFilterAttribute]
        public IActionResult Privacy()
        {

            using (var wb = new WebClient())
            {
                var response = wb.DownloadString("https://www.google.com");
            }

            var scope = Tracer.Instance.ActiveScope;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}