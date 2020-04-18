using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Client.Models.DB;

namespace Client.Models.DTO
{
    public class DetailPayment
    {
        public Detail details { get; set; }
        public Payment payment { get; set; }
        public Customer customer { get; set; }

        public DetailPayment()
        {
            details = new Detail();
            payment = new Payment();
            customer = new Customer();
        }
        public DetailPayment(Detail details, Payment payment, Customer customer)
        {
            this.details = details;
            this.payment = payment;
            this.customer = customer;
        }
    }
}