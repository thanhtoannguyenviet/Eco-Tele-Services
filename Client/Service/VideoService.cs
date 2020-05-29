using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Client.Models.DB;
using Newtonsoft.Json;

namespace Client.Service
{
    public class VideoService
    {
        public static Vid UpdateVid(Vid vid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PutAsync("https://localhost:44305/api/Video/updateVid/", new StringContent(
                    new JavaScriptSerializer().Serialize(vid), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return vid;
            }
            return null;
        }
        public static Vid AddVid(Vid vid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("https://localhost:44305/api/Video/addVid/", new StringContent(
                    new JavaScriptSerializer().Serialize(vid), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return vid;
            }
            return null;
        }
        public static List<Vid> GetVideoById(int entryid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.GetAsync("https://localhost:44305/api/Video/getVideoById/" + entryid);
                response.Wait();
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<List<Vid>>(result.Content.ReadAsStringAsync().Result);
                }
            }
            return null;
        }
    }
}