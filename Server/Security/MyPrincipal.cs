using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Server.Models.DAO;
using Server.Models.DTO;

namespace Server.Security
{
    public class MyPrincipal:IPrincipal
    {
        private AccountDTO account;
        public bool IsInRole(string role)
        {
            return  account.Roles!=null?true : false;
        }

        public IIdentity Identity { get; set;}
        public MyPrincipal(string username)
        {
            this.Identity = new GenericIdentity(username);

            this.account = new AccountDAO().Find(username);
        }
    }
}