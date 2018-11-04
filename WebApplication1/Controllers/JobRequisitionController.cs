using System;
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
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (!this.CheckLogin(AccountType.DepartmentHead))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        public ActionResult List()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (!this.CheckLogin(AccountType.DepartmentHead) || 
                !(!this.CheckLogin(AccountType.Applicant) && ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        [HttpPost]
        public ActionResult Requisition(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = true;

            string[] keys = new string[]
                            {
                                "month", "day", "year", "description", "position", "reason", "skills", "qualification",
                                "experience", "supervision", "requisition-type", "description"
                            };

            if (this.HasValues(form, keys))
            {
                if (this.CheckLogin(AccountType.DepartmentHead))
                {
                    try
                    {
                        string sched = (Int32.Parse(form.GetValue("month").AttemptedValue) + 1).ToString("00") + "/"
                                        + (Int32.Parse(form.GetValue("day").AttemptedValue) + 1)
                                           .ToString("00") + "/"+ form.GetValue("year").AttemptedValue;

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
                    

                        Notification notifS = new Notification();
                        notifS.Account = new Account().FindByProfile(rf.UnderSupervision.Profile.ProfileID);
                        notifS.Message = "Info: You have been assigned to supervise an incoming manpower requisition.";
                        notifS.TimeStamp = DateTime.Now;
                        notifS.Status = NotificationStatus.Unread;

                        notifS.Create();
                        DBHandler db = new DBHandler();
                        using (DataTable dt = db.Execute<DataTable>(
                            CRUD.READ,
                            "SELECT E.Profile FROM Employee E INNER JOIN Department D ON E.Department = D.DepartmentID WHERE D.Type = "
                            + ((int)DepartmentType.HumanResources)))
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                Notification notifHR = new Notification();
                                notifHR.Account = new Account().FindByProfile(Int32.Parse(row["Profile"].ToString()));
                                notifHR.TimeStamp = DateTime.Now;
                                notifHR.Status = NotificationStatus.Unread;
                                notifHR.Message = "A new requisition form was issued by the "
                                                  + (rf.UnderSupervision.Department.Type) + " department for the position: <b>" + rf.Position + "</b>";

                                notifHR.Create();
                            }
                        }

                        json["message"] = "Successfully completed request, please wait for further notification...";
                    }
                    catch (Exception e)
                    {
                        json["error"] = true;
                        json["message"] = e.Message;
                    }
                }
                else
                {
                    json["error"] = true;
                    json["message"] = "You are not authorized to continue...";
                }
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetSupervisors()
        {
            JObject json = new JObject();
            List<Employee> Employees = new List<Employee>();
            DBHandler db = new DBHandler();

            if (!this.CheckLogin(AccountType.Applicant))
            {
                string sql = "SELECT EmployeeID FROM Employee WHERE Department = "
                             + ((Employee)this.GetAccount().Profile).Department.DepartmentID;

                using (DataTable dt = db.Execute<DataTable>(CRUD.READ, sql))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        try
                        {
                            Employee emp = (Employee)new Employee().FindProfile(
                                Int32.Parse(row["EmployeeID"].ToString()),
                                byPrimary: true);
                            Employees.Add(emp);
                        }
                        catch (Exception e)
                        {
                        }
                    }
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
                if (this.CheckLogin(AccountType.DepartmentHead) || this.CheckLogin(AccountType.Employee) && 
                    ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
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
                        json["message"] = "Invalid data sent.";
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
                json["message"] = "Form is incomplete";
            }

            return Content(json.ToString(), "application/json");
        }
    }
}