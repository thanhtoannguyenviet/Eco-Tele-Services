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
                entities.Details.Add(detailPayment.details);
                entities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public void UpdateDetailStaff(Detail detail)
        {
            entities.Entry(detail).State=EntityState.Modified;
            entities.SaveChanges();
        }
        public Detail GetDetailFromStaff(int staffid)
        {
            return entities.Details.Where(d=>d.staffId==staffid).FirstOrDefault();
        }
    }
}