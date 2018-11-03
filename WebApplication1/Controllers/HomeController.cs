using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using WebApplication1.Models;

    public class HomeController : GlobalController
    {
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult Signup()
        {
            return View();
        }

        public ActionResult Dashboard()
        {
            ViewBag.Message = "Dashboard";

            return View();
        }
    }
}