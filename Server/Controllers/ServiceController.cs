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
    [RoutePrefix("api/staff")]
    public class ServiceController : ApiController
    {
        ServiceDAO serviceDao = new ServiceDAO();
        [HttpPost]
        [Route("registerService/")]
        public IHttpActionResult registerService([FromBody] AccountStaff accountStaff)
        {
            var flag = serviceDao.RegisteService(accountStaff);
            if(flag) return Ok();
            else return BadRequest();
        }
        [HttpPut]
        [Route("updateService/")]
        public IHttpActionResult updateService([FromBody] Service_ service)
        {
            serviceDao.removeService(service);
            return Ok();
        }
        [HttpGet]
        [Route("findWithRole/{role}")]
        public IHttpActionResult findWithRole(int role)
        {
            if (role < 20)
            {
                return BadRequest(ModelState);
            }

            try
            {
                return Ok(serviceDao.findWithRole(role));
            }
            catch (Exception e)
            {
                return Json(e.Message);
            }
        }
        [HttpGet]
        [Route("getServiceWithStaffId/{id}")]
        public IHttpActionResult getServiceOfStaff(int id)
        {
            return Ok(serviceDao.GetServiceOfStaff(id));
        }
    }
}
