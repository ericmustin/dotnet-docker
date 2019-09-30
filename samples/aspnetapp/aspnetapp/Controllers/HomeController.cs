using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using aspnetapp.Models;
using System.Configuration;
using Serilog;

namespace aspnetapp.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult Index()
        {


            Log.Information("This is my second debug inside of index route"); 
            return View();
        }

        public IActionResult About()
        {

            Log.Information("This is my second debug inside of about route");
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {

            Log.Information("This is my second debug inside of contact route");
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {

            Log.Information("This is my second debug inside of privacy route");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            Log.Information("This is my second debug inside of Error route");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
