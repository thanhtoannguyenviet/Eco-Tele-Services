﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using Server.Common;
using Server.Models.Entity;

namespace Server.Models.DAO
{
    public class CustomerDAO 
    {
        private ExcellonEntities entities = new ExcellonEntities();
        private List<Detail> GetAllDetail(int id)
        {
           var q = entities.Customers
                .SelectMany(
                    c => entities.Payments,
                    (c, p) =>
                        new
                        {
                            c = c,
                            p = p
                        }
                )
                .SelectMany(
                    temp0 => entities.Details,
                    (temp0, d) =>
                        new
                        {
                            temp0 = temp0,
                            d = d
                        }
                )
                .Where(
                    temp1 =>
                        (((Int32?)(temp1.temp0.c.id) == temp1.temp0.p.customerId) &&
                         ((Int32?)(temp1.temp0.p.id) == temp1.d.paymentId) && ((Int32?)(temp1.temp0.c.id) == id)
                        )
                )
                .Select(
                    temp1 =>
                        new Detail
                        {
                            id = temp1.d.id,
                            staffId = temp1.d.staffId,
                            startDate = temp1.d.startDate,
                            endDate = temp1.d.endDate,
                            amountMoney = temp1.d.amountMoney,
                            statusOrder = temp1.d.statusOrder,
                            createDate = temp1.d.createDate,
                            paymentId = temp1.d.paymentId
                        }
                );
           return q.ToList();
        }
        private List<Payment> GetAllPayment(int id)
        {
            var q = entities.Customers
                .SelectMany(
                    c => entities.Payments,
                    (c, p) =>
                        new
                        {
                            c = c,
                            p = p
                        }
                )
                .SelectMany(
                    temp0 => entities.Details,
                    (temp0, d) =>
                        new
                        {
                            temp0 = temp0,
                            d = d
                        }
                )
                .Where(
                    temp1 =>
                        (((Int32?)(temp1.temp0.c.id) == temp1.temp0.p.customerId) &&
                         ((Int32?)(temp1.temp0.p.id) == temp1.d.paymentId) && ((Int32?)(temp1.temp0.c.id) == id)
                        )
                )
                .Select(
                    temp1 =>
                        new Payment
                        {
                            id = temp1.temp0.p.id,
                            paymentId = temp1.temp0.p.paymentId,
                            createDate = temp1.temp0.p.createDate,
                            customerId = temp1.temp0.p.customerId,
                            totalMoney = temp1.temp0.p.totalMoney
                        }
                );
            return q.ToList();
        } 
        private List<Staff> GetAllStaff(int id)
        {
            var q =
            entities.Details
                .SelectMany(
                    d => entities.Customers,
                    (d, c) =>
                        new
                        {
                            d = d,
                            c = c
                        }
                )
                .SelectMany(
                    temp0 => entities.Payments,
                    (temp0, p) =>
                        new
                        {
                            temp0 = temp0,
                            p = p
                        }
                )
                .SelectMany(
                    temp1 => entities.Staffs,
                    (temp1, s) =>
                        new
                        {
                            temp1 = temp1,
                            s = s
                        }
                )
                .Where(
                    temp2 =>
                        ((((Int32?)(temp2.s.id) == temp2.temp1.temp0.d.staffId) &&
                          (temp2.temp1.temp0.d.paymentId == (Int32?)(temp2.temp1.p.id))
                         ) &&
                         (temp2.temp1.p.customerId == (Int32?)(temp2.temp1.temp0.c.id))
                         && (temp2.temp1.p.customerId== id)
                        )
                )
                .Select(
                    temp2 =>
                        new Staff
                        {
                            id = temp2.s.id,
                            staffEmail = temp2.s.staffEmail,
                            staffPhone = temp2.s.staffPhone,
                            staffName = temp2.s.staffName,
                            staffBirtday = temp2.s.staffBirtday,
                            department = temp2.s.department,
                            mistakeCount = temp2.s.mistakeCount,
                            bankCard = temp2.s.bankCard,
                            status_ = temp2.s.status_
                        }
                )
                .Distinct();
            return q.ToList();
        }
        public List<Detail> GetAllDetailsHasPaging(int id,int page)
        {
            var q = GetAllDetail(id);
            PagedList<Detail> model = new PagedList<Detail>(q.ToList(),page,Constant.PAGESIZEDEFAULT);
            
            return model.ToList();
        }
        public int CountDetails(int id)
        {
            var q = GetAllDetail(id).Count;
            return q;
        }
        public List<Payment> GetAllPaymentHasPaging(int id, int page)
        {
            var q = GetAllPayment(id);
            PagedList<Payment> model = new PagedList<Payment>(q.ToList(), page, Constant.PAGESIZEDEFAULT);
            return model.ToList();
        }
        public int CountPayment(int id)
        {
            var q = GetAllDetail(id).Count;
            return q;
        }
        public List<Staff> GetAllStaffHasPaging(int id,int page)
        {
            var q = GetAllStaff(id);
            PagedList<Staff> model = new PagedList<Staff>(q.ToList(),page,Constant.PAGESIZEDEFAULT);
            return model.ToList();
        }
        public int CountStaff(int id)
        {
            return GetAllStaff(id).Count;
        }

    }
}