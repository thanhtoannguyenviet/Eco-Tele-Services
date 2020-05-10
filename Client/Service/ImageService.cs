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
    public class ImageService
    {
        public static Img UpdateImg(Img img)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PutAsync("https://localhost:44305/api/Image/updateImg/", new StringContent(
                    new JavaScriptSerializer().Serialize(img), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return img;
            }
            return null;
        }
        public static Img AddImg(Img img)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("https://localhost:44305/api/Image/addImg/", new StringContent(
                    new JavaScriptSerializer().Serialize(img), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return img;
            }
            return null;
        }
        public static List<Img> GetImageById(int entryid)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.GetAsync("https://localhost:44305/api/Image/getImageById/"+entryid);
                response.Wait();
                var result = response.Result;
                if (result.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<List<Img>>(result.Content.ReadAsStringAsync().Result);
                }
            }
            return null;
        }
    }
}