using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Server.Models.DTO;
using Server.Models.Entity;

namespace Server.Models.DAO
{
    public class DetailPaymentDAO
    {
        private ExcellonEntities entities=new ExcellonEntities();
        public bool CreateOrder(DetailPayment detailPayment)
        {
            try
            {
                entities.Payments.Add(detailPayment.payment);
                entities.SaveChanges();
                foreach (var item in detailPayment.details)
                {
                    item.paymentId = detailPayment.payment.id;
                    entities.Details.Add(item);
                    entities.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public void UpdateDetailStaff(Detail detail)
        {
            entities.Entry(detail).State=EntityState.Modified;
            entities.SaveChanges();
        }
    }
}