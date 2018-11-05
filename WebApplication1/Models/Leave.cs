using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;

    public enum LeaveStatus
    {
        Pending = 1,
        Approved = 2,
        Denied = 3
    }

    public class Leave
    {
        public static int MAX = 5;

        private DBHandler DBHandler;

        public int LeaveID { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public LeaveStatus Status { get; set; }

        public Leave(int LeaveID = -1)
        {
            this.DBHandler = new DBHandler();

            if (LeaveID != -1)
            {
                this.LeaveID = LeaveID;
                this.Find(this.LeaveID);
            }
        }

        public Leave Find(int LeaveID, bool recursive = true)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM Leave WHERE LeaveID = " + LeaveID))
            {
                DataRow row = dt.Rows[0];

                this.StartDate = DateTime.Parse(row["StartDate"].ToString());
                this.EndDate = DateTime.Parse(row["EndDate"].ToString());
                this.Reason = row["Reason"].ToString();
                this.LeaveID = LeaveID;
                this.Status = (LeaveStatus)Int32.Parse(row["Status"].ToString());

                if (recursive)
                {
                    this.Employee = (Employee) new Employee().FindProfile(Int32.Parse(row["Employee"].ToString()), byPrimary:true);
                }
            }

            return this;
        }

        public Leave Create()
        {
            string columns = "INSERT INTO Leave(Employee, StartDate, EndDate, Reason, Status) OUTPUT INSERTED.LeaveID ";
            string values = " Values(@Employee, @StartDate, @EndDate, @Reason, @LeaveStatus)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Employee", this.Employee.EmployeeID);
            param.Add("@StartDate", this.StartDate.ToString("yyyy-MM-dd"));
            param.Add("@EndDate", this.EndDate.ToString("yyyy-MM-dd"));
            param.Add("@Reason", this.Reason);
            param.Add("@LeaveStatus", (int) this.Status);
            this.LeaveID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public Leave Update(bool recursive = true)
        {
            return this.Update(this.LeaveID, recursive);
        }

        public Leave Update(int LeaveID, bool recursive = true)
        {
            string set = "UPDATE Leave SET Employee = @Employee, "
                         + "StartDate = @StartDate, EndDate = @EndDate, "
                         + "Reason = @Reason, Status = @LeaveStatus OUTPUT INSERTED.LeaveID WHERE LeaveID = " + LeaveID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Employee", this.Employee.EmployeeID);
            param.Add("@StartDate", this.StartDate.ToString("yyyy-MM-dd"));
            param.Add("@EndDate", this.EndDate.ToString("yyyy-MM-dd"));
            param.Add("@Reason", this.Reason);
            param.Add("@LeaveStatus", (int)this.Status);

            this.DBHandler.Execute<Int32>(CRUD.CREATE, set, param);
            return this;
        }

        public Leave Delete()
        {
            this.DBHandler.Execute<Int32>(CRUD.DELETE, "DELETE FROM Leave WHERE LeaveID = " + this.LeaveID);
            return this;
        }
    }
}