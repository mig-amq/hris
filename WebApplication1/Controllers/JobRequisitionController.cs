using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using Newtonsoft.Json.Linq;

    using WebApplication1.Models;

    public class JobRequisitionController : GlobalController
    {
        // GET: JobRequisition
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Request(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = true;

            if (this.CheckLogin(AccountType.DepartmentHead))
            {
                RequisitionForm rf = new RequisitionForm();
                rf.Date = DateTime.Now;
                rf.Department = ((Employee)this.GetAccount().Profile).Department;
                rf.RequestedBy = ((Employee)this.GetAccount().Profile);

                rf.Description = form.GetValue("description").ToString();
                rf.Position = form.GetValue("position").ToString();
                rf.ReasonforVacancy = form.GetValue("reason").ToString();
                rf.SkillsRequired = form.GetValue("skills").ToString();
                rf.Qualification = form.GetValue("qualification").ToString();
                rf.ExperienceRequired = form.GetValue("experience").ToString();
                rf.UnderSupervision = new Employee(Int32.Parse(form.GetValue("supervision").ToString()));
                rf.Type = form.GetValue("type").ToString();
                rf.ExpectedJoiningDate = DateTime.Parse(form.GetValue("expected").ToString());
                rf.Description = form.GetValue("description").ToString();

                rf.Create();
                json["message"] = "Successfully completed request, please wait for further notification...";
            }

            return Content(json.ToString(), "application/json");
        }
    }
}