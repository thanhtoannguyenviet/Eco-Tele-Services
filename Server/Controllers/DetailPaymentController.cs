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
    [RoutePrefix("api/payment")]
    public class DetailPaymentController : ApiController
    {
        DetailPaymentDAO detailPaymentDao = new DetailPaymentDAO();
        [HttpPost]
        [Route("createPayment/")]
        public IHttpActionResult createPayment([FromBody] Payment payment)
        {
            return Ok(detailPaymentDao.CreatePayments(payment));
        }
        [HttpPost]
        [Route("createDetail/")]
        public IHttpActionResult createOrder([FromBody] List<Detail> detail)
        {
            return Ok(detailPaymentDao.CreateDetail(detail));
        }
        [HttpPut]
        [Route("updateDetail/")]
        public IHttpActionResult UpdateStaff([FromBody] Detail detail)
        {
            detailPaymentDao.UpdateDetailStaff(detail);
            return Ok();
        }
       
    }
}