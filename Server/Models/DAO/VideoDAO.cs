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
    public class VideoDAO
    {
        private ExcellonEntities entities = new ExcellonEntities();
        public bool UploadVid(Vid vid)
        {
            try
            {
                entities.Entry(vid).State = EntityState.Modified;
                entities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool AddVidStaff(AccountStaff accountStaff)
        {
            try{
            foreach(var item in accountStaff.vids)
            {
                item.entryId = accountStaff.staff.id;
                entities.Vids.Add(item);
                entities.SaveChanges();
            }
            return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public List<Vid> GetVids (int entryid)
        {
            return entities.Vids.Where(e => e.entryId == entryid).ToList();
        }
    }
}