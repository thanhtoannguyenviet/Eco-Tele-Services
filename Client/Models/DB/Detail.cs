using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.DB
{
    public class Detail
    {
        public int id { get; set; }
        public int? customerId { get; set; }
        public int? staffId { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public decimal? amountMoney { get; set; }
        public int? statusOrder { get; set; }
        public DateTime? createDate { get; set; }
        public int? paymentId { get; set; }
    }
}