using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Client.Common;
using Client.Models.DB;
using Client.Models.DTO;
using Newtonsoft.Json;
using static Client.Service.StaffService;
using static Client.Service.ImageService;
using static Client.Service.VideoService;
using static Client.Common.CODESTATUS;
using Client.Service;
using System.IO;

namespace Client.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public async Task<ActionResult> Index()
        {
            var info = Request.Cookies["Account"] ;
            if (info != null) { 
                var accountStaff= JsonConvert.DeserializeObject<AccountStaff>(info.Value);
                var lsDetail =  await GetDetailPayments(accountStaff.account.id);
                ViewBag.Staff = accountStaff;
                ViewBag.Model = lsDetail.Where(a=>a.details.statusOrder==STATUS_DEFAULT);
                ViewBag.TotalMoney= lsDetail.Where(a=>a.details.statusOrder==STATUS_ACCEPT).Sum(a=>a.details.amountMoney);
                return View();
            }
            return Redirect("/Home/");
        }
        public async Task<ActionResult> OrderAccept()
        {
            var info = Request.Cookies["Account"];
            if (info != null)
            {
            var accountStaff = JsonConvert.DeserializeObject<AccountStaff>(info.Value);
            var lsDetail = await GetDetailPayments(accountStaff.account.id);
            ViewBag.Staff = accountStaff;
            ViewBag.Model = lsDetail.Where(a => a.details.statusOrder == STATUS_DEFAULT);
            }
            return PartialView("_OrderAccept");
             
        }
        [HttpGet]
        public async Task<ActionResult> AcceptReplyCustomer(int detail)
        {
            var accountSaff = JsonConvert.DeserializeObject<AccountStaff>(Request.Cookies["Account"]?.Value);
            var update = await GetDetailPayments(accountSaff.id);
            var finder =update.Find(s => s.details.id == detail);
            finder.details.statusOrder = 1;
            var updateDetail = UpdateDetail(finder.details);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> DenyReplyCustomer(int detail)
        {
            var accountSaff = JsonConvert.DeserializeObject<AccountStaff>(Request.Cookies["Account"]?.Value);
            var update = accountSaff.details.Find(s => s.id == detail);
            update.statusOrder = -1;
            var updateDetail = UpdateDetail(update);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> SettingView()
        {
            var accountSaff = JsonConvert.DeserializeObject<AccountStaff>(Request.Cookies["Account"]?.Value);
            return View(accountSaff);
        }
        [HttpPost]
        public async Task<ActionResult> Img()
        {
            var accountSaff = JsonConvert.DeserializeObject<AccountStaff>(Request.Cookies["Account"]?.Value);
            accountSaff.imgs[0].path_= Request["ImageFile"];
            UpdateImg(accountSaff.imgs[0]);
            return View("SettingView");
        }
        [HttpPost]
        public async Task<ActionResult> ChangePassword(string password)
        {
            var  accountStaff = JsonConvert.DeserializeObject<AccountStaff>(Request.Cookies["Account"]?.Value);
            if (accountStaff.account.pass_word == Request["Oldpassword"] && password == Request["ConfPassword"])
            {
                accountStaff.account.pass_word = password;
                var newaccountStaff = UpdateStaff(accountStaff);
                if (newaccountStaff != null)
                {
                    return View("Index");
                }
                new HttpCookie("Account", new JavaScriptSerializer().Serialize(newaccountStaff));
            }
            return RedirectToAction("SettingView");
        }
        [HttpPost]
        public async Task<ActionResult> SettingView(Staff staff)
        {
            var accountStaff = JsonConvert.DeserializeObject<AccountStaff>(Request.Cookies["Account"]?.Value);
            staff.mistakeCount =accountStaff.staff.mistakeCount;
            staff.staffBirtday =accountStaff.staff.staffBirtday;
            accountStaff.staff=staff;
            var newaccountStaff = UpdateStaff(accountStaff);
            new HttpCookie("Account", new JavaScriptSerializer().Serialize(newaccountStaff));
            if (newaccountStaff != null)
            {
                return View("Index");
            }
            return View("SettingView");
        }
        public async Task<ActionResult> Service()
        {
            var accountStaff = JsonConvert.DeserializeObject<AccountStaff>(Request.Cookies["Account"]?.Value);
            List<AccountStaff> lsAccount = new List<AccountStaff>(FindWithRole(accountStaff.account.role_));
            foreach (var item in lsAccount)
            {
                item.services = GetServiceOfStaff(item.account.id);
            }
            Session["Acc"] = lsAccount;
            return View(lsAccount);
        }
        public async Task<ActionResult> SettingService(int id)
        {
            var lsAccount = Session["Acc"] as List<AccountStaff>;
            var accountStaff=lsAccount.Where(a => a.staff.id== id).FirstOrDefault();
            accountStaff.services=GetServiceOfStaff(id);
            Session["AccountStaff"] = accountStaff;
            return View(accountStaff);
        }
        [HttpPost]
        public ActionResult SettingServiceDetail(FormCollection form)
        {
            string[] strArray= new []{ form["inbound"] , form["outbound"], form["telemarketing"] };
            var accountStaff = Session["AccountStaff"] as AccountStaff;
            foreach (var item in accountStaff.services)
            {
                 UpdateService(item);   
            }
            accountStaff.services=new List<Service_>();
            foreach (var item in strArray)
            {
                if (item != null) { 
                var service = new Service_();
                service.serviceName=item;
                service.staffId=accountStaff.id;
                accountStaff.services.Add(service);
                }
            }
            
            string result = RegisterService(accountStaff);
            if (result != null)
            {
                Session.Remove("AccountStaff");
            }
            return View("Service");
        }
        public ActionResult Account()
        {
            var accountStaff = JsonConvert.DeserializeObject<AccountStaff>(Request.Cookies["Account"]?.Value);
            var accountRole = FindWithRole(accountStaff.account.role_);
            Session["AccountRole"] = accountRole;
            return View(accountRole);
        }
        public ActionResult NewAccount()
        {
            return View(new AccountStaff());
        }
        [HttpPost]
        public ActionResult NewAccount(AccountStaff accountStaff)
        {
            accountStaff.staff.staffBirtday = DateTime.Parse(Request["birthday"]);
            accountStaff.staff.department = Request["idCat"];
            accountStaff.staff.mistakeCount = 0;
            accountStaff.account.pass_word = accountStaff.account.pass_word;
            accountStaff.staff.status_ = 1;
            accountStaff.account.role_ = ROLE.GetKey(accountStaff.staff.department);
            accountStaff.imgs.Add(new Img(accountStaff.staff.id));
            accountStaff = RegisterStaff(accountStaff);
            return Redirect("/Admin/");
        }
        [HttpPost]
        public PartialViewResult Search(string txtSearch)
        {
            var lsAccount = Session["AccountRole"] as List<AccountStaff>;
            if(!String.IsNullOrEmpty(txtSearch))
                lsAccount =lsAccount.Where(s=>s.staff.staffName.ToUpper().Contains(txtSearch.ToUpper())).ToList();
            return PartialView("_AccountSearch",lsAccount);
        }

        [HttpPost]
        public ActionResult Video(string VideoFile)
        {
            var youtubeVideo = VideoFile;
            var accountSaff = JsonConvert.DeserializeObject<AccountStaff>(Request.Cookies["Account"]?.Value);
            accountSaff.vids[0].path_ = youtubeVideo;
            UpdateVid(accountSaff.vids[0]);
            Response.Cookies.Add(new HttpCookie("Account", new JavaScriptSerializer().Serialize(accountSaff)));
            return RedirectToAction("SettingView");
           
        }

        [HttpPost]
        public async Task<ActionResult> UpLoadYoutubeVideoAsync(VideoItem videoItem)
        {
           
            var newUrl = "https://www.youtube.com/embed/{0}";
            string videoId = String.Empty ;
            HttpPostedFileBase videofile = Request.Files[0];
           YoutubeService tk = new YoutubeService();
            try
            {
               videoId = await tk.UploadVideo(videofile,videoItem);
            } catch (Exception ex)
            {
                var a = ex;
            }
            return RedirectToAction("Youtube");
        }
        
        public async Task<ActionResult> Youtube()
        {
            
            YoutubeService tk = new YoutubeService();
            var videoItems = await tk.GetListVids();
            return View(videoItems);
        }

        [HttpGet]
        public ActionResult YoutubeView()
        {
            return View();
        }
        [HttpGet]
        public ActionResult YoutubeCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult YoutubeUpload(HttpPostedFileBase videofile, VideoItem video)
        {
            YoutubeService tk = new YoutubeService();
            return Json(tk.UploadVideo(videofile,video), JsonRequestBehavior.AllowGet);
        }
    }
}