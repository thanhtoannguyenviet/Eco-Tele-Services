using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Client.Models;
using Client.Models.DB;
using Client.Models.DTO;
using Newtonsoft.Json;
using PayPal.Api;
using static Client.Service.CustomerService;
using static Client.Service.PaymentDetailService;
using static Client.Service.ImageService;
using static Client.Service.VideoService;
using Payment = Client.Models.DB.Payment;

namespace Client.Controllers
{
    public class CustomerController : Controller
    {
        // GET: Customer
        public ActionResult Index()
        {
            Session["CountPage"]=1;
            var accountCustomer = JsonConvert.DeserializeObject<AccountCustomer>(Request.Cookies["Account"]?.Value);
            if (Session["cart"]!=null)
            {
                List<Detail> ls = Session["cart"] as List<Detail>;
            }
            List<Staff> lsStaff = GetAllStaff(accountCustomer.customer, 0);
            List<AccountStaff> lsAccountStaff = new List<AccountStaff>();
            foreach (var item in  lsStaff)
            { 
                var accountStaff = new AccountStaff
                {
                    staff = item,
                    services = GetServiceByIdStaff(item.id),
                    imgs = GetImageById(item.id),
                    vids=GetVideoById(item.id)
                    
                    
                };
                lsAccountStaff.Add(accountStaff);
            }
            return View(lsAccountStaff);
        }
        [HttpPost]
        public ActionResult LazyLoad(int page)
        {
            var accountCustomer = JsonConvert.DeserializeObject<AccountCustomer>(Request.Cookies["Account"]?.Value);
            List<Staff> lsStaff = GetAllStaff(accountCustomer.customer, page);
            List<AccountStaff> lsAccountStaff = new List<AccountStaff>();
            foreach (var item in lsStaff)
            {
                var accountStaff = new AccountStaff
                {
                    staff = item,
                    services = GetServiceByIdStaff(item.id),
                    imgs = GetImageById(item.id),
                    vids=GetVideoById(item.id)
                };
                lsAccountStaff.Add(accountStaff);
            }
            Session["CountPage"] = page + 1;
            return PartialView("_LazyLoad",lsAccountStaff);
        }
        public ActionResult AddToCart(int staffId, string dateStt, string dateEnd, string amountMoney)
        {
            DateTime dateStart = DateTime.ParseExact(dateStt, "yyyy-MM-dd",null);
            DateTime dateEnds = DateTime.ParseExact(dateStt, "yyyy-MM-dd", null);
            Detail detail = new Detail { staffId = staffId, startDate = dateStart, endDate = dateEnds, amountMoney = Decimal.Parse(amountMoney)};
            var ls = new List<Detail>();
            if (Session["cart"] != null)
            {
                ls = Session["cart"] as List<Detail>;
            }
            ls.Add(detail);
            Session["cart"] = ls;
            return RedirectToAction("Index", "Customer");
        }
        public ActionResult Pay()
        {
            var accountCustomer = JsonConvert.DeserializeObject<AccountCustomer>(Request.Cookies["Account"]?.Value);
            List<Detail> ls = Session["cart"] as List<Detail>;
            APIContext apiContext = PayPalConfiguration.GetAPIContext();
            string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/Customer/Pay?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var cancelURL = Request.Url.Scheme + "://" + Request.Url.Authority + "/Home/Index";
                    var createdPayment = CreatePaymentPaypalAPI(apiContext, baseURI + "guid=" + guid, ls,cancelURL);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            //saving the payapalredirect URL to which user will be redirected for payment  
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    TempData[guid] = createdPayment.id;
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, TempData[guid] as string);
                    var cus = accountCustomer.customer;
                    var pay = new Client.Models.DB.Payment()
                    {
                        paymentId = executedPayment.id,
                        totalMoney = decimal.Parse(executedPayment.transactions[0].amount.total),
                        createDate = DateTime.Now,
                        customerId = accountCustomer.customer.id,
                    };
                    var lsDetails = new List<Detail>();
                    foreach (var item in ls)
                    {
                        var detail = new Detail()
                        {
                            customerId = cus.id,
                            staffId = item.staffId,
                            startDate = item.startDate,
                            endDate = item.endDate,
                            amountMoney = item.amountMoney,
                            statusOrder = 0,
                            createDate = DateTime.Now,
                            paymentId = pay.id,
                        };
                        lsDetails.Add(detail);
                    };
                    //If executed payment failed then we will show payment failure message to user  
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        Session.Remove("cart");
                        return JavaScript("Payment Error!");
                    }
                    else
                    {
                        var payment = CreatePayment(pay);
                        foreach (var item in lsDetails)
                        {
                            item.paymentId = payment.id;
                        }
                        CreateDetail(lsDetails);
                        Session["cart"] = new List<Detail>();
                        return RedirectToAction("Index");
                    }
                }
        }
        public ActionResult RemoveFromCart(string index)
        {
            var ls = Session["cart"] as List<Detail>;
            if (ls != null) ls.RemoveAt(Int32.Parse(index) - 1);
            return RedirectToAction("Payment");
        }
        public ActionResult Payment()
        {
            if (Session["cart"] != null)
            {
                List<Detail> ls = Session["cart"] as List<Detail>;
                return View(ls);
            }
            return View();
        }

        public ActionResult History()
        {
            var accountCustomer = JsonConvert.DeserializeObject<AccountCustomer>(Request.Cookies["Account"]?.Value);
            var customer = accountCustomer.customer;
            ViewBag.Count = CountAllDetail(customer) ;
            return View(GetAllDetail(customer, 0));
        }
        [HttpPost]
        public ActionResult History(int id)
        {
            var accountCustomer = JsonConvert.DeserializeObject<AccountCustomer>(Request.Cookies["Account"]?.Value);
            var customer = accountCustomer.customer;
            ViewBag.Count = CountAllDetail(customer);
            return View(GetAllDetail(customer, id));
        }
        public ActionResult Bill()
        {
            var accountCustomer = JsonConvert.DeserializeObject<AccountCustomer>(Request.Cookies["Account"]?.Value);
            var customer = accountCustomer.customer;
            ViewBag.Count = CountAllPayment(customer) ;
            return View(GetAllPayment(customer, 0));
        }
        [HttpPost]
        public ActionResult Bill(int id)
        {
            var accountCustomer = JsonConvert.DeserializeObject<AccountCustomer>(Request.Cookies["Account"]?.Value);
            var customer = accountCustomer.customer;
            ViewBag.Count = CountAllPayment(customer);
            return View(GetAllPayment(customer, id));
        }
    }
}
