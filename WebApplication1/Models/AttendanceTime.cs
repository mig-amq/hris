using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;

    public class AttendanceTime
    {
        private DBHandler DBHandler { get; set; }
        public int AttendanceTimeID { get; set; }
        public DateTime TimeIn { get; set; }
        public DateTime TimeOut { get; set; }
        public DateTime Date { get; set; }
        public Attendance Attendance { get; set; }

        public AttendanceTime(int AttendanceTimeID = -1)
        {
            this.DBHandler = new DBHandler();

            if (AttendanceTimeID != -1)
            {
                this.Find(AttendanceTimeID, byPrimary: true);
            }
        }

        public AttendanceTime Find(int TableID, bool recursive = true, bool byPrimary = false)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM AttendanceTime WHERE " + (byPrimary ? " AttendanceTimeID " : " Attendance ") + " = "
                + TableID))
            {
                DataRow row = dt.Rows[0];

                this.TimeIn = DateTime.Parse(row["TimeIn"].ToString());
                this.TimeOut = DateTime.Parse(row["TimeOut"].ToString());
                this.Date = DateTime.Parse(row["Date"].ToString());
                this.AttendanceTimeID = Int32.Parse(row["AttendanceTimeID"].ToString());

                if (recursive)
                {
                    this.Attendance = new Attendance().Find(Int32.Parse(row["Attendance"].ToString()), byPrimary:true);
                }
            }
            return this;
        }

        public AttendanceTime Create()
        {
            string columns = "INSERT INTO AttendanceTime(TimeIn, TimeOut, Date, Attendance) OUTPUT INSERTED.AttendanceTimeID ";
            string values = " VALUES(@TimeIn, @TimeOut, @Date, @Attendance)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@TimeIn", this.TimeIn.ToShortTimeString());
            param.Add("@TimeOut", this.TimeOut.ToShortTimeString());
            param.Add("@Date", this.Date.ToShortDateString());
            param.Add("@Attendance", this.Attendance.AttendanceID);

            this.AttendanceTimeID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public AttendanceTime Update(bool recursive = true)
        {
            return this.Update(this.AttendanceTimeID, recursive, byPrimary: true);
        }

        public AttendanceTime Update(int TableID, bool recursive = true, bool byPrimary = false)
        {
            string set =
                "UPDATE AttendanceTime SET TimeIn = @TimeIn AND TimeOut = @TimeOut AND "
                + "Date = @Date AND Attendance = @Attendance WHERE "
                + (byPrimary ? " AttendanceTimeID " : " Attendance ") + " = " + TableID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@TimeIn", this.TimeIn.ToShortTimeString());
            param.Add("@TimeOut", this.TimeOut.ToShortTimeString());
            param.Add("@Date", this.Date.ToShortDateString());
            param.Add("@Attendance", this.Attendance.AttendanceID);

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public AttendanceTime Delete()
        {
            this.DBHandler.Execute<Int32>(
                CRUD.DELETE,
                "DELETE FROM AttendanceTime WHERE AttendanceTimeID = " + AttendanceTimeID);
            return this;
        }
    }
}