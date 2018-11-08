using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using System.Data;
    using System.Diagnostics;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Linq;

    using WebApplication1.Models;

    public class AppraisalController : GlobalController
    {
        // GET: Appraisal
        public ActionResult Index()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (this.CheckLogin(AccountType.Employee) || this.CheckLogin(AccountType.Applicant))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        // GET: Appraisal/Undiscussed
        public ActionResult Undiscussed()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (this.CheckLogin(AccountType.Employee) || this.CheckLogin(AccountType.Applicant))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        // GET: Appraisal/Personal
        public ActionResult Personal()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (this.CheckLogin(AccountType.Applicant))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        [HttpPost]
        public ContentResult Create(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new string[]
                                {
                                    "values",
                                };

            if (this.HasValues(form, keys))
            {
                if (!this.CheckLogin(AccountType.Applicant) && !this.CheckLogin(AccountType.Employee))
                {
                    JObject values = JObject.Parse(form.GetValue("values").AttemptedValue);
                    Appraisal ap = new Appraisal();

                    if (this.CheckLogin(AccountType.CEO) || this.CheckLogin(AccountType.VP))
                    {
                        ap.Type = AppraisalType.Supervisory;
                    }
                    else
                    {
                        ap.Type = AppraisalType.NonSupervisory;
                    }

                    ap.TechComp = 0;
                    foreach (JToken token in values.GetValue("competence"))
                    {
                        ap.TechComp += Double.Parse(token.ToString());
                    }

                    ap.InterSkills = 0;
                    foreach (JToken token in values.GetValue("interpersonal"))
                    {
                        ap.InterSkills += Double.Parse(token.ToString());
                    }

                    ap.CommComp = 0;
                    foreach (JToken token in values.GetValue("commitment"))
                    {
                        ap.CommComp += Double.Parse(token.ToString());
                    }

                    ap.TechComp = ap.TechComp / (values.GetValue("competence").Count() * 5) * 100;
                    ap.InterSkills = ap.InterSkills / (values.GetValue("interpersonal").Count() * 5) * 100;
                    ap.CommComp = ap.CommComp / (values.GetValue("commitment").Count() * 5) * 100;
                    ap.Total = (int)((ap.TechComp * 0.45) + (ap.InterSkills * 0.35) + (ap.CommComp * 0.20));
                    ap.Evaluator = (Employee)this.GetAccount().Profile;
                    ap.DatePrepared = DateTime.Now;
                    ap.DateNoted = DateTime.Now;

                    try
                    {
                        ap.DiscussedWith = new Employee(Int32.Parse(form.GetValue("employee").AttemptedValue), byPrimary: true);
                        ap.NotedBy = new Employee(Int32.Parse(form.GetValue("noted-by").AttemptedValue), byPrimary: true);
                        ap.Comments = form.GetValue("comment").AttemptedValue;
                        ap.Status = AppraisalStatus.Pending;
                        ap.CoveredPeriod = DateTime.ParseExact(
                            form.GetValue("year").AttemptedValue + "-"
                             + (Int32.Parse(form.GetValue("month").AttemptedValue) + 1).ToString("00") + "-"
                             + "01", "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        ap.Create();

                        Notification notif = new Notification();
                        notif.Account = new Account().FindByProfile(ap.DiscussedWith.Profile.ProfileID);
                        notif.Message = "You have been evaluated by your superior, please check the results in My Evaluations and wait for further information...";
                        notif.Status = NotificationStatus.Unread;
                        notif.TimeStamp = DateTime.Now;
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
                    json["message"] = "You are not allowed to evaluate anyone";
                }
            }
            else
            {
                json["error"] = true;
                json["message"] = "Form is incomplete";
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetEmployees()
        {
            JObject json = new JObject();
            List<Employee> Employees = new List<Employee>();

            DBHandler db = new DBHandler();

            string sql = "SELECT E.EmployeeID"
                         + " FROM Account A INNER JOIN Employee E ON A.Profile = E.Profile "
                         + "INNER JOIN Department D ON E.Department = D.DepartmentID ";

            string constraint = "";

            if (this.CheckLogin(AccountType.CEO))
            {
                constraint = " WHERE A.Type = " + ((int)AccountType.VP);
            } else if (this.CheckLogin(AccountType.VP))
            {
                constraint = " WHERE A.Type = " + ((int)AccountType.DepartmentHead) + " AND D.Branch = " + ((Employee)this.GetAccount().Profile).Department.Branch.BranchID;
            } else
            {
                constraint = " WHERE A.Type = " + ((int)AccountType.Employee) + " AND E.Department = " + ((Employee)this.GetAccount().Profile).Department.DepartmentID;
            }

            if (!String.IsNullOrEmpty(Request.QueryString["all"]) && Request.QueryString["all"].ToLower() == "true")
            {
                constraint = " WHERE D.Branch = " + ((Employee)this.GetAccount().Profile).Department.Branch.BranchID;
            }

            using (DataTable dt = db.Execute<DataTable>(CRUD.READ, sql + constraint))
            {
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        Employees.Add(new Employee(Int32.Parse(row["EmployeeID"].ToString()), byPrimary:true));
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                }
            }

            json["employees"] = JArray.FromObject(Employees);
            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetUndiscussed()
        {
            JObject json = new JObject();
            json["pages"] = 1;
            List<Appraisal> Appraisals = new List<Appraisal>();

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
                    constraints += " T2.FullName LIKE '%" + query[i] + "%'";

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

            string sql = "SELECT * FROM ("
                         + "SELECT CONCAT(P2.FirstName, ' ', P2.MiddleName, ' ', P2.LastName) AS FullName, A.AppraisalID, A.CoveredPeriod"
                         + " FROM Appraisal A INNER JOIN Employee E ON A.Evaluator = E.EmployeeID"
                         + " INNER JOIN Employee E2 ON A.DiscussedWith = E2.EmployeeID"
                         + " INNER JOIN Profile P2 ON E2.Profile = P2.ProfileID WHERE A.Status = 1 AND A.Evaluator = " + ((Employee) this.GetAccount().Profile).EmployeeID
                         + ") T2 " + constraints + " ORDER BY CoveredPeriod DESC";

            foreach (DataRow row in pt.Get(sql))
            {
                Appraisals.Add(new Appraisal(Int32.Parse(row["AppraisalID"].ToString())));
            }

            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["appraisals"] = JArray.FromObject(Appraisals);
            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult Discuss(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            if (!this.CheckLogin(AccountType.Applicant) && !this.CheckLogin(AccountType.Employee))
            {
                if (form.GetValue("id") != null)
                {
                    try
                    {
                        Appraisal ap = new Appraisal(Int32.Parse(form.GetValue("id").AttemptedValue), recursive: true, byDiscussed: false);
                        ap.DateDiscussed = DateTime.Now;
                        ap.Status = AppraisalStatus.Finished;

                        ap.Update(recursive: false);
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
                    json["message"] = "Invalid data was sent";
                }
            }
            else
            {
                json["error"] = true;
                json["message"] = "You are not authorized to continue";
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetPersonal()
        {
            JObject json = new JObject();

            try
            {

                json["appraisals"] = JArray.FromObject(
                    new Appraisal().FindAll(((Employee)this.GetAccount().Profile).EmployeeID, true));
            }
            catch
            {
                json["appraisals"] = default(JArray);
            }

            return Content(json.ToString(), "application/json");
        }
    }
}