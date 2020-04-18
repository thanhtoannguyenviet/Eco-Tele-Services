using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Google;
using static Client.Common.CODESTATUS;
namespace Client.Service
{
    public class YoutubeService
    {
        private async Task Run()
        {
            var credential;
            using(var stream = new FileStream(ApiKey, FileMode.Open, FileAccess.Read))
            {
                credential = await GoogleWebAuthorizationBroker
            }
        }
        #region MyRegion

        

        #endregion
    }
}