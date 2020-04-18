using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Server.Models.DAO;

namespace Server.Controllers
{
    [RoutePrefix("api/customer")]
    public class CustomerController : ApiController
    {
        private CustomerDAO customerDao = new CustomerDAO();
        [HttpGet]
        [Route("getAllDetail/{id}/{page}")]
        public IHttpActionResult GetAllDetail(int id,int page)
        {
            return    Ok(customerDao.GetAllDetailsHasPaging(id,page));
        }
        [HttpGet]
        [Route("countAllDetail/{id}")]
        public IHttpActionResult CountDetail(int id)
        {
            return Ok(customerDao.CountDetails(id));
        }
        [HttpGet]
        [Route("getAllPayment/{id}/{page}")]
        public IHttpActionResult getAllPayment(int id, int page)
        {
            return Ok(customerDao.GetAllPaymentHasPaging(id,page));
        }
        [HttpGet]
        [Route("countAllPayment/{id}")]
        public IHttpActionResult countPayment(int id)
        {
            return Ok(customerDao.CountPayment(id));
        }
        [HttpGet]
        [Route("getAllStaff/{id}/{page}")]
        public IHttpActionResult getAllStaff(int id,int page)
        {
            return Ok(customerDao.GetAllStaffHasPaging(id,page));
        }
        [HttpGet]
        [Route("countAllStaff/{id}")]
        public IHttpActionResult countAllStaff(int id,int page)
        {
            return Ok(customerDao.CountStaff(id));
        }
    }
}
