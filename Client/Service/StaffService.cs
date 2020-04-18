using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Client.Models.DB;
using Client.Models.DTO;
using Newtonsoft.Json;

namespace Client.Service
{
    public class StaffService
    {
        public static AccountStaff RegisterStaff(AccountStaff accountStaff)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("https://localhost:44305/api/account/Staff/Registe", new StringContent(
                    new JavaScriptSerializer().Serialize(accountStaff), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return accountStaff;
            }
            return null;
        }
        public static string RegisterService(AccountStaff accountStaff)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("https://localhost:44305/api/staff/registerService/", new StringContent(
                    new JavaScriptSerializer().Serialize(accountStaff), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return "Regist Completed";
            }
            return null;
        }
        public static string UpdateService(Service_ service)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PutAsync("https://localhost:44305/api/staff/updateService/", new StringContent(
                    new JavaScriptSerializer().Serialize(service), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return "UpdateCompleted";
            }
            return null;
        }
        public static Detail UpdateDetail(Detail detail)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PutAsync("https://localhost:44305/api/payment/updateDetail/", new StringContent(
                    new JavaScriptSerializer().Serialize(detail), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var readTask = JsonConvert.DeserializeObject<Detail>(response.Content.ReadAsStringAsync().Result);

                    return readTask;
                }
            }
            return null;
        }
        public static AccountStaff UpdateStaff(AccountStaff accountStaff)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("https://localhost:44305/api/account/Staff/UpdateInformation", new StringContent(
                    new JavaScriptSerializer().Serialize(accountStaff), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return accountStaff;
            }
            return null;
        }
        public static List<DetailPayment> GetDetailPayments(int staffid)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/api/staff/getDetail/" + staffid);

                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = JsonConvert.DeserializeObject<List<DetailPayment>>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return null;
            }
        }
        public static List<AccountStaff> FindWithRole(int? role)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress= new Uri("https://localhost:44305/api/staff/findWithRole/" + role);
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<AccountStaff>>(result.Content.ReadAsStringAsync().Result);
                }
                return null;
            }
        }
        public static List<Service_> GetServiceOfStaff(int id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/api/staff/getServiceWithStaffId/" + id);
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Service_>>(result.Content.ReadAsStringAsync().Result);
                }
                return null;
            }
        }
    }
}