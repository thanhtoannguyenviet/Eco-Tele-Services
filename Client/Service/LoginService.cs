using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Client.Models;
using Client.Models.DTO;
using Newtonsoft.Json;

namespace Client.Service
{
    public class LoginService
    {
        public static Account CheckLogin(Account account)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("https://localhost:44305/api/account/checkLogin/", new StringContent(
                    new JavaScriptSerializer().Serialize(account), Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    var readTask = JsonConvert.DeserializeObject<Account>(response.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return null;
            }
        }
        public static AccountStaff LoginStaff(Account account)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("https://localhost:44305/api/account/Staff/Login/", new StringContent(
                    new JavaScriptSerializer().Serialize(account), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var readTask = JsonConvert.DeserializeObject<AccountStaff>(response.Content.ReadAsStringAsync().Result);

                    return readTask;
                }
            }
            return null;
        }
        public static AccountCustomer LoginCustomer(Account account)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("https://localhost:44305/api/account/Customer/Login/", new StringContent(
                    new JavaScriptSerializer().Serialize(account), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return JsonConvert.DeserializeObject<AccountCustomer>(response.Content.ReadAsStringAsync().Result);
                }
            }
            return null;
        }
    }
}