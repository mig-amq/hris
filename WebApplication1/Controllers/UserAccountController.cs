using System;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using System.Collections.Generic;
    using System.Globalization;
    using Newtonsoft.Json.Linq;

    using WebApplication1.Models;
    using WebApplication1.Models.Supers;

    public class UserAccountController : GlobalController
    {
        // GET: UserAccount/Forgot
        public ActionResult Forgot()
        {
            if (!this.IsLoggedIn())
                return this.RedirectToAction("Index", "Home");

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

            Account a = new Account().FindByUsername(username, true); // look for the user in the DB

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
            } else if (a.Locked)
            {
                json["error"] = true;
                json["message"] = "Uh Oh! You entered an incorrect password three times, your account has been locked, please contact the HR...";
            }
            else
            { // return an error if the username and password combination is not in the DB
                if (Session["count"] != null)
                {
                    Session["count"] = Int32.Parse(Session["count"].ToString()) + 1;

                    if (Int32.Parse(Session["count"].ToString()) >= 3)
                    {
                        if (a.Exists)
                        {
                            a.Locked = true;
                            a.Update();
                        }

                        Session["count"] = 0;
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

        [HttpPost]
        public ActionResult RequestQuestion(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            if (form.GetValue("username") != null)
            {
                Account ac = new Account().FindByUsername(form.GetValue("username").AttemptedValue, false);

                if (ac.Exists)
                {
                    json["message"] = ac.Security;
                }
                else
                {
                    json["error"] = true;
                    json["message"] = "No account with that username exists";
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
        public ActionResult RequestPassword(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            if (form.GetValue("username") != null && form.GetValue("answer") != null)
            {
                Account ac = new Account().FindByUsername(form.GetValue("username").AttemptedValue, false);

                if (ac.Exists && ac.Answer == form.GetValue("answer").AttemptedValue)
                {
                    json["message"] = ac.Password;
                }
                else
                {
                    json["error"] = true;
                    json["message"] = "Incorrect security answer";
                }
            }
            else
            {
                json["error"] = true;
                json["message"] = "Form is incomplete";
            }

            return Content(json.ToString(), "application/json");
        }

        // POST: UserAccount/Create
        [HttpPost]
        public ActionResult ApplicantCreate(FormCollection form)
        {
            JObject json = new JObject();
            json["error"] = false;
            json["message"] = "";

            string[] keys = new string[]
                                {
                                    "username", "password", "email", "question", "answer", "skills", "desired",
                                    "frstname", "lastname", "middlename", " sex", "status", "byear", "bmonth", "bday",
                                    "contact", "emergency-name", "emergency-number", "emergency-rel", "house", "city", "province",
                                    "street", "education", "history"
                                };
            if (!this.IsLoggedIn())
            {
                if (this.HasValues(form, keys))
                {
                    Account ac = new Account();
                    ac.Profile = new Applicant();

                    if (!ac.FindByUsername(form.GetValue("username").AttemptedValue).Exists)
                    {
                        ac.Username = form.GetValue("username").AttemptedValue;
                        ac.Password = form.GetValue("password").AttemptedValue;
                        ac.Email = form.GetValue("email").AttemptedValue;
                        ac.Type = AccountType.Applicant;
                        ac.Security = form.GetValue("question").AttemptedValue;
                        ac.Answer = form.GetValue("answer").AttemptedValue;

                        ((Applicant)ac.Profile).Skills = form.GetValue("skills").AttemptedValue;
                        ((Applicant)ac.Profile).DesiredPosition = form.GetValue("desired").AttemptedValue;
                        ((Applicant)ac.Profile).Status = ApplicantStatus.Undecided;
                        ((Applicant)ac.Profile).Profile = new Profile();
                        ((Applicant)ac.Profile).Profile.FirstName = form.GetValue("firstname").AttemptedValue;
                        ((Applicant)ac.Profile).Profile.MiddleName = form.GetValue("middlename").AttemptedValue;
                        ((Applicant)ac.Profile).Profile.LastName = form.GetValue("lastname").AttemptedValue;
                        ((Applicant)ac.Profile).Profile.Sex = (SexType)Int32.Parse(form.GetValue("sex").AttemptedValue);
                        ((Applicant)ac.Profile).Profile.CivilStatus = (CivilStatusType)Int32.Parse(form.GetValue("status").AttemptedValue);

                        ((Applicant)ac.Profile).Profile.BirthDate = DateTime.ParseExact(
                            form.GetValue("byear").AttemptedValue + "-"
                                                                  + (Int32.Parse(form.GetValue("bmonth").AttemptedValue) + 1).ToString("00") + "-"
                                                                  + Int32.Parse(form.GetValue("bday").AttemptedValue).ToString("00"), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                        ((Applicant)ac.Profile).Profile.Contact = form.GetValue("contact").AttemptedValue;
                        ((Applicant)ac.Profile).Profile.ContactPerson = form.GetValue("emergency-name").AttemptedValue;
                        ((Applicant)ac.Profile).Profile.CPersonNo = form.GetValue("emergency-number").AttemptedValue;
                        ((Applicant)ac.Profile).Profile.CPersonRel = form.GetValue("emergency-rel").AttemptedValue;
                        ((Applicant)ac.Profile).Profile.HouseNo = form.GetValue("house").AttemptedValue;
                        ((Applicant)ac.Profile).Profile.City = form.GetValue("city").AttemptedValue;
                        ((Applicant)ac.Profile).Profile.Province = form.GetValue("province").AttemptedValue;
                        ((Applicant)ac.Profile).Profile.Street = form.GetValue("street").AttemptedValue;

                        JObject education = JObject.Parse(form.GetValue("education").AttemptedValue);
                        JArray history = JArray.Parse(form.GetValue("history").AttemptedValue);

                        Education edu = new Education(instantiate: false);
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
                            }
                            else if (property.Name == "hs")
                            {
                                edu.HighSchool = new EducationLevel(EducationType.HighSchool);
                                edu.HighSchool.Name = property.Value["name"].ToString();
                                edu.HighSchool.Address = property.Value["address"].ToString();
                                edu.HighSchool.Start = property.Value["start"].ToString();
                                edu.HighSchool.End = property.Value["end"].ToString();
                                hasProperties = true;
                            }
                            else if (property.Name == "college")
                            {
                                edu.College = new EducationLevel(EducationType.College);
                                edu.College.Name = property.Value["name"].ToString();
                                edu.College.Address = property.Value["address"].ToString();
                                edu.College.Start = property.Value["start"].ToString();
                                edu.College.End = property.Value["end"].ToString();
                                hasProperties = true;
                            }
                            else if (property.Name == "post")
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
                            ((Applicant)ac.Profile).Profile.Education = edu;
                        else
                            ((Applicant)ac.Profile).Profile.Education = null;

                        ac.Create();
                        foreach (EmploymentHistory o in hist)
                        {
                            o.Create(((Applicant)ac.Profile).Profile.ProfileID);
                        }

                        json["message"] = "Successfully created your account...";
                    }
                    else
                    {
                        json["error"] = true;
                        json["message"] = "Username is already in use";
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
                json["message"] = "You're not allowed to register as an applicant";
            }

            return Content(json.ToString(), "application/json");
        }
        
    }
}