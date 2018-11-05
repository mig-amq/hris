using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using System.Data;
    using System.Globalization;

    using Newtonsoft.Json.Linq;

    using WebApplication1.Models;

    public class ReportsController : GlobalController
    {
        // GET: Reports
        public ActionResult Index()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (this.CheckLogin(AccountType.Applicant) || (!this.CheckLogin(AccountType.Applicant) && 
                ((Employee)this.GetAccount().Profile).Department.Type != DepartmentType.HumanResources))
                return this.RedirectToAction("Dashboard", "Home");

            return RedirectToAction("Attendance", "Reports");
        }

        // GET: Reports/Attendance
        public ActionResult Attendance()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (this.CheckLogin(AccountType.Applicant) || (!this.CheckLogin(AccountType.Applicant) &&
                ((Employee)this.GetAccount().Profile).Department.Type != DepartmentType.HumanResources))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        // GET: Reports/Evaluation
        public ActionResult Evaluation()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (this.CheckLogin(AccountType.Applicant) || (!this.CheckLogin(AccountType.Applicant) &&
                                                           ((Employee)this.GetAccount().Profile).Department.Type != DepartmentType.HumanResources))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        // GET: Reports/List
        public ActionResult List()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (this.CheckLogin(AccountType.Applicant) || (!this.CheckLogin(AccountType.Applicant) &&
                                                           ((Employee)this.GetAccount().Profile).Department.Type != DepartmentType.HumanResources))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        // GET: Reports/Training
        public ActionResult Training()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (this.CheckLogin(AccountType.Applicant) || (!this.CheckLogin(AccountType.Applicant) &&
                                                           ((Employee)this.GetAccount().Profile).Department.Type != DepartmentType.HumanResources))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        // GET: Reports/Printable
        public ActionResult Printable()
        {
            return View();
        }

        // GET: Reports/AllAttendance
        public ContentResult AllAttendance()
        {
            JObject json = new JObject();
            json["content"] = null;
            List<Attendance> Attendances = new List<Attendance>();

            DateTime date = DateTime.Now;

            if (!String.IsNullOrEmpty(Request.QueryString["date"]))
            {
                try
                {
                    date = DateTime.ParseExact(Request.QueryString["date"], "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    date = DateTime.Now;
                }
            }

            DBHandler db = new DBHandler();
            using (DataTable dt = db.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM Attendance WHERE MONTH(Date) = " + date.Month + " AND YEAR(Date) = " + date.Year))
            {
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        Attendance at = new Attendance();
                        at.TotalWorkingDays = Int32.Parse(row["TotalWorkingDays"].ToString());
                        at.Present = Int32.Parse(row["Present"].ToString());
                        at.Absent = Int32.Parse(row["Absent"].ToString());
                        at.Late = Int32.Parse(row["Late"].ToString());
                        at.Overtime = Int32.Parse(row["Overtime"].ToString());
                        at.Undertime = Int32.Parse(row["Undertime"].ToString());
                        at.AttendanceID = Int32.Parse(row["AttendanceID"].ToString());
                        at.Leave = Int32.Parse(row["Leave"].ToString());
                        at.Date = DateTime.Parse(row["Date"].ToString());

                        at.Employee = (Employee)new Employee().FindProfile(
                            Int32.Parse(row["Employee"].ToString()),
                            byPrimary: true);

                        Attendances.Add(at);
                    } catch(Exception e) { }
                }

            }

            if (Attendances.Count > 0)
                json["content"] = JArray.FromObject(Attendances);

            return Content(json.ToString(), "application/json");

        }

        // GET: Reports/AllEmployee
        public ContentResult AllEmployee()
        {
            JObject json = new JObject();
            json["content"] = null;
            List<Employee> Employees = new List<Employee>();

            DBHandler db = new DBHandler();
            using (DataTable dt = db.Execute<DataTable>(CRUD.READ, "SELECT * FROM Employee"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        Employee emp = new Employee();
                        emp.EmployeeID = Int32.Parse(row["EmployeeID"].ToString());
                        emp.Status = (StatusType)Enum.Parse(typeof(StatusType), row["Status"].ToString(), true);
                        emp.Code = row["Code"].ToString();
                        emp.Position = row["Position"].ToString();
                        emp.EmploymentDate = DateTime.Parse(row["EmploymentDate"].ToString());

                        if (emp.Status == StatusType.Inactive)
                            emp.DateInactive = DateTime.Parse(row["DateInactive"].ToString());
                        else
                            emp.DateInactive = null;

                        emp.Profile = new Profile(Int32.Parse(row["Profile"].ToString()));
                        try
                        {
                            emp.Department = new Department(Int32.Parse(row["Department"].ToString()), recursive: true);
                        }
                        catch (Exception e)
                        {
                            emp.Department = null;
                        }

                        Employees.Add(emp);
                    }
                    catch (Exception e) { }
                }
            }

            if (Employees.Count > 0)
                json["content"] = JArray.FromObject(Employees);

            return Content(json.ToString(), "application/json");
        }

        // GET: Reports/AllTrainings
        public ContentResult AllTraining()
        {
            JObject json = new JObject();
            json["content"] = null;
            List<AssignedTraining> AssignedTrainings = new List<AssignedTraining>();

            DBHandler db = new DBHandler();
            using (DataTable dt = db.Execute<DataTable>(
                CRUD.READ,
                "SELECT AssignedTrainingID, Status, TrainingHistoryID, Profile FROM AssignedTraining AT"
                + " INNER JOIN TrainingHistory T ON AT.Training = T.TrainingHistoryID "
                + " ORDER BY AT.Profile DESC, T.StartDate DESC"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    AssignedTraining at = new AssignedTraining();
                    at.Status = (TrainingStatus)Int32.Parse(row["Status"].ToString());

                    at.Profile = new Profile(Int32.Parse(row["Profile"].ToString()));
                    at.Training = new Training(Int32.Parse(row["TrainingHistoryID"].ToString()));

                    AssignedTrainings.Add(at);
                }
            }

            if (AssignedTrainings.Count > 0)
                json["content"] = JArray.FromObject(AssignedTrainings);

            return Content(json.ToString(), "application/json");
        }

        // GET: Reports/Evaluation
        public ContentResult AllEvaluation()
        {
            JObject json = new JObject();
            json["content"] = null;
            List<Appraisal> Appraisals = new List<Appraisal>();

            if (!String.IsNullOrEmpty(Request.QueryString["id"]))
            {
                DBHandler db = new DBHandler();

                using (DataTable dt = db.Execute<DataTable>(
                    CRUD.READ,
                    "SELECT * FROM Appraisal WHERE DiscussedWith = " + Int32.Parse(Request.QueryString["id"]) + " ORDER BY CoveredPeriod DESC"))
                {
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        Appraisal ap = new Appraisal();

                        ap.AppraisalID = Int32.Parse(row["AppraisalID"].ToString());
                        ap.CoveredPeriod = DateTime.Parse(row["CoveredPeriod"].ToString());
                        ap.Criteria = row["Criteria"].ToString();
                        ap.Rating = Double.Parse(row["Rating"].ToString());
                        ap.TechComp = Double.Parse(row["TechComp"].ToString());
                        ap.InterSkills = Double.Parse(row["InterSkills"].ToString());
                        ap.CommComp = Double.Parse(row["CommComp"].ToString());
                        ap.Total = Double.Parse(row["Total"].ToString());
                        ap.Comments = row["Comments"].ToString();

                        if (!row.IsNull("DatePrepared"))
                            ap.DatePrepared = DateTime.Parse(row["DatePrepared"].ToString());

                        if (!row.IsNull("DateNoted"))
                            ap.DateNoted = DateTime.Parse(row["DateNoted"].ToString());

                        if (!row.IsNull("DateDiscussed"))
                            ap.DateDiscussed = DateTime.Parse(row["DateDiscussed"].ToString());

                        ap.Type = (AppraisalType)Int32.Parse(row["Type"].ToString());
                        ap.Status = (AppraisalStatus)Int32.Parse(row["Status"].ToString());

                        if (!row.IsNull("Evaluator"))
                            ap.Evaluator = (Employee)new Employee().FindProfile(Int32.Parse(row["Evaluator"].ToString()), byPrimary: true);

                        if (!row.IsNull("NotedBy"))
                            ap.NotedBy = (Employee)new Employee().FindProfile(Int32.Parse(row["NotedBy"].ToString()), byPrimary: true);

                        if (!row.IsNull("DiscussedWith"))
                            ap.DiscussedWith = (Employee)new Employee().FindProfile(Int32.Parse(row["DiscussedWith"].ToString()), byPrimary: true);

                        Appraisals.Add(ap);
                    }
                }
            }

            if (Appraisals.Count > 0)
                json["content"] = JArray.FromObject(Appraisals);

            return Content(json.ToString(), "application/json");
        }
    }
}