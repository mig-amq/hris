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

    public class LeaveController : GlobalController
    {
        // GET: Leave
        public ActionResult Index()
        {
            if (this.CheckLogin(AccountType.Applicant))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        public ActionResult Approval()
        {

            if (!this.CheckLogin(AccountType.Applicant) && ((Employee)this.GetAccount().Profile).Department.Type
                != DepartmentType.HumanResources)
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        [HttpGet]
        public ContentResult Applications()
        {
            JObject json = new JObject();
            json["pages"] = 1;
            List<Leave> Leave = new List<Leave>();

            int entries = Int32.Parse(Request.QueryString["entries"]);
            int page = Int32.Parse(Request.QueryString["page"]);

            Paginator pt = new Paginator(entries, page);

            string[] query = null;
            string constraints = "";

            if (Request.QueryString["query"] != null)
            {
                query = Regex.Replace(Request.QueryString["query"].Trim(), @"\s+", " ").Split(' ');
            }

            if (query != null)
            {
                for (int i = 0; i < query.Length; i++)
                {
                    if (query[i].Length > 0)
                    {
                        constraints += " FullName LIKE '%" + query[i] + "%'";

                        if (i < query.Length - 1)
                        {
                            constraints += " OR ";
                        }
                    }
                }
            }

            if (constraints.Length > 0)
            {
                constraints = " WHERE " + constraints + " AND ";
            }
            else
            {
                constraints = " WHERE ";
            }

            if (Request.QueryString["personal"] != null && !Boolean.Parse(Request.QueryString["personal"])
                && ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
            {
                constraints += " Employee <> " + ((Employee)this.GetAccount().Profile).EmployeeID;
            }
            else
            {
                constraints += " Employee = " + ((Employee)this.GetAccount().Profile).EmployeeID;
            }

            if (this.CheckLogin(AccountType.Employee) || this.CheckLogin(AccountType.DepartmentHead))
            {
                string sql = "SELECT * FROM ("
                             + "SELECT CONCAT(P.FirstName, ' ', P.MiddleName, ' ', P.LastName) AS FullName, L.LeaveID, L.Employee FROM"
                             + " Leave L INNER JOIN Employee E ON L.Employee = E.EmployeeID "
                             + "INNER JOIN Profile P ON P.ProfileID = E.Profile) T2 " + constraints;

                foreach (DataRow row in pt.Get(sql))
                {
                    Leave.Add(new Leave(Int32.Parse(row["LeaveID"].ToString())));
                }

                Leave = Leave.OrderByDescending(o => o.StartDate).ToList();
            }

            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["leaves"] = JArray.FromObject(Leave);
            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult Create(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new string[]
                            {
                                "start-month", "start-day", "start-year", "end-month", "end-day", "end-year", "reason"
                            };

            if (!this.CheckLogin(AccountType.Applicant))
            {
                if (this.HasValues(form, keys))
                {
                    try
                    {
                        Leave l = new Leave();
                        string start = (Int32.Parse(form.GetValue("start-month").AttemptedValue) + 1).ToString("00") + "/"
                                        + (Int32.Parse(form.GetValue("start-day").AttemptedValue) + 1).ToString("00") + "/"
                                        + form.GetValue("start-year").AttemptedValue;
                        string end = (Int32.Parse(form.GetValue("end-month").AttemptedValue) + 1).ToString("00") + "/"
                                       + (Int32.Parse(form.GetValue("end-day").AttemptedValue) + 1).ToString("00") + "/"
                                        + form.GetValue("end-year").AttemptedValue;

                        l.Employee = (Employee)this.GetAccount().Profile;
                        l.StartDate = DateTime.ParseExact(start, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        l.EndDate = DateTime.ParseExact(end, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                        l.Status = LeaveStatus.Pending;
                        l.Reason = form.GetValue("reason").AttemptedValue;

                        l.Create();

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

                                if (this.GetAccount().Profile.Profile.ProfileID != notifHR.Account.AccountID)
                                {
                                    notifHR.TimeStamp = DateTime.Now;
                                    notifHR.Status = NotificationStatus.Unread;

                                    notifHR.Message = l.Employee.Profile.FirstName + " " + l.Employee.Profile.MiddleName
                                                      + " " + l.Employee.Profile.LastName + " applied for a sick leave on "
                                                      + start + " - " + end;
                                    notifHR.Create();
                                }
                            }
                        }
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
                    json["message"] = "Form is incomplete";
                }
            }
            else
            {
                json["error"] = true;
                json["message"] = "You are not authorized to continue";
            }

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
                        Leave Leave = new Leave(Int32.Parse(form.GetValue("id").AttemptedValue));
                        Leave.Status = (LeaveStatus)Int32.Parse(form.GetValue("V").AttemptedValue);
                        Leave.Update(recursive:false);

                        Notification notif = new Notification();
                        notif.Account = new Account().FindByProfile(Leave.Employee.Profile.ProfileID);
                        notif.Message = "<b>Info:</b> Your leave request #" + Leave.LeaveID + " has been updated to: <i>" 
                                        + (Leave.Status == LeaveStatus.Approved ? 
                                               " Approved " : Leave.Status == LeaveStatus.Denied ? " Denied " : " Pending ") + "</i>";
                        notif.TimeStamp = DateTime.Now;
                        notif.Status = NotificationStatus.Unread;
                        notif.Create();
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
                    json["message"] = "You are not authorized to continue";
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