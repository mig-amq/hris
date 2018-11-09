using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calfurn.Models
{
    using System.Data;

    using WebApplication1.Models;

    public class ProfileRequest
    {
        private DBHandler DBHandler;

        public int ProfileRequestID { get; set; }
        public Profile CurrentProfile { get; set; }
        public Profile NewProfile { get; set; }

        public ProfileRequest(int ProfileRequest = -1, bool byPrimary = true)
        {
            this.DBHandler = new DBHandler();

            if (ProfileRequest != -1)
            {
                this.Find(ProfileRequest, true, true);
            }
        }

        public ProfileRequest Find(int ProfileRequest, bool byPrimary = true, bool recursive = true)
        {
            string sql = "SELECT * FROM ProfileRequest WHERE " + (byPrimary ? " ProfileRequestID " : " CurrentProfile ")
                                                               + " = " + ProfileRequest;

            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.ProfileRequestID = Int32.Parse(row["ProfileRequestID"].ToString());
                    if (recursive)
                    {
                        this.CurrentProfile = new Profile(Int32.Parse(row["CurrentProfile"].ToString()));
                        this.NewProfile = new Profile(Int32.Parse(row["NewProfile"].ToString()));
                    }
                }
                else
                {
                    throw new Exception("Cannot find Profile Request form for: #" + ProfileRequest);
                }
            }

            return this;
        }

        public ProfileRequest Create()
        {
            string sql = "INSERT INTO ProfileRequest(CurrentProfile, NewProfile) OUTPUT INSERTED.ProfileRequestID "
                         + "VALUES(@CurrentProfile, @NewProfile)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@CurrentProfile", this.CurrentProfile.ProfileID);
            param.Add("@NewProfile", this.NewProfile.ProfileID);

            this.ProfileRequestID = this.DBHandler.Execute<Int32>(CRUD.CREATE, sql, param);
            return this;
        }

        public ProfileRequest Update(bool recursive = true)
        {
            return this.Update(this.ProfileRequestID, true, recursive);
        }

        public ProfileRequest Update(int ProfileRequest, bool byPrimary = true, bool recursive = true)
        {
            string sql =
                "UPDATE ProfileRequest SET CurrentProfile = @CurrentProfile, NewProfile = @NewProfile OUTPUT INSERTED.ProfileRequestID WHERE ProfileRequestID = "
                + ProfileRequestID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@CurrentProfile", this.CurrentProfile.ProfileID);
            param.Add("@NewProfile", this.NewProfile.ProfileID);

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, sql, param);
            return this;
        }

        public ProfileRequest Delete()
        {
            this.DBHandler.Execute<Int32>(
                CRUD.DELETE,
                "DELETE FROM ProfileRequest WHERE ProfileRequestID = " + this.ProfileRequestID);
            return this;
        }
    }
}