using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Server.Models.DTO
{
    public class AccountDTO
    {
        public string Username { get;set;}
        public string Password { get;set;}
        public string Roles { get;set;}
    }
}