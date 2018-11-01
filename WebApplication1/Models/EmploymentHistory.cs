using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Collections;
    using System.Data;
    using System.Runtime.InteropServices;

    public class EmploymentHistory
    {
        private DBHandler DBHandler;
        public int EmploymentHistoryID { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LeavingReason { get; set; }
        public string ContactName { get; set; }
        public string ContactNo { get; set; }

        public EmploymentHistory(int EmploymentHistoryID = -1)
        {
            this.DBHandler = new DBHandler();

            if (EmploymentHistoryID != -1)
            {
                this.EmploymentHistoryID = EmploymentHistoryID;
                this.Find(this.EmploymentHistoryID);
            }
        }

        public EmploymentHistory Find(int EmploymentHistoryID)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM EmploymentHistory WHERE EmploymentHistoryID = " + EmploymentHistoryID))
            {
                DataRow row = dt.Rows[0];

                this.CompanyName = row["CompanyName"].ToString();
                this.Address = row["Address"].ToString();
                this.Position = row["Position"].ToString();
                this.StartDate = row["StartDate"].ToString();
                this.EndDate = row["EndDate"].ToString();
                this.LeavingReason = row["LeavingReason"].ToString();
                this.ContactName = row["ContactName"].ToString();
                this.ContactNo = row["ContactNo"].ToString();
                this.EmploymentHistoryID = Int32.Parse(row["EmploymentHistoryID"].ToString());
            }
            return this;
        }

        public ArrayList FindAll(int ProfileID)
        {
            ArrayList hist = new ArrayList();
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM EmploymentHistory WHERE Profile = " + ProfileID))
            {
                foreach (DataRow row in dt.Rows)
                {
                    EmploymentHistory emp = new EmploymentHistory();
                    emp.CompanyName = row["CompanyName"].ToString();
                    emp.Address = row["Address"].ToString();
                    emp.Position = row["Position"].ToString();
                    emp.StartDate = row["StartDate"].ToString();
                    emp.EndDate = row["EndDate"].ToString();
                    emp.LeavingReason = row["LeavingReason"].ToString();
                    emp.ContactName = row["ContactName"].ToString();
                    emp.ContactNo = row["ContactNo"].ToString();
                    emp.EmploymentHistoryID = Int32.Parse(row["EmploymentHistoryID"].ToString());

                    hist.Add(emp);
                }
            }

            return hist;
        }
        public EmploymentHistory Create(int ProfileID)
        {
            string columns = "INSERT INTO EmploymentHistory(Profile, CompanyName, "
                             + "Address, Position, StartDate, EndDate, LeavingReason, "
                             + "ContactName, ContactNo) OUTPUT INSERTED.EmploymentHistoryID";
            string values = " VALUES(@Profile, @CompanyName, @Address, @Position, "
                            + "@StartDate, @EndDate, @LeavingReason, @ContactName, @ContactNo)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Profile", ProfileID);
            param.Add("@CompanyName", this.CompanyName);
            param.Add("@Address", this.Address);
            param.Add("@Position", this.Position);
            param.Add("@StartDate", this.StartDate);
            param.Add("@EndDate", this.EndDate);
            param.Add("@ContactName", this.ContactName);
            param.Add("@ContactNo", this.ContactNo);
            param.Add("@LeavingReason", this.LeavingReason);

            this.EmploymentHistoryID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public EmploymentHistory Update(bool recursive = true)
        {
            return this.Update(this.EmploymentHistoryID, recursive);
        }

        public EmploymentHistory Update(int EmploymentHistoryID, bool recursive = true, bool byProfile = false)
        {
            string set = "UPDATE EmploymentHistory SET CompanyName = @CompanyName AND "
                         + "Address = @Address AND Position = @Position AND "
                         + "StartDate = @StartDate AND EndDate = @EndDate AND "
                         + "LeavingReason = @LeavingReason AND ContactName = @ContactName AND "
                         + "ContactNo = @ContactNo WHERE "
                         + (byProfile ? "Profile" : "EmploymentHistoryID") + "=" + EmploymentHistoryID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@CompanyName", this.CompanyName);
            param.Add("@Address", this.Address);
            param.Add("@Position", this.Position);
            param.Add("@StartDate", this.StartDate);
            param.Add("@EndDate", this.EndDate);
            param.Add("@LeavingReason", this.LeavingReason);
            param.Add("@ContactName", this.ContactName);
            param.Add("@ContactNo", this.ContactNo);

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public EmploymentHistory Delete()
        {
            this.DBHandler.Execute<Int32>(
                CRUD.DELETE,
                "DELETE FROM EmploymentHistory WHERE EmploymentHistoryID = " + this.EmploymentHistoryID);
            return this;
        }
    }
}