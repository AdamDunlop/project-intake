using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Smartsheetsproject.Models;

namespace SmartsheetsPIF2.Services
{
    public class SecurityService
    {
        SecurityDAO securityDAO = new SecurityDAO();

        public SecurityService()
        {
            
        }
        public bool IsValid(UserModel user)
        {
            return securityDAO.FindUser(user);
           
        }
    }
}
