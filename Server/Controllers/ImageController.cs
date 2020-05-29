using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Server.Models.DAO;
using Server.Models.DTO;
using Server.Models.Entity;

namespace Server.Controllers
{
    [RoutePrefix("api/Image")]
    public class ImageController : ApiController
    {
        ImageDAO imageDao = new ImageDAO();
        [HttpPut]
        [Route("addImg/")]
        public IHttpActionResult AddImg([FromBody] AccountStaff accountStaff)
        {
            bool flag =imageDao.AddImgsStaff(accountStaff);
            if(flag ==true)
                return Ok();
            else return NotFound();
        }
        [HttpPut]
        [Route("updateImg/")]
        public IHttpActionResult UpdateImg([FromBody] Img img)
        {
            bool flag = imageDao.UploadImg(img);
            if(flag==true)
                return Ok();
            else return NotFound();
        }
        [HttpGet]
        [Route("getImageById/{entryid}")]
        public IHttpActionResult GetImgList(int entryid)
        {
            return Ok(imageDao.GetImg(entryid));
        }
    }
}
