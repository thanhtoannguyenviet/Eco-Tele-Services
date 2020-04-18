using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class Account
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string pass_word { get; set; }
        public int? role_ { get; set; }
    }
}