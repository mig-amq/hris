using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using Newtonsoft.Json.Linq;

    using WebApplication1.Models;

    public class TrainingController : GlobalController
    {
        // GET: Training
        public ActionResult Index()
        {
            return View();
        }

        // POST: Training/Create
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {

            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            // check if the logged in user is the HR Head
            if (this.CheckLogin(AccountType.DepartmentHead))
            {
                if (((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
                {
                    
                }
            }
            else // else return an error
            {
                json["error"] = true;
                json["message"] = "Uh Oh! You don't have the required priveleges to do that.";
            }

            return Content(json.ToString(), "application/json");
        }
    }
}