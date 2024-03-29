﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Smartsheetsproject2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Smartsheetsproject2.Controllers
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

        public IActionResult Nifti()
        {
            return View();
        }

        public IActionResult Finance()
        {
            return View();
        }

        public IActionResult Pipeline()
        {
            return View();
        }


        public IActionResult Reports()
        {
            return View();
        }

        public IActionResult MKS()
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
