using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Server.Models.DAO;
using Server.Models.DTO;
using Server.Models.Entity;

namespace Server.Controllers
{
    [RoutePrefix("api/account")]
    public class AccountController : ApiController
    {
        private AccountDAO accountDao = new AccountDAO();
        [HttpPost]
        [Route("checkLogin")]
        public IHttpActionResult Post([FromBody]Account account)
        {
           var action =accountDao.Login(account.userName,account.pass_word);
           return Ok(action);
        }
        [HttpPost]
        [Route("Staff/Registe")]
        public IHttpActionResult RegisteStaff([FromBody]AccountStaff accountCustomer)
        {
            var action = accountDao.RegisteAccountStaff(accountCustomer);
            if (!action)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("Customer/Registe")]
        public IHttpActionResult RegisteCustomer([FromBody]AccountCustomer accountCustomer)
        {
            var action = accountDao.RegisteAccountCustomer(accountCustomer);
            if (!action)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("Customer/Login")]
        public IHttpActionResult LoginCustomer([FromBody] Account account)
        {
            var customer = accountDao.LoginCustomer(account);
            if(customer!=null)
                return Ok(customer);
            else return NotFound();
        }
        [HttpPost]
        [Route("Staff/Login")]
        public IHttpActionResult LoginStaff([FromBody] Account account)
        {
            var staff = accountDao.LoginStaff(account);
            if(staff!=null)
                return Ok(staff);
            else return NotFound();
        }
        [HttpPost]
        [Route("Customer/UpdateInformation")]
        public IHttpActionResult UpdateInfoCustomer([FromBody]AccountCustomer accountCustomer)
        {
            var action = accountDao.UpdateInformation(accountCustomer);
            if (!action)
            {
                return Ok();
            }
            return BadRequest();
        }
        [HttpPost]
        [Route("Staff/UpdateInformation")]
        public IHttpActionResult UpdateInfoCustomer([FromBody]AccountStaff accountStaff)
        {
            var action = accountDao.UpdateInformation(accountStaff);
            if (action!=null)
            {
                return Ok(accountStaff);
            }
            return BadRequest();
        }
    }
}
