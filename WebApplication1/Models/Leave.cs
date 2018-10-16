using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;

    public class Leave
    {
        private DBHandler DBHandler;

        public int LeaveID { get; set; }
        public Employee Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
        public string Type { get; set; }

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
                this.Type = row["Type"].ToString();
                this.LeaveID = LeaveID;

                if (recursive)
                {
                    this.Employee = (Employee) new Employee().FindProfile(Int32.Parse(row["Employee"].ToString()), byPrimary:true);
                }
            }

            return this;
        }

        public Leave Create()
        {
            string columns = "INSERT INTO Leave(Employee, StartDate, EndDate, Reason, Type) OUPUT INSERTED.LeaveID ";
            string values = " Values(@Employee, @StartDate, @EndDate, @Reason, @Type)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Employee", this.Employee.EmployeeID);
            param.Add("@StartDate", this.StartDate.ToShortDateString());
            param.Add("@EndDate", this.EndDate.ToShortDateString());
            param.Add("@Reason", this.Reason);
            param.Add("@Type", this.Type);

            this.LeaveID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public Leave Update(bool recursive = true)
        {
            return this.Update(this.LeaveID, recursive);
        }

        public Leave Update(int LeaveID, bool recursive = true)
        {
            string set = "UPDATE Leave SET Employee = @Employee AND "
                         + "StartDate = @StartDate AND EndDate = @EndDate AND "
                         + "Reason = @Reason AND Type = @Type WHERE LeaveID = " + LeaveID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Employee", this.Employee.EmployeeID);
            param.Add("@StartDate", this.StartDate.ToShortDateString());
            param.Add("@EndDate", this.EndDate.ToShortDateString());
            param.Add("@Reason", this.Reason);
            param.Add("@Type", this.Type);

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