using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Server.Models.DTO;
using Server.Models.Entity;

namespace Server.Models.DAO
{
    public class ServiceDAO
    {
        private ExcellonEntities entities = new ExcellonEntities();
        public List<Service_> GetServiceOfStaff(int id) => entities.Service_.Where(s=>s.staffId == id).ToList();
        public bool RegisteService(AccountStaff accountStaff)
        {
            if (accountStaff.account != null && accountStaff.staff != null && accountStaff.services != null && accountStaff.services.Count() > 0)
            {
                foreach (var item in accountStaff.services)
                {
                    entities.Service_.Add(item);
                    entities.SaveChanges();
                }
                return true;
            }
            else return false;
        }
        public void updateService(Service_ service)
        {
            if (service != null)
            {
                entities.Entry(service);
                entities.SaveChanges();
            }
        }
        public void findWithRole(int role) { }
    }
}