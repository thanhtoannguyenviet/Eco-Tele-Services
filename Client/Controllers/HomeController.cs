using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;
using Client.Models;
using Client.Service;
using static Client.Service.LoginService;
namespace Client.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        [HttpPost]
        public ActionResult Login()
        {
            Account account = new Account();
            account.userName= Request["UserAccount"];
            account.pass_word = Request["PasswordAccount"];
            var person = LoginService.CheckLogin(account);
            if (person != null)
            {

                if (person.role_ == 1)
                {
                    var customer = LoginCustomer(person);
                    customer.account = person;
                    new HttpCookie("Account",new JavaScriptSerializer().Serialize(customer));
                    ViewBag.Name = customer.customer.headName;
                    var username = Common.ROLE.GetValue(person.role_);
                    FormsAuthentication.SetAuthCookie(Common.ROLE.GetValue(person.role_), true);
                    return Redirect("/Customer/");
                }
                else if (person.role_ > 1)
                {
                    var staff = LoginStaff(person);
                    staff.account = person;
                    Response.Cookies.Add( new HttpCookie("Account", new JavaScriptSerializer().Serialize(staff)));
                    ViewBag.Name = staff.staff.staffName;
                    var username = Common.ROLE.GetValue(person.role_);
                    FormsAuthentication.SetAuthCookie(Common.ROLE.GetValue(person.role_), true);
                    return Redirect("/Admin/");
                }
            }
            else
            {
                ViewBag.Error = "1";
                ModelState.AddModelError("", "Invalid user and password");
            }
            return View("Index");
        } 
        public ActionResult LogOut()
        {
            if (Request.Cookies["Account"] != null)
            {
                HttpCookie c = new HttpCookie("Account");
                c.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Add(c);
            }
            Request.Cookies.Clear();
            return RedirectToAction("Index");
        }
    }
}