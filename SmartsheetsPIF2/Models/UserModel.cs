using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SmartsheetsPIF2.Models
{
    public class UserModel
    {
        
        public int id { get; set; }

        [Required(ErrorMessage = "Please enter your email")]
        public string username { get; set; }

        [Required(ErrorMessage = "Please enter your password")]
        public string password { get; set; }

        //[Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }
}
