﻿// using Newtonsoft.Json.Linq;
// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Web;
// using System.Web.Mvc;
//
// namespace WebApplication1.Controllers
// {
//     using Calfurn.Models;
//     using System.Data;
//     using System.Globalization;
//     using System.Text.RegularExpressions;
//     using WebApplication1.Models;
//
//     public class RecruitmentController_temp : GlobalController
//     {
//         // GET: Recruitment
//         public ActionResult Index()
//         {
//             return View();
//         }
//
//         public ActionResult ApplicantStatus()
//         {
//             if (!this.CheckLogin(AccountType.Applicant) && this.GetAccount() != null &&
//                 ((Employee)this.GetAccount().Profile).Department.Type != DepartmentType.HumanResources)
//                 return this.RedirectToAction("Dashboard", "Home");
//
//             return View();
//         }
//
//         [HttpGet]
//         public ContentResult GetJob()
//         {
//             JObject json = new JObject();
//
//             try
//             {
//                 json["jobs"] = JObject.FromObject(new JobPosting(Int32.Parse(Request.QueryString["id"])));
//             }
//             catch (Exception e)
//             {
//                 json["jobs"] = "";
//             }
//
//             return Content(json.ToString(), "application/json");
//         }
//
//         [HttpGet]
//         public ContentResult GetJobs()
//         {
//             JObject json = new JObject();
//             json["pages"] = 1;
//             List<JobPosting> Jobs = new List<JobPosting>();
//
//             string[] query = null;
//             int entries = Int32.Parse(Request.QueryString["entries"]);
//             int page = Int32.Parse(Request.QueryString["page"]);
//
//             if (Request.QueryString["query"] != null)
//             {
//                 query = Regex.Replace(Request.QueryString["query"].Trim(), @"\s+", " ").Split(' ');
//             }
//
//             string constraints = "";
//
//             if (query != null)
//             {
//                 for (int i = 0; i < query.Length; i++)
//                 {
//                     constraints += " JobTitle LIKE '%" + query[i] + "%'";
//
//                     if (i < query.Length - 1)
//                     {
//                         constraints += " OR ";
//                     }
//                 }
//             }
//
//             Paginator pt = new Paginator(entries, page);
//
//             if (constraints.Length > 1)
//             {
//                 constraints = " WHERE " + constraints;
//             }
//
//             string sql = "SELECT PostingID FROM JobPosting " + constraints;
//
//             foreach (DataRow row in pt.Get(sql))
//             {
//                 Jobs.Add(new JobPosting(Int32.Parse(row["PostingID"].ToString())));
//             }
//
//
//             json["pages"] = pt.Pages;
//             json["total"] = pt.Total;
//             json["jobs"] = JArray.FromObject(Jobs);
//             return Content(json.ToString(), "application/json");
//         }
//
//         [HttpPost]
//         public ContentResult Create(FormCollection form)
//         {
//             JObject json = new JObject();
//             json["error"] = false;
//             json["message"] = "";
//
//             string[] keys = new string[]
//                             {
//                                 "job-title", "job-desc", "job-req", "year", "month", "day"
//                             };
//
//             if (this.HasValues(form, keys))
//             {
//                 if (!this.CheckLogin(AccountType.Applicant) &&
//                     ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
//                 {
//                     JobPosting job = new JobPosting();
//                     job.JobTitle = form.GetValue("job-title").AttemptedValue;
//                     job.JobDescription = form.GetValue("job-desc").AttemptedValue;
//                     job.Requirements = form.GetValue("job-req").AttemptedValue;
//                     job.DatePosted = DateTime.ParseExact(
//                         form.GetValue("year").AttemptedValue + "-"
//                                                              + (Int32.Parse(form.GetValue("month").AttemptedValue) + 1).ToString("00") + "-"
//                                                              + Int32.Parse(form.GetValue("day").AttemptedValue).ToString("00"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
//                     job.Create();
//
//                     json["message"] = "Requisition Form has been added to Job Postings";
//                 }
//                 else
//                 {
//                     json["error"] = true;
//                     json["message"] = "You are not authorized to continue";
//                 }
//             }
//
//             return Content(json.ToString(), "application/json");
//         }
//
//         [HttpPost]
//         public ContentResult Update(FormCollection form)
//         {
//             JObject json = new JObject();
//             json["error"] = false;
//             json["message"] = "";
//
//             string[] keys = new string[]
//                                 {
//                                     "job-title", "job-desc", "job-req", "year", "month", "day", "ID"
//                                 };
//
//             if (this.HasValues(form, keys))
//             {
//                 if (!this.CheckLogin(AccountType.Applicant) &&
//                     ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
//                 {
//                     JobPosting job = new JobPosting(Int32.Parse(form.GetValue("ID").AttemptedValue));
//                     job.JobTitle = form.GetValue("job-title").AttemptedValue;
//                     job.JobDescription = form.GetValue("job-desc").AttemptedValue;
//                     job.Requirements = form.GetValue("job-req").AttemptedValue;
//                     job.DatePosted = DateTime.ParseExact(
//                         form.GetValue("year").AttemptedValue + "-"
//                                                              + (Int32.Parse(form.GetValue("month").AttemptedValue) + 1).ToString("00") + "-"
//                                                              + Int32.Parse(form.GetValue("day").AttemptedValue).ToString("00"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
//                     job.Update();
//                 }
//             }
//
//             return Content(json.ToString(), "application/json");
//         }
//
//         [HttpGet]
//         public ContentResult GetApplicants()
//         {
//             JObject json = new JObject();
//             json["pages"] = 1;
//             List<Applicant> Applicant = new List<Applicant>();
//
//             string[] query = null;
//             int entries = Int32.Parse(Request.QueryString["entries"]);
//             int page = Int32.Parse(Request.QueryString["page"]);
//
//             if (Request.QueryString["query"] != null)
//             {
//                 query = Regex.Replace(Request.QueryString["query"].Trim(), @"\s+", " ").Split(' ');
//             }
//
//             string constraints = "";
//
//             if (query != null)
//             {
//                 for (int i = 0; i < query.Length; i++)
//                 {
//                     constraints += " T2.FullName LIKE '%" + query[i] + "%'";
//
//                     if (i < query.Length - 1)
//                     {
//                         constraints += " OR ";
//                     }
//                 }
//             }
//
//             Paginator pt = new Paginator(entries, page);
//
//             if (constraints.Length > 1)
//             {
//                 constraints = " WHERE " + constraints;
//             }
//
//             string sql = "SELECT * FROM ( "
//                          + "SELECT CONCAT(P.FirstName, ' ', P.MiddleName, ' ', P.LastName) AS FullName, A.ApplicantID "
//                          + "FROM Profile P INNER JOIN Applicant A ON A.Profile = P.ProfileID) T2 " + constraints;
//
//             foreach (DataRow row in pt.Get(sql))
//             {
//                 Applicant.Add((Applicant)new Applicant().FindProfile(Int32.Parse(row["ApplicantID"].ToString()), byPrimary: true, recursive: true));
//             }
//
//
//             json["pages"] = pt.Pages;
//             json["total"] = pt.Total;
//             json["applicants"] = JArray.FromObject(Applicant);
//             return Content(json.ToString(), "application/json");
//         }
//
//         [HttpPost]
//         public ContentResult UpdateApplicant(FormCollection form)
//         {
//             JObject json = new JObject();
//             json["error"] = false;
//             json["message"] = "";
//
//             string[] keys = new string[] { "id", "s" };
//
//             if (!this.CheckLogin(AccountType.Applicant) &&
//                 ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
//             {
//                 if (this.HasValues(form, keys))
//                 {
//                     try
//                     {
//                         Applicant a = new Applicant(Int32.Parse(form.GetValue("id").AttemptedValue), true);
//                         a.Status = (ApplicantStatus)Int32.Parse(form.GetValue("s").AttemptedValue);
//                         a.Update(false);
//
//                         if (a.Status == Models.ApplicantStatus.Accepted)
//                         {
//                             json["message"] = "Accepted";
//
//                             // create employee data containing applicant profile
//                             Employee e = new Employee();
//                             e.EmploymentDate = DateTime.Now;
//                             e.Position = "New Employee";
//                             e.Code = Guid.NewGuid().ToString();
//                             e.Profile = a.Profile;
//                             e.Create();
//
//                             DBHandler db = new DBHandler();
//
//                             // update applicant account if it exists
//                             using (DataTable dt = db.Execute<DataTable>(
//                                 CRUD.READ,
//                                 "SELECT * FROM Account WHERE Profile = " + a.Profile.ProfileID))
//                             {
//                                 if (dt.Rows.Count == 1)
//                                 {
//                                     Account ac = new Account().FindById(Int32.Parse(dt.Rows[0]["AccountID"].ToString()), false);
//                                     ac.Type = AccountType.Employee;
//                                     ac.Profile = e;
//
//                                     ac.Update(recursive: false);
//
//                                     Notification notif = new Notification();
//                                     notif.Account = ac;
//                                     notif.Message =
//                                         "Congratulations! You've been approved for employment, please wait for further information, you will be contacted through phone or email.";
//                                     notif.Status = NotificationStatus.Unread;
//                                     notif.TimeStamp = DateTime.Now;
//
//                                     notif.Create();
//                                 }
//                             }
//
//                             using (DataTable dt = db.Execute<DataTable>(
//                                 CRUD.READ,
//                                 "SELECT * FROM Account WHERE Type = " + ((int)AccountType.DepartmentHead)))
//                             {
//                                 if (dt.Rows.Count >= 1)
//                                 {
//                                     Account ac = new Account().FindById(Int32.Parse(dt.Rows[0]["AccountID"].ToString()), false);
//
//                                     Notification notif = new Notification();
//                                     notif.Account = ac;
//                                     notif.Message =
//                                         "Info: An Applicant has been approved for employment";
//                                     notif.Status = NotificationStatus.Unread;
//                                     notif.TimeStamp = DateTime.Now;
//
//                                     notif.Create();
//                                 }
//                             }
//
//                             // delete applicant data
//                             // a.Delete();
//                         }
//                         else
//                         {
//                             json["message"] = "Rejected";
//                         }
//                     }
//                     catch (Exception e)
//                     {
//                         json["error"] = true;
//                         json["message"] = e.Message;
//                     }
//                 }
//                 else
//                 {
//                     json["error"] = true;
//                     json["message"] = "Form is incomplete";
//                 }
//             }
//             else
//             {
//                 json["error"] = true;
//                 json["message"] = "You are not authorized to continue";
//             }
//
//             return Content(json.ToString(), "application/json");
//         }
//
//         [HttpPost]
//         public ContentResult Delete(FormCollection form)
//         {
//             JObject json = new JObject();
//             json["error"] = false;
//             json["message"] = "";
//
//             string[] keys = new string[] { "id" };
//
//             if (!this.CheckLogin(AccountType.Applicant) &&
//                 ((Employee)this.GetAccount().Profile).Department.Type == DepartmentType.HumanResources)
//             {
//                 if (this.HasValues(form, keys))
//                 {
//                     try
//                     {
//                         JobPosting post = new JobPosting(Int32.Parse(form.GetValue("id").AttemptedValue));
//                         post.Delete();
//                     }
//                     catch (Exception e)
//                     {
//                         json["error"] = true;
//                         json["message"] = e.Message;
//                     }
//                 }
//                 else
//                 {
//                     json["error"] = true;
//                     json["message"] = "Missing Job Posting ID";
//                 }
//             }
//             else
//             {
//                 json["error"] = true;
//                 json["message"] = "You are not authorized to continue";
//             }
//
//             return Content(json.ToString(), "application/json");
//         }
//
//         [HttpPost]
//         public ContentResult CreateSchedule(FormCollection form)
//         {
//             JObject json = new JObject();
//             json["error"] = false;
//             json["message"] = "";
//
//             string[] keys = new string[]
//                                 {
//                                     "start", "id"
//                                 };
//
//             if (this.HasValues(form, keys))
//             {
//                 if (!this.CheckLogin(AccountType.Applicant) && ((Employee)this.GetAccount().Profile).Department.Type
//                     == DepartmentType.HumanResources)
//                 {
//                     DateTime Start = DateTime.Parse(form.GetValue("start").AttemptedValue);
//
//                     try
//                     {
//                         JobApplication a = new JobApplication(Int32.Parse(form.GetValue("id").AttemptedValue), byPrimary: true);
//                         Schedule sc = new Schedule();
//                         sc.JobApplication = a;
//                         sc.HR = ((Employee)this.GetAccount().Profile);
//                         sc.TimeStart = Start;
//                         sc.TimeEnd = Start.AddHours(1);
//
//                         sc.Create();
//
//                         Notification notif = new Notification();
//                         notif.Account = new Account().FindByProfile(a.Applicant.Profile.ProfileID, false);
//                         notif.TimeStamp = DateTime.Now;
//                         notif.Status = NotificationStatus.Unread;
//                         notif.Message = "<b>Info: </b> You have been scheduled for an interview with the HR department on "
//                                         + Start.ToString("yyyy/MM/dd") + " " + Start.ToString("hh:mm tt") + " - "
//                                         + sc.TimeEnd.ToString("hh:mm tt");
//                         notif.Create();
//                         json["message"] = "Schedule successfully created...";
//                     }
//                     catch (Exception e)
//                     {
//                         json["error"] = true;
//                         json["message"] = e.Message;
//                     }
//                 }
//                 else
//                 {
//                     json["error"] = true;
//                     json["message"] = "You are not authorized to continue";
//                 }
//             }
//             else
//             {
//                 json["error"] = true;
//                 json["message"] = "Form is incomplete";
//             }
//             return Content(json.ToString(), "application/json");
//         }
//
//         [HttpPost]
//         public ContentResult UpdateSchedule(FormCollection form)
//         {
//             JObject json = new JObject();
//             json["error"] = false;
//             json["message"] = "";
//
//             string[] keys = new string[]
//                                 {
//                                     "start", "id"
//                                 };
//
//             if (this.HasValues(form, keys))
//             {
//                 if (!this.CheckLogin(AccountType.Applicant) && ((Employee)this.GetAccount().Profile).Department.Type
//                     == DepartmentType.HumanResources)
//                 {
//                     try
//                     {
//                         DateTime Start = DateTime.Parse(form.GetValue("start").AttemptedValue);
//
//                         JobApplication a = new JobApplication(Int32.Parse(form.GetValue("id").AttemptedValue), byPrimary: true);
//                         Schedule sc = new Schedule().Find(a.Applicant.ApplicantID, byPrimary: false, recursive: false);
//                         sc.JobApplication = a;
//                         sc.HR = ((Employee)this.GetAccount().Profile);
//                         sc.TimeStart = Start;
//                         sc.TimeEnd = Start.AddHours(1);
//
//                         sc.Update(false);
//
//                         Notification notif = new Notification();
//                         notif.Account = new Account().FindByProfile(a.Applicant.Profile.ProfileID, false);
//                         notif.TimeStamp = DateTime.Now;
//                         notif.Status = NotificationStatus.Unread;
//                         notif.Message = "<b>Info: </b> You have been re-scheduled for an interview with the HR department on "
//                                         + Start.ToString("yyyy/MM/dd") + " " + Start.ToString("hh:mm tt") + " - "
//                                         + sc.TimeEnd.ToString("hh:mm tt");
//                         notif.Create();
//
//                         json["message"] = "Schedule successfully updated...";
//                     }
//                     catch (Exception e)
//                     {
//                         json["error"] = true;
//                         json["message"] = e.Message;
//                     }
//                 }
//                 else
//                 {
//                     json["error"] = true;
//                     json["message"] = "You are not authorized to continue";
//                 }
//             }
//             else
//             {
//                 json["error"] = true;
//                 json["message"] = "Form is incomplete";
//             }
//             return Content(json.ToString(), "application/json");
//         }
//
//         [HttpGet]
//         public ContentResult GetSchedule()
//         {
//             JObject json = new JObject();
//             json["schedule"] = null;
//
//             if (Request.QueryString["id"] != null)
//             {
//                 try
//                 {
//                     json["schedule"] = JObject.FromObject(new Schedule().Find(
//                         Int32.Parse(Request.QueryString["id"]),
//                         byPrimary: false,
//                         recursive: true));
//                 }
//                 catch (Exception e)
//                 {
//                     json["schedule"] = null;
//                 }
//             }
//
//             return Content(json.ToString(), "application/json");
//         }
//     }
// }