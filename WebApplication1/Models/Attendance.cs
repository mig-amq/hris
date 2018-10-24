using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;

    public class Attendance
    {
        private DBHandler DBHandler;

        public Employee Employee { get; set; }
        public int AttendanceID { get; set; }
        public int TotalWorkingDays { get; set; }
        public int Present { get; set; }
        public int Absent { get; set; }
        public int Late { get; set; }
        public int Overtime { get; set; }
        public int Undertime { get; set; }

        public Attendance(int AttendanceID = -1)
        {
            this.DBHandler = new DBHandler();

            if (AttendanceID != -1)
            {
                this.AttendanceID = AttendanceID;
                this.Find(AttendanceID, byPrimary:true);
            }
        }

        public Attendance Find(int TableID, bool recursive = true, bool byPrimary = false)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM Attendance WHERE " + 
                (byPrimary ? " AttendanceID " : " Employee ") + " = " + TableID))
            {
                DataRow row = dt.Rows[0];

                this.TotalWorkingDays = Int32.Parse(row["TotalWorkingDays"].ToString());
                this.Present = Int32.Parse(row["Present"].ToString());
                this.Absent = Int32.Parse(row["Absent"].ToString());
                this.Late = Int32.Parse(row["Late"].ToString());
                this.Overtime = Int32.Parse(row["Overtime"].ToString());
                this.Undertime = Int32.Parse(row["Undertime"].ToString());
                this.AttendanceID = Int32.Parse(row["AttendanceID"].ToString());

                if (recursive)
                {
                    this.Employee = (Employee)new Employee().FindProfile(
                        Int32.Parse(row["Employee"].ToString()),
                        byPrimary: true);
                }
            }
            return this;
        }

        public Attendance Create()
        {
            string columns =
                "INSERT INTO Attendance(Employee, TotalWorkingDays, Present, Absent, Overtime, Late, Undertime) OUTPUT INSERTED.AttendanceID ";
            string values = " VALUES(@Employee, @TotalWorkingDays, @Present, @Absent, @Overtime, @Late, @Undertime)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Employee", this.Employee.EmployeeID);
            param.Add("@TotalWorkingDays", this.TotalWorkingDays);
            param.Add("@Present", this.Present);
            param.Add("@Absent", this.Absent);
            param.Add("@Overtime", this.Overtime);
            param.Add("@Late", this.Late);
            param.Add("@Undertime", this.Undertime);

            this.AttendanceID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public Attendance Update(bool recursive = true)
        {
            return this.Update(this.AttendanceID, recursive);
        }

        public Attendance Update(int AttendanceID, bool recursive = true)
        {
            string set = "UPDATE Attendance SET Employee = @Employee AND "
                         + "TotalWorkingDays = @TotalWorkingDays AND "
                         + "Present = @Present AND Absent = @Absent AND "
                         + "Overtime = @Overtime AND Late = @Late AND Undertime = @Undertime WHERE AttendanceID = " + this.AttendanceID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Employee", this.Employee.EmployeeID);
            param.Add("@TotalWorkingDays", this.TotalWorkingDays);
            param.Add("@Present", this.Present);
            param.Add("@Absent", this.Absent);
            param.Add("@Overtime", this.Overtime);
            param.Add("@Late", this.Late);
            param.Add("@Undertime", this.Undertime);

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public Attendance Delete()
        {
            this.DBHandler.Execute<Int32>(
                CRUD.DELETE,
                "DELETE FROM Attendance WHERE AttendanceID = " + this.AttendanceID);
            return this;
        }
    }
}