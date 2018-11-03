﻿using System;
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

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using WebApplication1.Models;

    public class EmployeeController : GlobalController
    {
        private DBHandler DBHandler = new DBHandler();

        // GET: Employee
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult List()
        {

            return View();
        }

        [HttpGet]
        public ContentResult GetEmployee()
        {
            JObject json = new JObject();
            json["employee"] = JObject.FromObject(new Employee().FindProfile(Int32.Parse(Request.QueryString["id"]), true, true));

            return Content(json.ToString(), "application/json");
        }

        [HttpGet]
        public ContentResult GetEmployees()
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
                         + "FROM(SELECT CONCAT(P.FirstName, ' ', P.LastName, ' ', P.MiddleName) AS FullName, EmployeeID AS ID "
                         + "FROM Employee E INNER JOIN Profile P ON E.Profile = P.ProfileID) T2 " + constraints;

            Debug.WriteLine(sql);
            foreach (DataRow row in pt.Get(sql))
            {
                Employees.Add((Employee)new Employee().FindProfile(Int32.Parse(row["ID"].ToString()), true, true));
            }
            

            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["employees"] = JArray.FromObject(Employees);
            return Content(json.ToString(), "application/json");
        }

        [HttpPost]
        public ContentResult Update(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            Employee emp = (Employee)new Employee().FindProfile(Int32.Parse(form.GetValue("ID").AttemptedValue), true, true);

            emp.Status = (StatusType)Int32.Parse(form.GetValue("status").AttemptedValue);
            emp.Position = form.GetValue("position").AttemptedValue;
            emp.Department = new Department(Int32.Parse(form.GetValue("department").AttemptedValue), byPrimary:false);
            emp.Code = form.GetValue("code").AttemptedValue;
            emp.Profile.FirstName = form.GetValue("first-name").AttemptedValue;
            emp.Profile.MiddleName = form.GetValue("middle-name").AttemptedValue;
            emp.Profile.LastName = form.GetValue("last-name").AttemptedValue;
            emp.EmploymentDate = DateTime.ParseExact(
                form.GetValue("year").AttemptedValue + "-"
                                                           + (Int32.Parse(form.GetValue("month").AttemptedValue) + 1).ToString("00") + "-"
                                                           + Int32.Parse(form.GetValue("day").AttemptedValue).ToString("00"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            emp.Position = form.GetValue("position").AttemptedValue;

            if (emp.Status == StatusType.Inactive)
            {
                emp.DateInactive = DateTime.Now;
            }

            emp.Update();
            return Content(json.ToString(), "application/json");
        }
        
        public ActionResult Add()
        {
            return this.View();
        }

        [HttpPost]
        public ContentResult Create(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            Account ac = new Account();
            ac.Profile = new Employee();

            ac.Username = form.GetValue("username").AttemptedValue;
            ac.Password = form.GetValue("password").AttemptedValue;
            ac.Email = form.GetValue("email").AttemptedValue;
            ac.Type = (AccountType)Int32.Parse(form.GetValue("type").AttemptedValue);

            ((Employee)ac.Profile).Code = form.GetValue("code").AttemptedValue;
            ((Employee)ac.Profile).Position = form.GetValue("position").AttemptedValue;
            ((Employee)ac.Profile).EmploymentDate = DateTime.ParseExact(
                form.GetValue("hyear").AttemptedValue + "-"
                                                     + (Int32.Parse(form.GetValue("hmonth").AttemptedValue) + 1).ToString("00") + "-"
                                                     + Int32.Parse(form.GetValue("hday").AttemptedValue).ToString("00"), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            ((Employee)ac.Profile).Status = StatusType.Active;

            ((Employee)ac.Profile).Profile = new Profile();
            ((Employee)ac.Profile).Profile.FirstName = form.GetValue("firstname").AttemptedValue;
            ((Employee)ac.Profile).Profile.MiddleName = form.GetValue("middlename").AttemptedValue;
            ((Employee)ac.Profile).Profile.LastName = form.GetValue("lastname").AttemptedValue;
            ((Employee)ac.Profile).Profile.Sex = (SexType)Int32.Parse(form.GetValue("sex").AttemptedValue);
            ((Employee)ac.Profile).Profile.CivilStatus = (CivilStatusType)Int32.Parse(form.GetValue("status").AttemptedValue);

            ((Employee)ac.Profile).Profile.BirthDate = DateTime.ParseExact(
                form.GetValue("byear").AttemptedValue + "-"
                                                      + (Int32.Parse(form.GetValue("bmonth").AttemptedValue) + 1).ToString("00") + "-"
                                                      + Int32.Parse(form.GetValue("bday").AttemptedValue).ToString("00"), "yyyy-MM-dd", CultureInfo.InvariantCulture);

            ((Employee)ac.Profile).Profile.Contact = form.GetValue("contact").AttemptedValue;
            ((Employee)ac.Profile).Profile.ContactPerson = form.GetValue("emergency-name").AttemptedValue;
            ((Employee)ac.Profile).Profile.CPersonNo = form.GetValue("emergency-number").AttemptedValue;
            ((Employee)ac.Profile).Profile.CPersonRel = form.GetValue("emergency-rel").AttemptedValue;
            ((Employee)ac.Profile).Profile.HouseNo = form.GetValue("house").AttemptedValue;
            ((Employee)ac.Profile).Profile.City = form.GetValue("city").AttemptedValue;
            ((Employee)ac.Profile).Profile.Province = form.GetValue("province").AttemptedValue;
            ((Employee)ac.Profile).Profile.Street = form.GetValue("street").AttemptedValue;


            Debug.WriteLine(form.GetValue("history").AttemptedValue);
            JObject education = JObject.Parse(form.GetValue("education").AttemptedValue);
            JArray history = JArray.Parse(form.GetValue("history").AttemptedValue);

            Education edu = new Education(instantiate:false);
            bool hasProperties = false;

            foreach (JProperty property in education.Properties())
            {
                if (property.Name == "elementary")
                {
                    edu.Elementary = new EducationLevel(EducationType.Elementary);
                    edu.Elementary.Name = property.Value["name"].ToString();
                    edu.Elementary.Address = property.Value["address"].ToString();
                    edu.Elementary.Start = property.Value["start"].ToString();
                    edu.Elementary.End = property.Value["end"].ToString();
                    hasProperties = true;
                } else if (property.Name == "hs")
                {
                    edu.HighSchool = new EducationLevel(EducationType.HighSchool);
                    edu.HighSchool.Name = property.Value["name"].ToString();
                    edu.HighSchool.Address = property.Value["address"].ToString();
                    edu.HighSchool.Start = property.Value["start"].ToString();
                    edu.HighSchool.End = property.Value["end"].ToString();
                    hasProperties = true;
                } else if (property.Name == "college")
                {
                    edu.College = new EducationLevel(EducationType.College);
                    edu.College.Name = property.Value["name"].ToString();
                    edu.College.Address = property.Value["address"].ToString();
                    edu.College.Start = property.Value["start"].ToString();
                    edu.College.End = property.Value["end"].ToString();
                    hasProperties = true;
                } else if (property.Name == "post")
                {
                    edu.PostGraduate = new EducationLevel(EducationType.PostGraduate);
                    edu.PostGraduate.Name = property.Value["name"].ToString();
                    edu.PostGraduate.Address = property.Value["address"].ToString();
                    edu.PostGraduate.Start = property.Value["start"].ToString();
                    edu.PostGraduate.End = property.Value["end"].ToString();
                    hasProperties = true;
                }
            }

            List<EmploymentHistory> hist = new List<EmploymentHistory>();

            foreach (JObject o in history)
            {
                EmploymentHistory temp = new EmploymentHistory();
                temp.Address = o.GetValue("address").ToString();
                temp.CompanyName = o.GetValue("company").ToString();
                temp.Position = o.GetValue("position").ToString();
                temp.ContactName = o.GetValue("cperson").ToString();
                temp.ContactNo = o.GetValue("number").ToString();
                temp.StartDate = o.GetValue("start").ToString();
                temp.EndDate = o.GetValue("end").ToString();
                temp.LeavingReason = o.GetValue("leave").ToString();
                hist.Add(temp);
            }
            
            if (hasProperties == true)
                ((Employee)ac.Profile).Profile.Education = edu;
            else
                ((Employee)ac.Profile).Profile.Education = null;

            ac.Create(Int32.Parse(form.GetValue("department").AttemptedValue));
            foreach (EmploymentHistory o in hist)
            {
                o.Create(((Employee)ac.Profile).Profile.ProfileID);
            }
            return Content(json.ToString(), "application/json");
        }
    }
}