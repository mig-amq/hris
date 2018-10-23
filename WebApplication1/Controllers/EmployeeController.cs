using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using System.Data;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    using Newtonsoft.Json.Linq;

    using WebApplication1.Models;

    public class EmployeeController : Controller
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

            string sql = "SELECT T2.FullName, T2.ID "
                         + "FROM(SELECT CONCAT(FirstName, ' ', LastName) AS FullName, EmployeeID AS ID "
                         + "FROM Employee E INNER JOIN Profile P ON E.Profile = P.ProfileID) T2 " + constraints;

            foreach (DataRow row in pt.Get(sql))
            {
                Employees.Add((Employee)new Employee().FindProfile(Int32.Parse(row["ID"].ToString()), true, true));
            }

            json["pages"] = pt.Pages;
            json["total"] = pt.Total;
            json["employees"] = JArray.FromObject(Employees);
            return Content(json.ToString(), "application/json");
        }
    }
}