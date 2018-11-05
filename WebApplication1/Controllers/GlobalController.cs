using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using System.Data;
    using System.Diagnostics;
    using WebApplication1.Models;

    public class GlobalController : Controller
    {
        public Boolean CheckLogin(AccountType AccountType = AccountType.Any)
        {
            if (Session["user"] != null)
            {
                try
                {
                    if (AccountType != AccountType.Any && ((Account)Session["user"]).Type != AccountType)
                    {
                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    return false;
                }
            }

            return false;
        }

        public Boolean IsLoggedIn()
        {
            try
            {
                return ((Account) Session["user"]).Exists;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public Account GetAccount()
        {
            return Session["user"] != null ? (Account)Session["user"] : null;
        }

        public int GetNotificationNum()
        {
            if (GetAccount() != null)
            {
                DBHandler db = new DBHandler();
                Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();
                param.Add("@Status", (int)NotificationStatus.Unread);
                param.Add("@Account", this.GetAccount().AccountID);

                using (DataTable dt = db.Execute<DataTable>(CRUD.READ, "SELECT COUNT(*) AS Count FROM Notification WHERE Status = @Status AND Account = @Account", param)) { 
                    int count = Int32.Parse(dt.Rows[0]["Count"].ToString());

                    return count;
                }
            }

            return 0;
        }

        public Boolean HasValues(FormCollection form, string[] keys)
        {
            foreach(string key in keys)
                if (!form.AllKeys.Contains(key) && form.GetValue(key) == null || (form.GetValue(key) != null && String.IsNullOrEmpty(form.GetValue(key).AttemptedValue)))
                    return false;

            return true;
        }
    }
}