using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Server.Common;
using Server.Models.DTO;
using Server.Models.Entity;

namespace Server.Models.DAO
{
    public class AccountDAO
    {
        private ExcellonEntities entities = new ExcellonEntities();
        private List<Detail>GetDetailOfPayment(int id)
        {
           var q= entities.Details.SelectMany(d => entities.Payments, (d, p) => new {d, p})
               .SelectMany(@t => entities.Customers, (@t, c) => new {@t, c})
               .Where(@t => @t.c.id == @t.@t.p.customerId && @t.@t.d.paymentId == @t.@t.p.id && @t.c.id == id)
               .Select(@t => new
               {
                   id = @t.@t.d.id,
                   staffId = @t.@t.d.staffId,
                   startDate = @t.@t.d.startDate,
                   endDate = @t.@t.d.endDate,
                   amountMoney = @t.@t.d.amountMoney,
                   statusOrder = @t.@t.d.statusOrder,
                   createDate = @t.@t.d.createDate,
                   paymentId = @t.@t.d.paymentId
               });
            var ls = new List<Detail>();
            foreach (var item in q)
            {
                var detail = new Detail()
                {
                    id = item.id,
                    paymentId = item.paymentId,
                    amountMoney = item.amountMoney, 
                    createDate = item.createDate,
                    endDate = item.endDate,
                    staffId = item.staffId,
                    startDate = item.startDate,
                    statusOrder = item.statusOrder
                };
                ls.Add(detail);
            }
            return ls;
        }
        public AccountDTO Find(string username)
        {
            IQueryable<AccountDTO> objects;
            objects = entities.Accounts.Where(a => a.userName.Equals(username))
                .Select(a =>
                    new AccountDTO() {Password = a.pass_word, Username = a.userName, Roles = ROLE.GetValue(a.role_)});
            return objects.SingleOrDefault();
         }
        public Account Login(string username,string password)
        {

            return entities.Accounts
                .FirstOrDefault(a => a.userName.Equals(username)&&a.pass_word.Equals(password));
            
        }
        public String CheckLogin(string username, string password)
        {
            var objects = entities.Accounts
                .FirstOrDefault(a => a.userName.Equals(username) && a.pass_word.Equals(password));
            if(objects.role_==1)
                return Constant.CUSTOMERTABLE;
            else if(objects.role_>1)
                return Constant.STAFFTABLE;
            else return Constant.WRONGPASSWORD;
        }
        public AccountStaff LoginStaff(Account account)
        {
            AccountStaff accountStaff = new AccountStaff();
            accountStaff.account = account;
            accountStaff.staff = entities.Staffs.Find(account.id);
            accountStaff.imgs= entities.Imgs.Where(a=>a.entryId==account.id&&a.entryName==Constant.STAFFTABLE).ToList();
            accountStaff.vids = entities.Vids.Where(a => a.entryId == account.id).ToList();
            accountStaff.services=entities.Service_.Where(a=>a.staffId==account.id).ToList();
            accountStaff.details=entities.Details.Where(a=>a.staffId==account.id).ToList();
            return accountStaff;
        }
        public AccountCustomer LoginCustomer(Account account)
        {
            AccountCustomer accountCustomer = new AccountCustomer();
            accountCustomer.account=account;
            accountCustomer.customer = entities.Customers.Find(account.id);
            accountCustomer.img=entities.Imgs.Where(a=>a.entryId==account.id&&a.entryName==Constant.CUSTOMERTABLE).FirstOrDefault();
            accountCustomer.payments = entities.Payments.Where(a=>a.customerId==account.id).ToList();
            accountCustomer.details= GetDetailOfPayment(account.id);
            return accountCustomer;
        }
        public bool RegisteAccountCustomer(AccountCustomer accountCustomer)
        {
            try
            {
                var check = entities.Accounts.Where(a => a.userName == accountCustomer.account.userName).Count();
                if (check > 0)
                {
                    accountCustomer.messsage = Constant.ERROREXIST;
                    return false;
                }
                entities.Accounts.Add(accountCustomer.account);
                entities.SaveChanges();
                accountCustomer.customer.id = accountCustomer.account.id;
                entities.Customers.Add(accountCustomer.customer);
                entities.SaveChanges();
                if (accountCustomer.img != null)
                {
                    accountCustomer.img.entryId = accountCustomer.account.id;
                    accountCustomer.img.entryName = Constant.CUSTOMERTABLE;
                    entities.Imgs.Add(accountCustomer.img);
                    entities.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                accountCustomer.messsage=e.Message;
                return false;
            }
        }
        public bool RegisteAccountStaff(AccountStaff accountStaff)
        {
            try
            {
                var check = entities.Accounts.Where(a => a.userName == accountStaff.account.userName).Count();
                if (check > 0)
                {
                    accountStaff.messsage = Constant.ERROREXIST;
                    return false;
                }
                entities.Accounts.Add(accountStaff.account);
                entities.SaveChanges();
                accountStaff.staff.id = accountStaff.account.id;
                entities.Staffs.Add(accountStaff.staff);
                entities.SaveChanges();
                if (accountStaff.imgs != null)
                {
                    accountStaff.imgs[0].entryId = accountStaff.account.id;
                    accountStaff.imgs[0].entryName = Constant.CUSTOMERTABLE;
                    entities.Imgs.AddRange(accountStaff.imgs);
                    entities.SaveChanges();
                }
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                accountStaff.messsage = e.Message;
                return false;
            }
        }
        public bool UpdateInformation(AccountCustomer accountCustomer)
        {
            try
            {
               entities.Entry(accountCustomer.account).State = EntityState.Modified;
               entities.SaveChanges();
               accountCustomer.customer.id = accountCustomer.account.id;
               entities.Entry(accountCustomer.customer).State = EntityState.Modified;
               entities.SaveChanges();
               if (accountCustomer.img != null)
               {
                   accountCustomer.img.entryId = accountCustomer.account.id;
                   accountCustomer.img.entryName = Constant.CUSTOMERTABLE;
                   entities.Entry(accountCustomer.img).State = EntityState.Modified;
                   entities.SaveChanges();
               }
               return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                accountCustomer.messsage = e.Message;
                return false;
            }
        }
        public AccountStaff UpdateInformation(AccountStaff accountStaff)
        {
            try
            {
                entities.Entry(accountStaff.account).State = EntityState.Modified;
                entities.SaveChanges();
                accountStaff.staff.id = accountStaff.account.id;
                entities.Entry(accountStaff.staff).State = EntityState.Modified;
                entities.SaveChanges();
                return accountStaff;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                accountStaff.messsage = e.Message;
                return null;
            }
        }
    }
}