using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartsheetsPIF2.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartsheetsPIF2.Controllers
{
    public class LoginController : Controller
    {
 
        public IActionResult Login()
        {
            return View();
        }

        public List<UserModel> PutValue()
        {
            var users = new List<UserModel>

            {
                new UserModel{ id=1, username="adunlop@criticalmass.com", password="123abc"},
                new UserModel{ id=2, username="ana.nicolov@criticalmass.com", password="helpme"},
                new UserModel{ id=3, username="Peter", password="another"},
                new UserModel{ id=4, username="Nicole", password="basic"}

            };
            return users;
        }

        [HttpPost]
        public IActionResult Verify(UserModel usr)
        {
            var u = PutValue();
            var ue = u.Where(u => u.username.Equals(usr.username));
            var up = u.Where(p => p.password.Equals(usr.password));

            if (up.Count()==1)

            //if (usr.username === ue && usr.password = up )

            {
                ViewBag.message = "Login Successful";

                return View("../Home/Index");
            }
            else
            {
                ViewBag.message = "Login Failed";
                return View("Login");
            }

        }


    }
}
