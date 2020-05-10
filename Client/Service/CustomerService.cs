using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Client.Models;
using Client.Models.DB;
using Newtonsoft.Json;

namespace Client.Service
{
    public class CustomerService
    {
        public static List<Detail> GetAllDetail(Customer customer, int pageNumber)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/api/customer/getAllDetail/" + customer.id + "/" + pageNumber);

                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = JsonConvert.DeserializeObject<List<Detail>>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return null;
            }
        }
        public static int CountAllDetail(Customer customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/api/customer/countAllDetail/" + customer.id);

                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return -1;
            }
        }
        public static List<Payment> GetAllPayment(Customer customer, int pageNumber)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/api/customer/getAllPayment/" + customer.id + "/" + pageNumber);
                var responseTask = client.GetAsync(client.BaseAddress);

                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {

                    var readTask = JsonConvert.DeserializeObject<List<Payment>>(result.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return null;
            }
        }
        public static int CountAllPayment(Customer customer)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/api/customer/countAllPayment/" + customer.id);

                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                }
                return -1;
            }
        }
        public static List<Staff> GetAllStaff(Customer customer, int pageNumber)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/api/customer/getAllStaff/"+customer.id+"/"+pageNumber);
                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();
                var result= responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Staff>>(result.Content.ReadAsStringAsync().Result);
                }
            }
            return null;
        }
        public static int CountAllStaff(Customer customer)
        {

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44305/api/customer/countAllStaff/" + customer.id);

                var responseTask = client.GetAsync(client.BaseAddress);
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<int>(result.Content.ReadAsStringAsync().Result);
                }
                return -1;
            }
        }
        public static AccountCustomer UpdateCustomer(AccountCustomer accountCustomer)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("https://localhost:44305/api/account/Staff/UpdateInformation", new StringContent(
                    new JavaScriptSerializer().Serialize(accountCustomer), Encoding.UTF8, "application/json")).Result;
                if (response.StatusCode == HttpStatusCode.OK)
                    return accountCustomer;
            }
            return null;
        }
        public static List<Service_> GetServiceByIdStaff(int id) => StaffService.GetServiceOfStaff(id);
    }
}