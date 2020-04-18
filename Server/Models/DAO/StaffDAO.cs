using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Server.Models.DTO;
using Server.Models.Entity;

namespace Server.Models.DAO
{
    public class StaffDAO
    {
        private ExcellonEntities entities = new ExcellonEntities();
        public List<DetailPayment> getDetail(int staffid)
        {
            var q = (from Payments in entities.Payments
                from Details in entities.Details
                from Customers in entities.Customers
                where
                    Customers.id == Payments.customerId &&
                    Details.paymentId == Payments.id
                select new
                {
                    id = Details.id,
                    staffId = Details.staffId,
                    startDate = Details.startDate,
                    endDate = Details.endDate,
                    amountMoney = Details.amountMoney,
                    statusOrder = Details.statusOrder,
                    createDate = Details.createDate,
                    paymentId = Details.paymentId,
                    Column1 = Customers.id,
                    headEmail = Customers.headEmail,
                    headPhone = Customers.headPhone,
                    headName = Customers.headName,
                    headBirtday = Customers.headBirtday,
                    taxCode = Customers.taxCode,
                    address_ = Customers.address_,
                    checkOTP = Customers.checkOTP,
                    active = Customers.active,
                    Column2 = Payments.id,
                    Column3 = Payments.paymentId,
                    totalMoney = Payments.totalMoney,
                    Column4 = Payments.createDate,
                    customerId = Payments.customerId
                }).ToList();
            List<DetailPayment> ls = new List<DetailPayment>();
            foreach (var item in q)
            {
                DetailPayment detail = new DetailPayment();
                detail.details = new Detail()
                {
                    id = item.id,
                    staffId = item.staffId,
                    startDate = item.startDate,
                    endDate = item.endDate,
                    amountMoney = item.amountMoney,
                    statusOrder = item.statusOrder,
                    createDate = item.createDate,
                    paymentId = item.paymentId,
                };
                detail.payment= new Payment()
                {
                    id = item.Column2,
                    paymentId = item.Column3,
                    totalMoney = item.totalMoney,
                    createDate = item.createDate,
                    customerId = item.customerId
                };
                detail.customer= new Customer()
                {
                    id =item.Column1 ,
                    headEmail = item.headEmail,
                    headPhone = item.headPhone,
                    headName = item.headName,
                    headBirtday = item.headBirtday,
                    taxCode = item.taxCode,
                    address_ = item.address_,
                    checkOTP = item.checkOTP,
                };
                ls.Add(detail);
            }
            
            return ls;
        }
    }
}