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
    public class ImageDAO
    {
        private ExcellonEntities entities = new ExcellonEntities();
        public bool UploadImg(Img img)
        {
            try
            {
                entities.Entry(img).State=EntityState.Modified;
                entities.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }
        public bool AddImgsStaff(AccountStaff accountStaff)
        {
            try { 
            foreach (var item in accountStaff.imgs)
            {
                item.entryName = Constant.STAFFTABLE;
                item.entryId = accountStaff.staff.id;
                entities.Imgs.Add(item);
                entities.SaveChanges();
            }
            return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}