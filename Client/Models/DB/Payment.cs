using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.DB
{
    public class Payment
    {
        public int id { get; set; }
        public string paymentId { get; set; }
        public decimal? totalMoney { get; set; }
        public DateTime? createDate { get; set; }
        public int? customerId { get; set; }
    }
}