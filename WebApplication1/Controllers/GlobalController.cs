using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    using WebApplication1.Models;

    public class GlobalController : Controller
    {
        public Boolean CheckLogin(AccountType AccountType = AccountType.Any)
        {
            if (Session["user"] != null)
            {
                if (AccountType != AccountType.Any && ((Account)Session["user"]).Type != AccountType)
                {
                    return false;
                }

                return true;
            }

            return false;
        }

        public Account GetAccount()
        {
            return Session["user"] != null ? (Account)Session["user"] : null;
        }
    }
}