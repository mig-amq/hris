using System;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using System.Collections.Generic;

    using Newtonsoft.Json.Linq;

    using WebApplication1.Models;
    using WebApplication1.Models.Supers;

    public class UserAccountController : Controller
    {
        [HttpPost]
        public ActionResult LoginNoAjax(FormCollection form)
        {
            ViewData["Alert"] = LogIn(form);

            return View();
        }

        // POST: UserAccount
        [HttpPost]
        public ContentResult LogIn(FormCollection form)
        {
            /**
             * Initialize a JSON object that will be returned to the client.
             * error - dictates whether an error was encountered while logging in
             * message - the message that will tell the user what happened
             */
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string username = form.Get("username");
            string password = form.Get("password");

            Account a = new Account().FindByUsername(username); // look for the user in the DB

            if (a.Exists && password.Equals(a.Password) && !a.Locked) // check if the user exists in the DB
            {
                if (Session["user"] == null) // check if the user is not yet logged in
                {
                    Session["user"] = a;

                    json["message"] = "You logged in successfully! Please wait...";
                }
                else
                { // return an error if the user is trying to log in again
                    json["error"] = true;
                    json["message"] = "Looks like you're already logged in!";
                }
            }
            else
            { // return an error if the username and password combination is not in the DB
                if (Session["count"] != null)
                {
                    Session["count"] = Int32.Parse(Session["count"].ToString()) + 1;

                    if (Int32.Parse(Session["count"].ToString()) >= 3)
                    {
                        a.Locked = true;
                        a.Update();
                    }
                }
                else
                {
                    Session["count"] = 1;
                }

                json["error"] = true;
                json["message"] = "Oops! You entered an incorrect username or password";
            }

            // return the results of the login proccess
            return Content(json.ToString(), "application/json");
        }

        // GET: UserAccount/LogOut
        public ActionResult LogOut()
        {
            if (Session["user"] != null) // check if the user is logged in
            {
                Session.Clear(); // destroy current user session
                Session.Abandon();

                if (Request.Cookies.Get("userCookie") != null) // check if the user ticked Remember Me
                {
                    Response.Cookies.Remove("userCookie"); // delete related cookie
                    HttpCookie cookie = new HttpCookie("userCookie");
                    cookie.Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies.Add(cookie);
                }
            }

            return RedirectToAction("Index", "Home"); // redirect to home
        }

        // POST: UserAccount/Create
        [HttpPost]
        public ActionResult Create(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            Account creator = (Account)Session["user"];

            if (creator.Type == AccountType.DepartmentHead && ((Employee) creator.Profile).Department.Type == DepartmentType.HumanResources)
            {
                if ((AccountType)form.GetValue("type").ConvertTo(typeof(AccountType)) == AccountType.Applicant)
                {
                    Employee emp = new Employee();
                    emp.Status = StatusType.Active;
                    emp.Profile = new Profile();
                    emp.Profile.FirstName = form.GetValue("FirstName").ToString();
                    emp.Profile.MiddleName = form.GetValue("MiddleName").ToString();
                    emp.Profile.LastName = form.GetValue("LastName").ToString();

                    emp.Profile.HouseNo = form.GetValue("HouseNo").ToString();
                    emp.Profile.City = form.GetValue("City").ToString();
                    emp.Profile.Street = form.GetValue("Street").ToString();
                    emp.Profile.Province = form.GetValue("Province").ToString();
                    emp.Profile.Sex = (SexType)form.GetValue("Sex").ConvertTo(typeof(SexType));

                    emp.Profile.Contact = form.GetValue("Contact").ToString().Replace(" ", "");
                    emp.Profile.CPersonNo = form.GetValue("CPersonNo").ToString().Replace(" ", "");
                    emp.Profile.CPersonRel = form.GetValue("CPerson").ToString();
                    emp.Profile.ContactPerson = form.GetValue("ContactPerson").ToString();

                    emp.Profile.BirthDate = DateTime.Parse(form.GetValue("BirthDate").ToString());
                    emp.Profile.Education = new Education();
                }
                else
                {

                }
            }
            else
            {
                json["error"] = true;
                json["message"] = "Uh oh! You're not the HR head, you can't make an account :c";
            }

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        // GET: UserAccount/Forgot
        public ActionResult Forgot()
        {
            return View();
        }

        // POST: UserAccount/Forgot
        [HttpPost]
        public ActionResult Forgot(FormCollection form)
        {
            JObject json = new JObject();

            return Json(json, JsonRequestBehavior.AllowGet);
        }
    }
}