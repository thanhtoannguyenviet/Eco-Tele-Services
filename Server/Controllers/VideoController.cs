using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Google.Apis.Auth.OAuth2;
using Server.Models.DAO;
using Server.Models.DTO;
using Server.Models.Entity;

namespace Server.Controllers
{
    [RoutePrefix("api/Video")]
    public class VideoController : ApiController
    {
        VideoDAO videoDao = new VideoDAO();
        [HttpPut]
        [Route("addVid/")]
        public IHttpActionResult AddVid([FromBody] AccountStaff accountStaff)
        {
            bool flag = videoDao.AddVidStaff(accountStaff);
            if (flag == true)
                return Ok();
            else return NotFound();
        }
        [HttpPut]
        [Route("updateVid/")]
        public IHttpActionResult UpdateVid([FromBody] Vid vid)
        {
            bool flag = videoDao.UploadVid(vid);
            if (flag == true)
                return Ok();
            else return NotFound();
        }
        [HttpGet]
        [Route("getVideoById/{entryid}")]
        public IHttpActionResult GetVidList(int entryid)
        {
            return Ok(videoDao.GetVids(entryid));
        }
    }
}