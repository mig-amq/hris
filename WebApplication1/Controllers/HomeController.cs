using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using WebApplication1.Models;

    public class HomeController : Controller
    {
        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Sad";

            return View();
        }

        public ActionResult Dashboard()
        {
            ViewBag.Message = "Dashboard";

            return View();
        }
    }
}