using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartsheetsPIF.Controllers
{
    public class PDSController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
