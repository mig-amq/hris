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
    using Microsoft.Ajax.Utilities;

    using Newtonsoft.Json.Linq;

    using WebApplication1.Models;

    public class AttendanceController : GlobalController
    {
        // GET: Attendance
        public ActionResult Index()
        {
            return View();
        }

        // GET: Attendance/Add
        public ActionResult Add()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (this.CheckLogin(AccountType.Applicant) || ((Employee)this.GetAccount().Profile).Department.Type
                != DepartmentType.HumanResources)
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        // GET: Attendance/Add
        public ActionResult List()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

            if (!this.CheckLogin(AccountType.Employee) && !this.CheckLogin(AccountType.DepartmentHead))
                return this.RedirectToAction("Dashboard", "Home");

            return View();
        }

        [HttpGet]
        public ContentResult GetAttendances()
        {
            JObject json = new JObject();
            json["pages"] = 1;
            List<Attendance> Attendances = new List<Attendance>();

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

            string sql = "SELECT * "
                         + "FROM(SELECT CONCAT(P.FirstName, ' ', P.LastName, ' ', P.MiddleName) AS FullName, "
                         + "AttendanceID AS ID FROM Attendance A INNER JOIN Employee E ON A.Employee = E.EmployeeID "
                         + "INNER JOIN Profile P ON E.Profile = P.ProfileID WHERE MONTH(Date) = " + DateTime.Now.Month
                         + " AND YEAR(Date) = " + DateTime.Now.Year + ") T2 " + constraints;

            foreach (DataRow row in pt.Get(sql))
            {
                Attendances.Add(new Attendance(Int32.Parse(row["ID"].ToString())));
            }


            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["attendances"] = JArray.FromObject(Attendances);
            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetAttendanceToday()
        {
            JObject json = new JObject();
            json["attendance"] = null;

            if (this.GetAccount() != null)
            {
                if (!this.CheckLogin(AccountType.Applicant))
                {
                    try
                    {
                        Attendance at = new Attendance().Find(
                            ((Employee)this.GetAccount().Profile).EmployeeID,
                            DateTime.Now,
                            recursive: false,
                            byPrimary: false);

                        AttendanceTime aTime = new AttendanceTime(at.AttendanceID, recursive: true, byPrimary: false);
                        json["attendance"] = JObject.FromObject(aTime);
                    }
                    catch (Exception e)
                    {
                        json["attendance"] = null;
                    }
                }
            }
            else
            {
                json["error"] = false;
                json["message"] = "";

                if (Request.QueryString["code"] != null)
                {
                    string code = Request.QueryString["code"].ToString();
                    DBHandler db = new DBHandler();

                    Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
                    param.Add("@Code", code);
                    using (DataTable dt = db.Execute<DataTable>(
                        CRUD.READ,
                        "SELECT EmployeeID, A.Image FROM Employee E INNER JOIN Account A ON E.Profile = A.Profile WHERE E.Code = @Code AND (A.Type =" 
                        + ((int)AccountType.Employee) + " OR A.Type = " + ((int)AccountType.DepartmentHead) + ")", param))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            json["error"] = false;
                            json["message"] = "Succesfully found your employee data";
                            json["image"] = dt.Rows[0]["Image"].ToString();

                            try
                            {
                                Attendance at = new Attendance().Find(
                                    Int32.Parse(dt.Rows[0]["EmployeeID"].ToString()),
                                    DateTime.Now,
                                    recursive: true,
                                    byPrimary: false);

                                json["employee"] = JObject.FromObject(at.Employee);
                                try
                                {
                                    AttendanceTime aTime = new AttendanceTime(
                                        at.AttendanceID,
                                        recursive: true,
                                        byPrimary: false);
                                    json["attendance"] = JObject.FromObject(aTime);
                                }
                                catch (Exception e){}

                                json["onleave"] = at.Employee.OnLeave(DateTime.Now);
                            }
                            catch (Exception e)
                            {
                                json["error"] = true;
                                json["message"] =
                                    "<b>Error: </b> You do not have an attendance sheet for this month yet, please contact the HR department.";
                            }
                        }
                        else
                        {
                            json["error"] = true;
                            json["message"] = "That is not a valid employee code";
                        }
                    }
                }
                else
                {
                    json["error"] = true;
                    json["message"] = "Form is incomplete";
                }
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetMyAttendance()
        {
            JObject json = new JObject();
            json["attendance"] = null;

            if (!this.CheckLogin(AccountType.Applicant))
            {
                try
                {
                    json["attendance"] = JObject.FromObject(new Attendance().Find(((Employee)this.GetAccount().Profile).EmployeeID,
                        DateTime.Now, recursive: false, byPrimary: false));
                }
                catch (Exception e)
                {
                    json["attendance"] = null;
                }
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetAttendanceTimes()
        {
            JObject json = new JObject();
            List<AttendanceTime> AttendanceTime = new List<AttendanceTime>();
            json["attendancetimes"] = null;
            json["pages"] = 1;
            int page = Int32.Parse(Request.QueryString["page"]);
            int entries = 5;
            int id = Int32.Parse(Request.QueryString["id"]);

            Paginator pt = new Paginator(entries, page);

            if (!this.CheckLogin(AccountType.Applicant))
            {
                string sql = "SELECT AT.AttendanceTimeID "
                             + "FROM Attendance A INNER JOIN AttendanceTime AT ON A.AttendanceID = AT.Attendance "
                             + "WHERE Employee = " + id + " AND MONTH(AT.Date) = " 
                             + DateTime.Now.Month + " AND YEAR(AT.Date) = " + DateTime.Now.Year;

                foreach (DataRow row in pt.Get(sql))
                {
                    try
                    {
                        AttendanceTime.Add(
                            new AttendanceTime(
                                Int32.Parse(row["AttendanceTimeID"].ToString()),
                                recursive: true,
                                byPrimary: true));
                    }
                    catch (Exception e)
                    {
                    }
                }
            }
            
            if (AttendanceTime.Count > 0)
                json["attendancetimes"] = JArray.FromObject(AttendanceTime);

            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetAllAttendanceTimes()
        {
            JObject json = new JObject();
            List<AttendanceTime> AttendanceTime = new List<AttendanceTime>();
            json["attendancetimes"] = null;

            if (!this.CheckLogin(AccountType.Applicant))
            {
                string sql = "SELECT AT.AttendanceTimeID "
                             + "FROM Attendance A INNER JOIN AttendanceTime AT ON A.AttendanceID = AT.Attendance "
                             + "WHERE Employee = " + (Int32.Parse(Request.QueryString["id"])) + " AND MONTH(AT.Date) = "
                             + DateTime.Now.Month + " AND YEAR(AT.Date) = " + DateTime.Now.Year;

                DBHandler db = new DBHandler();
                using (DataTable dt = db.Execute<DataTable>(CRUD.READ, sql))
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        AttendanceTime.Add(new AttendanceTime(Int32.Parse(row["AttendanceTimeID"].ToString()), recursive: true, byPrimary: true));
                    }
                }
            }

            if (AttendanceTime.Count > 0)
                json["attendancetimes"] = JArray.FromObject(AttendanceTime);
            
            return Content(json.ToString(), "application/json");
        }
        [HttpGet]
        // GET: Attendance/GetAllNoAttenance
        public ContentResult GetAllNoAttenance()
        {
            JObject json = new JObject();
            List<Employee> Employees = new List<Employee>();
            DBHandler db = new DBHandler();
            using (DataTable dt = db.Execute<DataTable>(
                CRUD.READ,
                "SELECT EmployeeID FROM Employee E INNER JOIN Account A ON E.Profile = A.Profile WHERE (A.Type = "
                + ((int)AccountType.Employee) + " OR A.Type = " + ((int)AccountType.DepartmentHead)
                + ") AND EmployeeID NOT IN (SELECT Employee FROM Attendance)"))
            {
                foreach (DataRow row in dt.Rows)
                {
                    try
                    {
                        Employees.Add(new Employee(Int32.Parse(row["EmployeeID"].ToString()), byPrimary: true));
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
        // GET: Attendance/GetNoAttendance
        public ContentResult GetNoAttendance()
        {
            JObject json = new JObject();
            json["pages"] = 1;
            List<Employee> Employees = new List<Employee>();

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

            string sql = "SELECT * "
                         + "FROM(SELECT CONCAT(P.FirstName, ' ', P.LastName, ' ', P.MiddleName) AS FullName, "
                         + "EmployeeID AS ID FROM Employee E INNER JOIN Profile P ON E.Profile = P.ProfileID "
                         + "INNER JOIN Account A ON A.Profile = E.Profile WHERE EmployeeID NOT IN "
                         + "(SELECT Employee FROM Attendance WHERE MONTH(Date) = " + DateTime.Now.Month 
                         + "AND YEAR(Date) = " + DateTime.Now.Year + ") AND (A.Type = " 
                         + ((int) AccountType.DepartmentHead) + " OR A.Type = " + ((int) AccountType.Employee) 
                         + ")) T2 " + constraints;
            
            foreach (DataRow row in pt.Get(sql))
            {
                Employees.Add((Employee)new Employee().FindProfile(Int32.Parse(row["ID"].ToString()), true, true));
            }


            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["employees"] = JArray.FromObject(Employees);
            return Content(json.ToString(), "application/json");
        }

        // POST: Attendance/AddAttendance
        [HttpPost]
        public ContentResult AddAttendance(FormCollection form)
        {
            // add new attendance row for an employee
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new string[]{"id", "v"};

            if (this.HasValues(form, keys))
            {
                if (!this.CheckLogin(AccountType.Applicant) && ((Employee)this.GetAccount().Profile).Department.Type
                    == DepartmentType.HumanResources)
                {
                    try
                    {
                        if (String.IsNullOrEmpty(form.GetValue("id").AttemptedValue))
                            throw new Exception("Form is incomplete");

                        Employee emp = new Employee(Int32.Parse(form.GetValue("id").AttemptedValue), true);

                        Attendance at = new Attendance();
                        try
                        {
                            at = new Attendance().Find(emp.EmployeeID, DateTime.Now, recursive: false, byPrimary:false);
                        }
                        catch (Exception e)
                        {
                            at.Absent = 0;
                            at.Late = 0;
                            at.Overtime = 0;
                            at.Present = 0;
                            at.Undertime = 0;
                            at.Date = DateTime.Now;
                            at.Employee = emp;
                            at.Leave = emp.GetNumLeaves(DateTime.Now);

                            if (String.IsNullOrEmpty(form.GetValue("v").AttemptedValue))
                                throw new Exception("Form is incomplete");

                            at.TotalWorkingDays = Int32.Parse(form.GetValue("v").AttemptedValue);

                            at.Create();

                            json["message"] = "Successfully created Attendance row for: <b>" + 
                                              emp.Profile.FirstName + " " + emp.Profile.LastName + "</b>";
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

        // POST Attendance/AddAll
        [HttpPost]
        public ContentResult AddAll(FormCollection form)
        {
            // add new attendance row for all employees
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new string[] { "v" };
            int count = 0;

            if (this.HasValues(form, keys))
            {
                if (!this.CheckLogin(AccountType.Applicant) && ((Employee)this.GetAccount().Profile).Department.Type
                    == DepartmentType.HumanResources)
                {
                    try
                    {
                        DBHandler db = new DBHandler();

                        using (DataTable dt = db.Execute<DataTable>(
                            CRUD.READ,
                            "SELECT Profile FROM Account WHERE Type = " + ((int)AccountType.Employee) + " OR Type = "
                            + ((int)AccountType.DepartmentHead)))
                        {
                            foreach (DataRow row in dt.Rows)
                            {
                                Employee emp = new Employee(Int32.Parse(row["Profile"].ToString()));
                                Attendance at = new Attendance();
                                try
                                {
                                    at = new Attendance().Find(emp.EmployeeID, DateTime.Now, recursive: false, byPrimary: false);
                                }
                                catch (Exception e)
                                {
                                    at.Absent = 0;
                                    at.Late = 0;
                                    at.Leave = emp.GetNumLeaves(DateTime.Now);
                                    at.Overtime = 0;
                                    at.Present = 0;
                                    at.Undertime = 0;
                                    at.Date = DateTime.Now;
                                    at.Employee = emp;
                                    if (String.IsNullOrEmpty(form.GetValue("v").AttemptedValue))
                                        throw new Exception("Form is incomplete");

                                    at.TotalWorkingDays = Int32.Parse(form.GetValue("v").AttemptedValue);

                                    at.Create();
                                    count++;
                                }
                            }

                            json["message"] = "Successfully create Attendance rows for " + count + " employees";
                        }
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
            }
            else
            {
                json["error"] = true;
                json["message"] = "Form is incomplete";
            }

            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult TimeIn(FormCollection form)
        {
            // time in
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";
            
            Employee emp = null;

            DBHandler db = new DBHandler();

            if (this.GetAccount() != null && this.GetAccount().Type != AccountType.Applicant)
            {
                emp = ((Employee)this.GetAccount().Profile);
            }
            else
            {
                if (form.GetValue("code") != null)
                {
                    string code = form.GetValue("code").AttemptedValue;
                    Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
                    param.Add("@Code", code);

                    using (DataTable dt = db.Execute<DataTable>(
                            CRUD.READ,
                            "SELECT EmployeeID FROM Employee E INNER JOIN Account A ON E.Profile = A.Profile WHERE E.Code = @Code AND (A.Type ="
                            + ((int)AccountType.Employee) + " OR A.Type = " + ((int)AccountType.DepartmentHead) + ")", param))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {
                                emp = new Employee(Int32.Parse(dt.Rows[0]["EmployeeID"].ToString()), byPrimary: true);
                            }
                            catch (Exception e)
                            {
                                emp = null;
                            }
                        }
                    }
                }
            }

            if (emp != null || (this.GetAccount() != null && 
                (this.CheckLogin(AccountType.Employee) || this.CheckLogin(AccountType.DepartmentHead))))
            {
                Attendance at = new Attendance();
                try
                {
                    at = at.Find(
                        emp.EmployeeID,
                        DateTime.Now,
                        recursive: true,
                        byPrimary: false);

                    AttendanceTime aTime = new AttendanceTime();

                    try
                    {
                        aTime.Find(at.AttendanceID, DateTime.Now, recursive: false, byPrimary: false);

                        json["error"] = true;
                        json["message"] = "You have already timed in today";
                    }
                    catch (Exception e)
                    {
                        aTime.Attendance = at;
                        aTime.TimeIn = DateTime.Now;
                        aTime.Date = DateTime.Now;

                        aTime.Create();

                        DateTime callTime = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " " 
                                            + "08:00 AM", "yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
                        DateTime endTime = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " "
                                            + "05:00 PM", "yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
                        json["attendance"] = aTime.AttendanceTimeID;
                        json["message"] = "Successfully Timed In today.";

                        if (DateTime.Compare(aTime.TimeIn.Value, callTime) > 0 && DateTime.Compare(aTime.TimeIn.Value, endTime) < 0)
                        {
                            at.Late += 1;
                            at.Update(false);
                            json["message"] = "Successfully Timed In today. However, you are late";
                        }
                        else if (DateTime.Compare(aTime.TimeIn.Value, endTime) >= 0)
                        {
                            at.Absent += 1;
                            at.Update(false);
                            json["message"] = "You have Timed In today. However, you are already counted as absent";
                        }
                    }
                }
                catch (Exception e)
                {
                    json["error"] = true;
                    json["message"] =
                        "Cannot find an attendance row for your account, please consult the HR Department";
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
        public ContentResult TimeOut(FormCollection form)
        {
            // time outJObject json = new JObject();
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            Employee emp = null;

            DBHandler db = new DBHandler();

            if (this.GetAccount() != null && this.GetAccount().Type != AccountType.Applicant)
            {
                emp = ((Employee)this.GetAccount().Profile);
            }
            else
            {
                if (form.GetValue("code") != null)
                {
                    string code = form.GetValue("code").AttemptedValue;
                    Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
                    param.Add("@Code", code);
                    using (DataTable dt = db.Execute<DataTable>(
                        CRUD.READ,
                        "SELECT EmployeeID FROM Employee E INNER JOIN Account A ON E.Profile = A.Profile WHERE E.Code = @Code AND (A.Type ="
                        + ((int)AccountType.Employee) + " OR A.Type = " + ((int)AccountType.DepartmentHead) + ")", param))
                    {
                        if (dt.Rows.Count > 0)
                        {
                            try
                            {
                                emp = new Employee(Int32.Parse(dt.Rows[0]["EmployeeID"].ToString()), byPrimary: true);
                            }
                            catch (Exception e)
                            {
                                emp = null;
                            }
                        }
                    }
                }
            }

            if (emp != null && this.GetAccount() == null || (this.GetAccount() != null &&
                (this.CheckLogin(AccountType.Employee) || this.CheckLogin(AccountType.DepartmentHead))))
            {
                try
                {
                    Attendance at = new Attendance().Find(
                        ((Employee)this.GetAccount().Profile).EmployeeID,
                        DateTime.Now,
                        recursive: true,
                        byPrimary: false);

                    AttendanceTime aTime = new AttendanceTime();
                    try
                    {
                        aTime.Find(at.AttendanceID, DateTime.Now, recursive: false, byPrimary: false);

                        if (aTime.TimeIn.HasValue && !aTime.TimeOut.HasValue)
                        {
                            aTime.Attendance = at;
                            aTime.TimeOut = DateTime.Now;

                            aTime.Update(false);

                            DateTime callTime = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " "
                                                                                                        + "08:00 AM", "yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);
                            DateTime endTime = DateTime.ParseExact(DateTime.Now.ToString("yyyy-MM-dd") + " "
                                                                                                       + "05:00 PM", "yyyy-MM-dd hh:mm tt", CultureInfo.InvariantCulture);

                            json["message"] = "Successfully Timed Out today.";

                            if (DateTime.Compare(aTime.TimeOut.Value, endTime) < 0)
                            {
                                at.Undertime += 1;
                                json["message"] = "Successfully Timed Out today. However you've timed out early (Undertime)";
                            }
                            else if (DateTime.Compare(aTime.TimeOut.Value, endTime.AddMinutes(30)) > 0 &&
                                     !(DateTime.Compare(aTime.TimeIn.Value, endTime) >= 0))
                            {
                                at.Overtime += 1;
                                json["message"] = "Successfully Timed Out today. However you've timed out late (Overtime)";
                            }

                            if (DateTime.Compare(aTime.TimeIn.Value, callTime) >= 0 && DateTime.Compare(aTime.TimeIn.Value, endTime) < 0)
                            {
                                at.Present += 1;
                            }

                            at.Update(false);
                        }
                        else
                        {
                            json["error"] = true;
                            json["message"] = "You've already timed out today";
                        }
                    }
                    catch (Exception e)
                    {
                        json["error"] = true;
                        json["message"] = "You haven't timed in today";
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

            return Content(json.ToString(), "application/json");
        }
    }
}