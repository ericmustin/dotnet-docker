using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using aspnetapp.Models;
using System.Configuration;
using log4net;
using log4net.Config;

namespace aspnetapp.Controllers
{
    public class HomeController : Controller
    {
        private static ILog logger = LogManager.GetLogger(typeof(Program));

        public IActionResult Index()
        {
            logger.Debug("This is my second debug inside of index route"); 
            return View();
        }

        public IActionResult About()
        {
            logger.Debug("This is my second debug inside of about route");
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            logger.Debug("This is my second debug inside of contact route");
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            logger.Debug("This is my second debug inside of privacy route");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            logger.Debug("This is my second debug inside of Error route");
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
