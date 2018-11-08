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
    using System.Web.Http;

    using Newtonsoft.Json.Linq;

    using WebApplication1.Models;

    public class TrainingController : GlobalController
    {
        // GET: Training
        public ActionResult Index()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (this.CheckLogin(AccountType.VP) || this.CheckLogin(AccountType.CEO) || this.CheckLogin(AccountType.Applicant))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        [HttpGet]
        public ActionResult Assigned()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (this.CheckLogin(AccountType.VP) || this.CheckLogin(AccountType.CEO) || this.CheckLogin(AccountType.Applicant))
                return this.RedirectToAction("Dashboard", "Home");

            return this.View();
        }

        // POST: Training/Create
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new string[]
                                {
                                    "desc", "faci", "title", "loc", "start-month", "start-day", "start-year",
                                    "end-month", "end-day", "end-year"
                                };

            if (this.HasValues(form, keys))
            {
                // check if the logged in user is the HR Head
                if (!this.CheckLogin(AccountType.Applicant))
                {
                    if (((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
                    {
                        try
                        {
                            Training tr = new Training();
                            tr.Description = form.GetValue("desc").AttemptedValue;
                            tr.Facilitator = form.GetValue("faci").AttemptedValue;
                            tr.Title = form.GetValue("title").AttemptedValue;
                            tr.Location = form.GetValue("loc").AttemptedValue;

                            string start = (Int32.Parse(form.GetValue("start-month").AttemptedValue) + 1).ToString("00")
                                           + "/"
                                           + (Int32.Parse(form.GetValue("start-day").AttemptedValue) + 1).ToString("00")
                                           + "/" + form.GetValue("start-year").AttemptedValue;
                            string end = (Int32.Parse(form.GetValue("end-month").AttemptedValue) + 1).ToString("00")
                                         + "/" + (Int32.Parse(form.GetValue("end-day").AttemptedValue) + 1).ToString(
                                             "00") + "/" + form.GetValue("end-year").AttemptedValue;
                            tr.StartDate = DateTime.ParseExact(start, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                            tr.EndDate = DateTime.ParseExact(end, "MM/dd/yyyy", CultureInfo.InvariantCulture);
                            tr.Create();
                        }
                        catch (Exception e)
                        {
                            json["error"] = true;
                            json["message"] = e.Message;
                        }
                    }
                }
                else // else return an error
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
        public ContentResult Delete(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            if(!this.CheckLogin(AccountType.Applicant) && 
               ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources){
                if (form.GetValue("ID") != null)
                {
                    Training tr = new Training(Int32.Parse(form.GetValue("ID").AttemptedValue)).Delete();
                }
                else
                {
                    json["error"] = true;
                    json["message"] = "Invalid Training number";
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
        public ContentResult Get()
        {
            JObject json = new JObject();

            try
            {
                Training tr = new Training(Int32.Parse(Request.QueryString["ID"].ToString()));
                json["training"] = JObject.FromObject(tr);
            } catch (Exception e)
            {
                json["training"] = "";
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
                                    "id", "desc", "title", "faci", "loc", "start-year", "start-month", "start-day",
                                    "end-month", "end-year", "end-day"
                                };

            if (this.HasValues(form, keys))
            {
                if (!this.CheckLogin(AccountType.Applicant) &&
                    ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
                {
                    try
                    {
                        Training th = new Training(Int32.Parse(form.GetValue("id").AttemptedValue));
                        th.Description = form.GetValue("desc").AttemptedValue;
                        th.Title = form.GetValue("title").AttemptedValue;
                        th.Facilitator = form.GetValue("faci").AttemptedValue;
                        th.Location = form.GetValue("loc").AttemptedValue;
                        th.StartDate = DateTime.ParseExact(
                            form.GetValue("start-year").AttemptedValue + "-"
                                                                       + (Int32.Parse(form.GetValue("start-month").AttemptedValue) + 1).ToString("00") + "-"
                                                                       + Int32.Parse(form.GetValue("start-day").AttemptedValue).ToString("00"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                        th.EndDate = DateTime.ParseExact(
                            form.GetValue("end-year").AttemptedValue + "-"
                                                                     + (Int32.Parse(form.GetValue("end-month").AttemptedValue) + 1).ToString("00") + "-"
                                                                     + Int32.Parse(form.GetValue("end-day").AttemptedValue).ToString("00"), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        th.Update();
                    } catch (Exception e)
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

                json["error"] = true;
                json["message"] = "Form is incomplete";
            }
            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetTrainings()
        {
            JObject json = new JObject();
            json["pages"] = 1;
            List<Training> Training = new List<Training>();

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
                Training.Add(new Training(Int32.Parse(row.ItemArray[0].ToString())));
            }

            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["trainings"] = JArray.FromObject(Training);
            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult GetAssigned(FormCollection form)
        {
            JObject json = new JObject();
            json["pages"] = 1;
            List<AssignedTraining> Training = new List<AssignedTraining>();

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
            if (((Employee)this.GetAccount().Profile).Department.Type
                != DepartmentType.HumanResources)
            {
                constraints += " AND Profile = " + this.GetAccount().Profile.Profile.ProfileID;
            }

            string sql = "SELECT AssignedTrainingID FROM "
                         + " AssignedTraining A INNER JOIN TrainingHistory T ON A.Training = T.TrainingHistoryID" + constraints;

            foreach (DataRow row in pt.Get(sql))
            {
                Training.Add(new AssignedTraining(Int32.Parse(row["AssignedTrainingID"].ToString())));
            }

            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["trainings"] = JArray.FromObject(Training);
            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult UpdateAssigned(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            if (!this.CheckLogin(AccountType.Applicant) && ((Employee)this.GetAccount().Profile).Department.Type
                == DepartmentType.HumanResources)
            {
                if (form.GetValue("v") != null && Int32.Parse(form.GetValue("v").AttemptedValue) <= 2 || Int32.Parse(form.GetValue("v").AttemptedValue) >= 1)
                {
                    AssignedTraining at = new AssignedTraining(Int32.Parse(form.GetValue("id").AttemptedValue));
                    at.Status = (TrainingStatus)Int32.Parse(form.GetValue("v").AttemptedValue);
                    at.Update(false);
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

        [HttpGet]
        public ContentResult GetList()
        {
            JObject json = new JObject();
            List<Training> Trainings = new List<Training>();
            List<Profile> Profiles = new List<Profile>();

            DBHandler db = new DBHandler();
            using (DataTable dt = db.Execute<DataTable>(CRUD.READ, "SELECT TrainingHistoryID FROM TrainingHistory"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Trainings.Add(new Training(Int32.Parse(row["TrainingHistoryID"].ToString())));
                }
            }

            using (DataTable dt = db.Execute<DataTable>(CRUD.READ, "SELECT Profile FROM Employee"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Profiles.Add(new Profile(Int32.Parse(row["Profile"].ToString())));
                }
            }

            json["trainings"] = JArray.FromObject(Trainings);
            json["profiles"] = JArray.FromObject(Profiles);

            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult Assign(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            if (!this.CheckLogin(AccountType.Applicant) && this.GetAccount().Type == AccountType.DepartmentHead)
            {
                if (form.GetValue("profile") != null && form.GetValue("training") != null)
                {
                    try
                    {
                        Profile Profile = new Profile(Int32.Parse(form.GetValue("profile").AttemptedValue));
                        Training Training = new Training(Int32.Parse(form.GetValue("training").AttemptedValue));

                        AssignedTraining at = new AssignedTraining();
                        at.Profile = Profile;
                        at.Training = Training;

                        if (form.GetValue("suggested") == null)
                            at.Status = TrainingStatus.Pending;
                        else
                            at.Status = TrainingStatus.Suggested;

                        at.Create(false);

                        Notification notif = new Notification();
                        notif.Account = new Account().FindByProfile(at.Profile.ProfileID);

                        if (at.Status == TrainingStatus.Pending)
                        {
                            notif.Message =
                                "You've been assigned to a Training service, please check the Assigned Trainings page.";
                            json["message"] = "Succesfully assigned employee to training service...";
                        }
                        else
                        {
                            notif.Message =
                                "You were suggested to join a Training service, please wait for further instructions.";
                            json["message"] = "Succesfully suggested employee to training service...";
                        }

                        notif.Status = NotificationStatus.Unread;
                        notif.TimeStamp = DateTime.Now;

                        notif.Create();

                        DBHandler db = new DBHandler();
                        using (DataTable dt = db.Execute<DataTable>(
                            CRUD.READ,
                            "SELECT Profile FROM Employee E INNER JOIN Department D ON E.Department = D.DepartmentID WHERE Type = "
                            + ((int)DepartmentType.HumanResources)))
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                Account a = new Account().FindByProfile(Int32.Parse(row["Profile"].ToString()), false);
                                Notification n = new Notification();
                                n.Status = NotificationStatus.Unread;
                                n.TimeStamp = DateTime.Now;
                                n.Account = a;
                                n.Message = "An employee was suggested for training";

                                n.Create();
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

        [HttpGet]
        public ContentResult GetDepartmentEmployeeList()
        {
            JObject json = new JObject();
            List<Employee> Employees = new List<Employee>();

            DBHandler db = new DBHandler();
            using (DataTable dt = db.Execute<DataTable>(
                CRUD.READ,
                "SELECT EmployeeID FROM Employee WHERE Department = "
                + ((Employee)this.GetAccount().Profile).Department.DepartmentID + " AND EmployeeID <> " + ((Employee)this.GetAccount().Profile).EmployeeID))
            {
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        Employees.Add(new Employee(Int32.Parse(row["EmployeeID"].ToString()), true));
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            json["employees"] = JArray.FromObject(Employees);
            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetAllTrainings()
        {
            JObject json = new JObject();
            List<Training> Trainings = new List<Training>();

            DBHandler db = new DBHandler();
            using (DataTable dt = db.Execute<DataTable>(
                CRUD.READ,
                "SELECT TrainingHistoryID FROM TrainingHistory"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        Trainings.Add(new Training(Int32.Parse(row["TrainingHistoryID"].ToString())));
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            json["trainings"] = JArray.FromObject(Trainings);
            return Content(json.ToString(), "application/json");
        }
    }
}