using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.Supers
{
    public abstract class ProfiledObject
    {
        protected DBHandler DBHandler;
        public Profile Profile;

        public ProfiledObject()
        {
            this.Profile = new Profile();
            this.DBHandler = new DBHandler();
        }

        public ProfiledObject(int AccountID) : this()
        {
            this.FindProfile(AccountID);
        }

        public abstract ProfiledObject FindProfile(int AccountID, bool recursive = false);
    }
}