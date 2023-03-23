using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Smartsheetsproject.Models;
using SmartsheetsPIF2.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Smartsheetsproject.Controllers
{
    public class LoginController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    

    public IActionResult ProcessLogin(UserModel userModel)
    {


        SecurityService checkValidUser = new SecurityService();

        if (checkValidUser.IsValid(userModel))
        {
            return View("../Home/Nifti", userModel);
        }
        else
        {
            return View("Index", userModel);
        }
    }

    }
}
