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
        [Route("createOrder/")]
        public IHttpActionResult createOrder([FromBody] DetailPayment detailPayment)
        {
            return Ok(detailPaymentDao.CreateOrder(detailPayment));
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