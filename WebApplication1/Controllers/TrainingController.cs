using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using System.Data;
    using System.Text.RegularExpressions;
    using System.Web.Http;

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
        [System.Web.Mvc.HttpPost]
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
                    TrainingHistory tr = new TrainingHistory();
                    tr.Description = form.GetValue("desc").AttemptedValue;
                    tr.Facilitator = form.GetValue("faci").AttemptedValue;
                    tr.Title = form.GetValue("title").AttemptedValue;
                    tr.Location = form.GetValue("loc").AttemptedValue;

                    string start = form.GetValue("start-month").AttemptedValue + "/" + form.GetValue("start-day").AttemptedValue
                                   + "/" + form.GetValue("start-year").AttemptedValue;
                    string end = form.GetValue("end-month").AttemptedValue + "/" + form.GetValue("end-day").AttemptedValue
                                   + "/" + form.GetValue("end-year").AttemptedValue;
                    tr.StartDate = DateTime.Parse(start);
                    tr.EndDate = DateTime.Parse(end);
                    tr.Create();
                }
            }
            else // else return an error
            {
                json["error"] = true;
                json["message"] = "Uh Oh! You don't have the required priveleges to do that.";
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult Delete(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            TrainingHistory tr = new TrainingHistory(Int32.Parse(form.GetValue("ID").AttemptedValue)).Delete();

            return Content(json.ToString(), "application/json");
        }
        [System.Web.Mvc.HttpGet]
        public ContentResult GetTrainings()
        {
            JObject json = new JObject();
            json["pages"] = 1;
            List<TrainingHistory> Training = new List<TrainingHistory>();

            string[] query = null;
            int entries = Int32.Parse(Request.QueryString["entries"]);
            int page = Int32.Parse(Request.QueryString["page"]);

            if (Request.QueryString["query"] != null)
            {
                query = Regex.Replace(Request.QueryString["query"].Trim(), @"\s+", " ").Split(' ');
            }

            string constraints = "";

            if (query != null)
            {
                for (int i = 0; i < query.Length; i++)
                {
                    constraints += " Title LIKE '%" + query[i] + "%'";

                    if (i < query.Length - 1)
                    {
                        constraints += " OR ";
                    }
                }
            }

            Paginator pt = new Paginator(entries, page);

            if (constraints.Length > 1)
            {
                constraints = " WHERE " + constraints;
            }

            string sql = "SELECT TrainingHistoryID FROM TrainingHistory " + constraints;

            foreach (DataRow row in pt.Get(sql))
            {
                Training.Add(new TrainingHistory(Int32.Parse(row.ItemArray[0].ToString())));
            }

            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["trainings"] = JArray.FromObject(Training);
            return Content(json.ToString(), "application/json");
        }
    }
}