using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartsheetsproject.Models;

namespace SmartsheetsPIF2.Services
{
    public class SecurityService
    {
        List<UserModel> userAccess = new List<UserModel>();
 

        public SecurityService()
        {
            userAccess.Add(new UserModel { Id = 0, Username = "adunlop@criticalmass.com", Password = "cmpass" });
            userAccess.Add(new UserModel { Id = 1, Username = "ana.nicolov@criticalmass.com", Password = "cmpass" });
            userAccess.Add(new UserModel { Id = 2, Username = "peter.ghering@criticalmass.com", Password = "cmpass" });

        }
        public bool IsValid(UserModel user)
        {

            return userAccess.Any(usr => usr.Username == user.Username && usr.Password == user.Password);
           
        }
    }
}
