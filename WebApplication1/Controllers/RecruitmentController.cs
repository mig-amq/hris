using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Controllers;
using WebApplication1.Models;

namespace Calfurn.Controllers
{
    using Calfurn.Models;
    using System.Globalization;

    public class RecruitmentController : GlobalController
    {
        // GET: RecruitmentController
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ApplicantStatus()
        {
            if (!this.IsLoggedIn())
            {
                return this.RedirectToAction("Index", "Home");
            }

            if (!this.CheckLogin(AccountType.Applicant) && this.GetAccount() != null &&
                ((Employee)this.GetAccount().Profile).Department.Type != DepartmentType.HumanResources)
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        [HttpGet]
        public ContentResult GetJob()
        {
            JObject json = new JObject();

            try
            {
                json["jobs"] = JObject.FromObject(new JobPosting(Int32.Parse(Request.QueryString["id"])));
            }
            catch (Exception e)
            {
                json["jobs"] = "";
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetJobs()
        {
            JObject json = new JObject();
            json["pages"] = 1;
            List<JobPosting> Jobs = new List<JobPosting>();

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
                    constraints += " JobTitle LIKE '%" + query[i] + "%'";

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

            string sql = "SELECT PostingID FROM JobPosting " + constraints;

            foreach (DataRow row in pt.Get(sql))
            {
                JobPosting job = new JobPosting(Int32.Parse(row["PostingID"].ToString()));
                bool hasApplication = false;

                if (this.IsLoggedIn() && this.GetAccount().Type == AccountType.Applicant)
                {
                    foreach (JobApplication application in job.GetApplications(job.PostingID))
                    {
                        if (application.Applicant.ApplicantID == ((Applicant)this.GetAccount().Profile).ApplicantID)
                        {
                            hasApplication = true;
                        }
                    }
                }

                if (!hasApplication)
                    Jobs.Add(job);
            }


            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["jobs"] = JArray.FromObject(Jobs);
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
                                "job-title", "job-desc", "job-req", "year", "month", "day"
                            };

            if (this.HasValues(form, keys))
            {
                if (!this.CheckLogin(AccountType.Applicant) &&
                    ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
                {
                    JobPosting job = new JobPosting();
                    job.JobTitle = form.GetValue("job-title").AttemptedValue;
                    job.JobDescription = form.GetValue("job-desc").AttemptedValue;
                    job.Requirements = form.GetValue("job-req").AttemptedValue;
                    job.DatePosted = DateTime.ParseExact(
                        form.GetValue("year").AttemptedValue + "-"
                                                             + (Int32.Parse(form.GetValue("month").AttemptedValue) + 1).ToString("00") + "-"
                                                             + Int32.Parse(form.GetValue("day").AttemptedValue).ToString("00"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    job.Create();

                    json["message"] = "Requisition Form has been added to Job Postings";
                }
                else
                {
                    json["error"] = true;
                    json["message"] = "You are not authorized to continue";
                }
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult Update(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new string[]
                                {
                                    "job-title", "job-desc", "job-req", "year", "month", "day", "ID"
                                };

            if (this.HasValues(form, keys))
            {
                if (!this.CheckLogin(AccountType.Applicant) &&
                    ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
                {
                    JobPosting job = new JobPosting(Int32.Parse(form.GetValue("ID").AttemptedValue));
                    job.JobTitle = form.GetValue("job-title").AttemptedValue;
                    job.JobDescription = form.GetValue("job-desc").AttemptedValue;
                    job.Requirements = form.GetValue("job-req").AttemptedValue;
                    job.DatePosted = DateTime.ParseExact(
                        form.GetValue("year").AttemptedValue + "-"
                                                             + (Int32.Parse(form.GetValue("month").AttemptedValue) + 1).ToString("00") + "-"
                                                             + Int32.Parse(form.GetValue("day").AttemptedValue).ToString("00"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    job.Update();
                }
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult Delete(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new string[] { "id" };

            if (!this.CheckLogin(AccountType.Applicant) &&
                ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
            {
                if (this.HasValues(form, keys))
                {
                    try
                    {
                        JobPosting post = new JobPosting(Int32.Parse(form.GetValue("id").AttemptedValue));
                        post.Delete();
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
                    json["message"] = "Missing Job Posting ID";
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
        public ContentResult GetApplications()
        {
            JObject json = new JObject();
            json["pages"] = 1;
            List<JobApplication> Applications = new List<JobApplication>();

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
                    constraints += " JobTitle LIKE '%" + query[i] + "%'";

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

                if (Request.QueryString["id"] != null)
                {
                    constraints += " AND Applicant = " + Request.QueryString["id"];
                }
            }
            else if (Request.QueryString["id"] != null)
            {
                constraints = " WHERE Applicant = " + Request.QueryString["id"];
            }

            string sql = "SELECT JobApplicationID FROM JobApplication JA "
                         + "INNER JOIN JobPosting JP ON JA.JobPosting = JP.PostingID " + constraints;

            foreach (DataRow row in pt.Get(sql))
            {
                Applications.Add(new JobApplication(Int32.Parse(row["JobApplicationID"].ToString())));
            }

            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["applications"] = JArray.FromObject(Applications);
            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult Apply(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new string[] { "id" };

            if (this.HasValues(form, keys))
            {
                if (this.IsLoggedIn() && this.GetAccount().Type == AccountType.Applicant)
                {
                    DBHandler db = new DBHandler();

                    try
                    {
                        JobPosting job = new JobPosting(Int32.Parse(form.GetValue("id").AttemptedValue));
                        JobApplication application = new JobApplication();
                        application.JobPosting = job;
                        application.Status = JobApplicationStatus.Undecided;
                        application.Applicant = ((Applicant)this.GetAccount().Profile);

                        application.Create();
                        json["message"] = "You have applied for: " + job.JobTitle
                                                                   + "; please wait for the HR to contact you";
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
                                notifHR.Message = "An applicant has applied for the job: <b>" + job.JobTitle + "</b>";

                                notifHR.Create();
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

        [HttpPost]
        public ContentResult CreateSchedule(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new string[]
                                {
                                    "start", "id"
                                };

            if (this.HasValues(form, keys))
            {
                if (!this.CheckLogin(AccountType.Applicant) && ((Employee)this.GetAccount().Profile).Department.Type
                    == DepartmentType.HumanResources)
                {
                    DateTime Start = DateTime.Parse(form.GetValue("start").AttemptedValue);

                    try
                    {
                        JobApplication a = new JobApplication(Int32.Parse(form.GetValue("id").AttemptedValue), byPrimary: true);
                        if (a.Status != JobApplicationStatus.Schedule)
                        {
                            a.Status = JobApplicationStatus.Schedule;
                            a.Update(false);

                            Schedule sc = new Schedule();
                            sc.JobApplication = a;
                            sc.HR = ((Employee)this.GetAccount().Profile);
                            sc.TimeStart = Start;
                            sc.TimeEnd = Start.AddHours(1);

                            sc.Create();

                            Notification notif = new Notification();
                            notif.Account = new Account().FindByProfile(a.Applicant.Profile.ProfileID, false);
                            notif.TimeStamp = DateTime.Now;
                            notif.Status = NotificationStatus.Unread;
                            notif.Message = "<b>Info: </b> You have been scheduled for an interview with the HR department on "
                                            + Start.ToString("yyyy/MM/dd") + " " + Start.ToString("hh:mm tt") + " - "
                                            + sc.TimeEnd.ToString("hh:mm tt");
                            notif.Create();
                            json["message"] = "Schedule successfully created...";
                        }
                        else
                        {
                            json["error"] = true;
                            json["message"] = "An interview has already been scheduled for this applicant";
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

        [HttpPost]
        public ContentResult UpdateSchedule(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new string[]
                                {
                                    "start", "id"
                                };

            if (this.HasValues(form, keys))
            {
                if (!this.CheckLogin(AccountType.Applicant) && ((Employee)this.GetAccount().Profile).Department.Type
                    == DepartmentType.HumanResources)
                {
                    try
                    {
                        DateTime Start = DateTime.Parse(form.GetValue("start").AttemptedValue);

                        JobApplication a = new JobApplication(Int32.Parse(form.GetValue("id").AttemptedValue), byPrimary: true);
                        Schedule sc = new Schedule().Find(a.Schedule.ScheduleID, byPrimary: true, recursive: false);
                        sc.JobApplication = a;
                        sc.HR = ((Employee)this.GetAccount().Profile);
                        sc.TimeStart = Start;
                        sc.TimeEnd = Start.AddHours(1);

                        sc.Update(false);

                        Notification notif = new Notification();
                        notif.Account = new Account().FindByProfile(a.Applicant.Profile.ProfileID, false);
                        notif.TimeStamp = DateTime.Now;
                        notif.Status = NotificationStatus.Unread;
                        notif.Message = "<b>Info: </b> You have been re-scheduled for an interview with the HR department on "
                                        + Start.ToString("yyyy/MM/dd") + " " + Start.ToString("hh:mm tt") + " - "
                                        + sc.TimeEnd.ToString("hh:mm tt");
                        notif.Create();

                        json["message"] = "Schedule successfully updated...";
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

        [HttpGet]
        public ContentResult GetSchedule()
        {
            JObject json = new JObject();
            json["schedule"] = null;

            if (Request.QueryString["id"] != null)
            {
                try
                {
                    json["schedule"] = JObject.FromObject(new JobApplication(Int32.Parse(Request.QueryString["id"]), true).Schedule);
                }
                catch (Exception e)
                {
                    json["schedule"] = null;
                }
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult UpdateStatus(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new[] { "id", "status" };

            if (this.IsLoggedIn())
            {
                if (this.GetAccount().Type != AccountType.Applicant && ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
                {
                    try
                    {
                        JobApplication ja = new JobApplication(Int32.Parse(form.GetValue("id").AttemptedValue), true);
                        ja.Status = (JobApplicationStatus)Int32.Parse(form.GetValue("status").AttemptedValue);
                        ja.Update(false);

                        if (ja.Status == JobApplicationStatus.Rejected)
                        {
                            json["message"] = (int)JobApplicationStatus.Rejected;
                        } else if (ja.Status == JobApplicationStatus.Accepted)
                        {
                            json["message"] = "Accepted";

                            // create employee data containing applicant profile
                            Employee e = new Employee();
                            e.EmploymentDate = DateTime.Now;
                            e.Position = ja.JobPosting.JobTitle;
                            e.Code = Guid.NewGuid().ToString();
                            e.Profile = ja.Applicant.Profile;

                            DBHandler db = new DBHandler();

                            // update applicant account if it exists
                            using (DataTable dt = db.Execute<DataTable>(
                                CRUD.READ,
                                "SELECT * FROM Account WHERE Profile = " + ja.Applicant.Profile.ProfileID))
                            {
                                if (dt.Rows.Count == 1)
                                {
                                    Account ac = new Account().FindById(
                                        Int32.Parse(dt.Rows[0]["AccountID"].ToString()),
                                        false);
                                    ac.Type = AccountType.Employee;
                                    ac.Profile = e;

                                    ac.Update(recursive: false);

                                    Notification notif = new Notification();
                                    notif.Account = ac;
                                    notif.Message =
                                        "Congratulations! You've been approved for employment, please wait for further information, you will be contacted through phone or email.";
                                    notif.Status = NotificationStatus.Unread;
                                    notif.TimeStamp = DateTime.Now;

                                    notif.Create();
                                }
                            }

                            using (DataTable dt = db.Execute<DataTable>(
                                CRUD.READ,
                                "SELECT * FROM Account WHERE Type = " + ((int)AccountType.DepartmentHead)))
                            {
                                if (dt.Rows.Count >= 1)
                                {
                                    Account ac = new Account().FindById(
                                        Int32.Parse(dt.Rows[0]["AccountID"].ToString()),
                                        false);

                                    Notification notif = new Notification();
                                    notif.Account = ac;
                                    notif.Message = "Info: An Applicant has been approved for employment";
                                    notif.Status = NotificationStatus.Unread;
                                    notif.TimeStamp = DateTime.Now;

                                    notif.Create();
                                }
                            }

                            // delete applicant data
                            foreach (JobApplication jobApp in ja.Applicant.GetApplications(ja.Applicant.ApplicantID))
                            {
                                jobApp.Schedule.Delete();
                                jobApp.Delete();
                            }

                            ja.JobPosting.Delete();
                            ja.Applicant.Delete();

                            e.Create();
                            json["message"] = "Applicant is now a company employee";
                        }
                    }
                    catch (Exception e)
                    {
                        json["error"] = true;
                        json["message"] = e.Message;
                    }
                } else
                {
                    json["error"] = true;
                    json["message"] = "You are not authorized to continue";
                }
            }
            else
            {
                json["error"] = true;
                json["message"] = "You must be logged in to continue";
            }
            return Content(json.ToString(), "application/json");
        }
    }
}