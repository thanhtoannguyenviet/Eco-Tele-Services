using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using Client.Models.DB;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Ocsp;
using PayPal.Api;
using Payment = Client.Models.DB.Payment;

namespace Client.Service
{
    public class PaymentDetailService
    {
        public static List<Detail> CreateDetail(List<Detail> account)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("https://localhost:44305/api/payment/createDetail/", new StringContent(
                    new JavaScriptSerializer().Serialize(account), Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    return JsonConvert.DeserializeObject<List<Detail>>(response.Content.ReadAsStringAsync().Result);
                }
                return null;
            }
        }
        public static Payment CreatePayment(Payment payment)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                var response = client.PostAsync("https://localhost:44305/api/payment/createPayment/", new StringContent(
                    new JavaScriptSerializer().Serialize(payment), Encoding.UTF8, "application/json")).Result;
                if (response.IsSuccessStatusCode)
                {
                    var readTask = JsonConvert.DeserializeObject<Payment>(response.Content.ReadAsStringAsync().Result);
                    return readTask; // nếu return ngay đây sao k return lại method trên luôn
                }
                return null;
            }
        }
        public static PayPal.Api.Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            PayPal.Api.Payment payment;
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
           payment = new PayPal.Api.Payment()
            {
                id = paymentId
            };
            return payment.Execute(apiContext, paymentExecution);
        }
        public static PayPal.Api.Payment CreatePaymentPaypalAPI(APIContext apiContext, string redirectUrl, List<Detail> ls,String cancelURL)
        {
            //create itemlist and add item objects to it
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
            //Adding Item Details like name, currency, price etc
            decimal totalAll = 0;
            foreach (var item in ls)
            {
                itemList.items.Add(new Item()
                {
                    name = item.staffId.ToString(),
                    currency = "USD",
                    price = (item.amountMoney / decimal.Parse(((item.endDate.Value - item.startDate.Value).TotalDays + 1).ToString())).ToString(),
                    quantity = ((item.endDate.Value - item.startDate.Value).TotalDays + 1).ToString()
                });
                totalAll += item.amountMoney.Value;
            };
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            // Configure Redirect Urls here with RedirectUrls object  
            var redirUrls = new RedirectUrls()
            {
                cancel_url = cancelURL,
                return_url = redirectUrl
            };
            //Final amount with details  
            var amount = new Amount()
            {
                currency = "USD",
                total = totalAll.ToString()
            };
            var transactionList = new List<Transaction>();
            // Adding description about the transaction  
            transactionList.Add(new Transaction()
            {
                description = "Transaction description",
                invoice_number = Client.Common.FormatJsonString.RANDOMINVOICENUMBER(), //Generate an Invoice No  
                amount = amount,
                item_list = itemList
            });
            PayPal.Api.Payment payment = new PayPal.Api.Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            // Create a payment using a APIContext  
            return payment.Create(apiContext);
        }
    }
}