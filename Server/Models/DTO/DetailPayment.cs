using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Server.Models.Entity;

namespace Server.Models.DTO
{
    public class DetailPayment
    {
        public Detail details { get; set; }
        public Payment payment { get; set; }
        public Customer customer { get; set; }

      
    }
}