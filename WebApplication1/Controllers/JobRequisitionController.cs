﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using Newtonsoft.Json.Linq;
    using System.Data;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using WebApplication1.Models;

    public class JobRequisitionController : GlobalController
    {
        // GET: JobRequisition
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Requisition(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = true;

            if (this.CheckLogin(AccountType.DepartmentHead))
            {
                string sched = (Int32.Parse(form.GetValue("month").AttemptedValue) + 1).ToString("00") + "/" + (Int32.Parse(form.GetValue("day").AttemptedValue) + 1).ToString("00")
                               + "/" + form.GetValue("year").AttemptedValue;

                RequisitionForm rf = new RequisitionForm();
                rf.Date = DateTime.Now;
                rf.Department = ((Employee)this.GetAccount().Profile).Department;
                rf.RequestedBy = ((Employee)this.GetAccount().Profile);

                rf.Description = form.GetValue("description").AttemptedValue;
                rf.Position = form.GetValue("position").AttemptedValue;
                rf.ReasonforVacancy = form.GetValue("reason").AttemptedValue;
                rf.SkillsRequired = form.GetValue("skills").AttemptedValue;
                rf.Qualification = form.GetValue("qualification").AttemptedValue;
                rf.ExperienceRequired = form.GetValue("experience").AttemptedValue;
                rf.UnderSupervision = new Employee(Int32.Parse(form.GetValue("supervision").AttemptedValue));
                rf.Type = form.GetValue("requisition-type").AttemptedValue;
                rf.ExpectedJoiningDate = DateTime.ParseExact(sched, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                rf.Description = form.GetValue("description").AttemptedValue;
                rf.Status = RequisitionStatus.Pending;

                rf.Create();
                json["message"] = "Successfully completed request, please wait for further notification...";
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetSupervisors()
        {
            JObject json = new JObject();
            List<Employee> Employees = new List<Employee>();
            DBHandler db = new DBHandler();

            string sql = "SELECT EmployeeID FROM Employee WHERE Department = "
                         + ((Employee)this.GetAccount().Profile).Department.DepartmentID;

            using (DataTable dt = db.Execute<DataTable>(CRUD.READ, sql))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Employees.Add((Employee) new Employee().FindProfile(Int32.Parse(row["EmployeeID"].ToString()), byPrimary:true));
                }
            }

            json["supervisors"] = JArray.FromObject(Employees);
            return Content(json.ToString(), "application/json");
        }
        
        [HttpGet]
        public ContentResult GetRequisitions()
        {
            JObject json = new JObject();
            json["pages"] = 1;
            List<RequisitionForm> RequisitionForm = new List<RequisitionForm>();

            int entries = Int32.Parse(Request.QueryString["entries"]);
            int page = Int32.Parse(Request.QueryString["page"]);

            Paginator pt = new Paginator(entries, page);
            
            string constraints = "";

            if (((Employee)this.GetAccount().Profile).Department.Type != DepartmentType.HumanResources)
            {
                constraints += " WHERE Department = " + ((Employee)this.GetAccount().Profile).Department.DepartmentID;
            }

            if (((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources || 
                this.CheckLogin(AccountType.DepartmentHead))
            {
                string sql = "SELECT RequisitionID FROM RequisitionForm " + constraints;

                foreach (DataRow row in pt.Get(sql))
                {
                    RequisitionForm.Add(new RequisitionForm(Int32.Parse(row["RequisitionID"].ToString())));
                }

                RequisitionForm = RequisitionForm.OrderByDescending(o => o.Status).ToList();
            }

            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["requisitions"] = JArray.FromObject(RequisitionForm);
            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult Update(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            if (form.GetValue("id") != null && form.GetValue("v") != null)
            {
                if (((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
                {
                    try
                    {
                        RequisitionForm RequisitionForm =
                            new RequisitionForm(Int32.Parse(form.GetValue("id").AttemptedValue));
                        RequisitionForm.Status = (RequisitionStatus)Int32.Parse(form.GetValue("v").AttemptedValue);
                        RequisitionForm.Update(false);
                    }
                    catch (Exception e)
                    {
                        json["error"] = true;
                        json["message"] = "Uh oh! Invalid data sent.";
                    }
                }
                else
                {
                    json["error"] = true;
                    json["message"] = "Oops! Looks like you're not an HR!";
                }
            }
            else
            {
                json["error"] = true;
                json["message"] = "Oops! Some data failed to send to the server";
            }

            return Content(json.ToString(), "application/json");
        }
    }
}