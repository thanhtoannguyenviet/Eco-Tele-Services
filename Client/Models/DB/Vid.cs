using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Client.Models.DB
{
    public class Vid
    {
        public int Id { get; set; }//id

        public string path_ { get; set; }//path

        public int entryId { get; set; }
        public Vid (int entryId)
        {
            path_ = "~Videos/piano.mp4";
            this.entryId = entryId;
        }

    }
}