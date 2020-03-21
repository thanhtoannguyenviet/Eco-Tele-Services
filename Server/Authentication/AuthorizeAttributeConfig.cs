using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using Server.Models.DAO;
using Server.Models.DTO;
using Server.Security;

namespace Server.Authentication
{
    public class AuthorizeAttributeConfig : AuthorizeAttribute
    {
        const string BAuthResponseHeader = "WWW-Authenticate";
        const string BAuthResponseHeaderValue = "Basic";
        private AccountDTO ParseAuthorizationHeader(string authHeader)
        {
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(authHeader)).Split(new[] { ':' });
            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1]))
                return null;
            AccountDAO accountDao =new AccountDAO();
            AccountDTO account = accountDao.Login(credentials[0],credentials[1]);
            return account;
        }
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            try
            {
                AuthenticationHeaderValue authValue = actionContext.Request.Headers.Authorization;
                if (authValue != null && !String.IsNullOrWhiteSpace(authValue.Parameter)
                   && authValue.Scheme == BAuthResponseHeaderValue)
                {
                    AccountDTO account = ParseAuthorizationHeader(authValue.Parameter);
                    var myPrincipal = new MyPrincipal(account.Username);
                    if (!myPrincipal.IsInRole(Roles))
                    {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        actionContext.Response.Headers.Add(BAuthResponseHeader, BAuthResponseHeaderValue);
                        return;
                    }
                }
                else
                {
                        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                        actionContext.Response.Headers.Add(BAuthResponseHeader, BAuthResponseHeaderValue);
                        return;

                }
                
            }
            catch (Exception e)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
                actionContext.Response.Headers.Add(BAuthResponseHeader, BAuthResponseHeaderValue);
                return;
            }
        }
    }
}