using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SmartsheetsPIF2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SmartsheetsPIF2.Controllers
{
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
            return View();
        }


        public IActionResult Metrics()
        {
            return View();
        }


        public IActionResult BidSheet()
        {
            return View();
        }

        public IActionResult LATAM()
        {
            return View();
        }

        public IActionResult Process()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
