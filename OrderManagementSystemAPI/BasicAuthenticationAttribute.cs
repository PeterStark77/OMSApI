using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading;
using System.Security.Principal;

namespace OrderManagementSystemAPI
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if(actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse
                    (HttpStatusCode.Unauthorized);
            }
            else
            {
                string authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                // Decoding Base64 string email:Password
                string decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                //Spliting email and Password
                string[] EmailPasswordArray = decodedAuthenticationToken.Split(':');
                string email = EmailPasswordArray[0];
                string password = EmailPasswordArray[1];

                //Setting Current Principal of the Executing Thread to provided Buyer email 
                if (BuyerSecurity.Login(email, password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(email), null);
                }
                else if (AdministratorSecurity.Login(email, password))
                {
                    Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(email), null);
                }
                else
                {
                    actionContext.Response = actionContext.Request.CreateResponse
                    (HttpStatusCode.Unauthorized);
                }
            }
        }
    }
}