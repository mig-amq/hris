using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;
    using System.Diagnostics;

    public class AttendanceTime
    {
        private DBHandler DBHandler { get; set; }
        public int AttendanceTimeID { get; set; }
        public DateTime? TimeIn { get; set; }
        public DateTime? TimeOut { get; set; }
        public DateTime Date { get; set; }
        public Attendance Attendance { get; set; }

        public AttendanceTime(int AttendanceTimeID = -1, bool recursive = true, bool byPrimary = true)
        {
            this.DBHandler = new DBHandler();

            if (AttendanceTimeID != -1)
            {
                this.Find(AttendanceTimeID, DateTime.Now, byPrimary: byPrimary, recursive: recursive);
            }
        }

        public AttendanceTime Find(int TableID, DateTime? Date, bool recursive = true, bool byPrimary = false)
        {
            string where = "";

            if (Date.HasValue)
            {
                where = " AND MONTH(Date) = " + Date.Value.Month + " AND YEAR(Date) = " + Date.Value.Year + " AND DAY(Date) = " + Date.Value.Date;
            }

            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM AttendanceTime WHERE " + (byPrimary ? " AttendanceTimeID " : " Attendance ") + " = "
                + TableID + where))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    try
                    {
                        this.TimeIn = DateTime.Parse(row["TimeIn"].ToString());
                    }
                    catch (Exception e)
                    {}

                    try
                    {
                        this.TimeOut = DateTime.Parse(row["TimeOut"].ToString());
                    }
                    catch (Exception e)
                    {}

                    this.Date = DateTime.Parse(row["Date"].ToString());
                    this.AttendanceTimeID = Int32.Parse(row["AttendanceTimeID"].ToString());

                    if (recursive)
                    {
                        this.Attendance = new Attendance().Find(Int32.Parse(row["Attendance"].ToString()), Date, byPrimary: true);
                    }
                }
                else
                {
                    throw new Exception("Cannot find attendance time for: " + ((Date.HasValue)
                                                                                   ? Date.Value.ToString("yyyy MMMM dd")
                                                                                   : DateTime.Now.ToString(
                                                                                       "yyyy MMMM dd")));
                }
            }
            return this;
        }

        public AttendanceTime Create()
        {
            string columns = "INSERT INTO AttendanceTime(TimeIn, TimeOut, Date, Attendance) OUTPUT INSERTED.AttendanceTimeID ";
            string values = " VALUES(@TimeIn, @TimeOut, @Date, @Attendance)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            if (this.TimeIn.HasValue)
            {
                param.Add("@TimeIn", this.TimeIn.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                param.Add("@TimeIn", DBNull.Value);
            }

            if (this.TimeOut.HasValue)
            {
                param.Add("@TimeOut", this.TimeOut.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                param.Add("@TimeOut", DBNull.Value);
            }

            param.Add("@Date", this.Date.ToString("yyyy-MM-dd"));
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
                "UPDATE AttendanceTime SET TimeIn = @TimeIn, TimeOut = @TimeOut, "
                + "Date = @Date, Attendance = @Attendance OUTPUT INSERTED.AttendanceTimeID WHERE "
                + (byPrimary ? " AttendanceTimeID " : " Attendance ") + " = " + TableID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            if (this.TimeIn.HasValue)
            {
                param.Add("@TimeIn", this.TimeIn.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                param.Add("@TimeIn", DBNull.Value);
            }

            if (this.TimeOut.HasValue)
            {
                param.Add("@TimeOut", this.TimeOut.Value.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
            else
            {
                param.Add("@TimeOut", DBNull.Value);
            }

            param.Add("@Date", this.Date.ToString("yyyy-MM-dd"));
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