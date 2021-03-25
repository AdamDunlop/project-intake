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

        [HttpGet]
        public IActionResult Details()
        {
            return View();
        }
        [HttpGet]
        public IActionResult List()
        {
            return View();
            //var sheet = LoadSheet(sheetId, initSheet());
            //return View(GetRows(sheet));
        }

        [HttpGet]
        public IActionResult Edit()
        {
            //DisplayModel model = new DisplayModel();
            //model = GetProjectEdit(id);
            //return View(model);
            return View();

        }

    }


}
