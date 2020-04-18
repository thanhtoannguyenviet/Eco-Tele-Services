using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Server.Models.DAO;

namespace Server.Controllers
{
    [RoutePrefix("api/staff")]
    public class StaffController : ApiController
    {
        private StaffDAO staffDao= new StaffDAO();
        [HttpGet]
        [Route("getDetail/{id}")]
        public IHttpActionResult GetDetail(int id)
        {
            return Ok(staffDao.getDetail(id));
        }
        
    }
}