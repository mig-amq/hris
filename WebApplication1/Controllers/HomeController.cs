using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using Newtonsoft.Json.Linq;
    using System.Data;
    using System.Text.RegularExpressions;
    using WebApplication1.Models;

    public class HomeController : GlobalController
    {
        public ActionResult Index()
        {
            if (this.IsLoggedIn())
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        public ActionResult Signup()
        {
            if (this.IsLoggedIn())
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        public ActionResult Dashboard()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            ViewBag.ViewAccount = null;

            if (Request.QueryString["id"] != null && this.GetAccount().Type != AccountType.Applicant && 
                ((Employee) this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
            {
                try
                {
                    ViewBag.ViewAccount = new Account().FindByProfile(Int32.Parse(Request.QueryString["id"]), true);
                }
                catch (Exception e)
                {
                    ViewBag.ViewAccount = null;
                }
            }

            return View();
        }

        public ActionResult Notifications()
        {
            if (!this.IsLoggedIn())
                this.RedirectToAction("Index", "Home");
            return View();
        }

        [HttpGet]
        public ContentResult GetNotifications()
        {
            JObject json = new JObject();
            json["pages"] = 1;
            List<Notification> Notifications = new List<Notification>();
            
            int entries = Int32.Parse(Request.QueryString["entries"]);
            int page = Int32.Parse(Request.QueryString["page"]);
            
            Paginator pt = new Paginator(entries, page);

            string sql = "SELECT * FROM Notification WHERE Account = " + this.GetAccount().AccountID + " ORDER BY Timestamp DESC";

            foreach (DataRow row in pt.Get(sql))
            {
                Notifications.Add(new Notification(Int32.Parse(row["NotificationID"].ToString())));
            }

            foreach (Notification notif in Notifications)
            {
                if (notif.Status == NotificationStatus.Unread)
                {
                    notif.Status = NotificationStatus.Read;
                    notif.Update(false);
                }
            }

            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["notifications"] = JArray.FromObject(Notifications);
            return Content(json.ToString(), "application/json");
        }
    }
}