using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OMSDataAccessLayer;

namespace OrderManagementSystemAPI
{
    public class AdministratorSecurity
    {
        public static bool Login(string email, string password)
        {
            using (OMSdbEntities entities = new OMSdbEntities())
            {
                //comapring email and password where email is case insensative
                return entities.AppAdministrators.Any(admin => admin.AdminEmail.Equals(email,
                     StringComparison.OrdinalIgnoreCase) && admin.AdminPassword.Equals(password));
            }
        }
    }
}