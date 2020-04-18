using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Server.Models.DTO;
using Server.Models.Entity;

namespace Server.Models.DAO
{
    public class ServiceDAO
    {
        private ExcellonEntities entities = new ExcellonEntities();
        public List<Service_> GetServiceOfStaff(int id) => entities.Service_.Where(s=>s.staffId == id).ToList();
        public bool RegisteService(AccountStaff accountStaff)
        {
            if (accountStaff.account != null && accountStaff.staff != null && accountStaff.services != null && accountStaff.services.Count() > 0)
            {
                foreach (var item in accountStaff.services)
                {
                    item.staffId=accountStaff.account.id;
                    entities.Service_.Add(item);
                    entities.SaveChanges();
                }
                return true;
            }
            else return false;
        }
        public void removeService(Service_ service)
        {
            if (service != null)
            {
                var p = entities.Service_.SingleOrDefault(x=>x.id==service.id);
                entities.Service_.Remove(p);
                entities.SaveChanges();
            }
        }
        public List<AccountStaff> findWithRole(int role)
        {
            var obj = (from a in entities.Accounts
                from s in entities.Staffs
                where
                    a.id == s.id &&
                    a.role_<=role
                select new 
                {
                    id = a.id,
                    userName = a.userName,
                    pass_word = a.pass_word,
                    role_ = a.role_,
                    Column1 = s.id,
                    staffEmail = s.staffEmail,
                    staffPhone = s.staffPhone,
                    staffName = s.staffName,
                    staffBirtday = s.staffBirtday,
                    department = s.department,
                    mistakeCount = s.mistakeCount,
                    bankCard = s.bankCard,
                    status_ = s.status_,
                }).Distinct();
            var lsAccountStaff = new List<AccountStaff>();
            foreach (var item in  obj)
            {
                var acc = new AccountStaff()
                {
                    account = new Account
                    {
                        id = item.id,
                        userName = item.userName,
                        pass_word = item.pass_word,
                        role_ = item.role_
                    },
                    staff = new Staff
                    {
                        id = item.id,
                        bankCard = item.bankCard,
                        department = item.department,
                        mistakeCount = item.mistakeCount,
                        staffBirtday = item.staffBirtday,
                        staffEmail = item.staffEmail,
                        staffName = item.staffName,
                        staffPhone = item.staffPhone,
                        status_ = item.status_
                    }
                };
                lsAccountStaff.Add(acc);
            }
            return lsAccountStaff;
        }
    }
}