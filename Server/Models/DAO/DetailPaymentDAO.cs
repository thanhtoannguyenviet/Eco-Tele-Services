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
        public Payment CreatePayments(Payment payment)
        {
            entities.Payments.Add(payment);
            entities.SaveChanges();
            return payment;
        }
        public List<Detail> CreateDetail(List<Detail> lsDetail)
        {
            foreach (var item in lsDetail)
            {
                entities.Details.Add(item);
                entities.SaveChanges();
            }
            return lsDetail;
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