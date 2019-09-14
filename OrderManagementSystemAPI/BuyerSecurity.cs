using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using OMSDataAccessLayer;

namespace OrderManagementSystemAPI
{
    public class BuyerSecurity
    {
        public static bool Login(string email, string password)
        {
            using(OMSdbEntities entities = new OMSdbEntities())
            {
                //comapring email and password where email is case insensative
                return entities.Buyers.Any(buyer => buyer.Email.Equals(email,
                     StringComparison.OrdinalIgnoreCase) && buyer.Password.Equals(password));
            }
        }
    }
}