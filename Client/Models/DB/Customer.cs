using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models
{
    public class Customer
    {
        public int id { get; set; }
        public string headEmail { get; set; }
        public string headPhone { get; set; }
        public string headName { get; set; }
        public DateTime? headBirtday { get; set; }
        public string taxCode { get; set; }
        public string address_ { get; set; }
        public bool? checkOTP { get; set; }
        public int? active { get; set; }
    }
}