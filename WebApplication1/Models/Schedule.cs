using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calfurn.Models
{
    using System.Data;

    using Microsoft.Ajax.Utilities;

    using WebApplication1.Models;

    public class Schedule
    {
        private DBHandler DBHandler;

        public int ScheduleID { get; set; }
        public DateTime TimeStart { get; set; }
        public DateTime TimeEnd { get; set; }
        public Applicant Applicant { get; set; }
        public Employee HR { get; set; }

        public Schedule(int ScheduleID = -1, bool recursive = true)
        {
            this.DBHandler = new DBHandler();

            if (ScheduleID != -1)
            {
                this.ScheduleID = ScheduleID;
                this.Find(ScheduleID, recursive);
            }
        }

        public Schedule Find(int ScheduleID, bool byPrimary = true, bool recursive = true)
        {
            string sql = "SELECT * FROM Schedule WHERE " + (byPrimary ? " ScheduleID " : " Applicant ") + " = "
                         + ScheduleID;

            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.ScheduleID = Int32.Parse(row["ScheduleID"].ToString());
                    this.TimeEnd = DateTime.Parse(row["TimeEnd"].ToString());
                    this.TimeStart = DateTime.Parse(row["TimeStart"].ToString());

                    if (recursive)
                    {
                        this.Applicant = (Applicant)new Applicant().FindProfile(
                            Int32.Parse(row["Applicant"].ToString()),
                            recursive: recursive,
                            byPrimary: true);

                        this.HR = new Employee(
                            Int32.Parse(row["HR"].ToString()),
                            byPrimary: true);
                    }
                }
                else
                {
                    throw new Exception("Cannot find Schedule with" + (byPrimary ? "" : "Applicant") + " ID: " + ScheduleID);
                }
            }

            return this;
        }

        public Schedule Create()
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT ScheduleID FROM Schedule WHERE YEAR(TimeStart) = " + this.TimeStart.Year
                                                                           + " AND MONTH(TimeStart) = "
                                                                           + this.TimeStart.Month
                                                                           + " AND DAY(TimeStart) = "
                                                                           + this.TimeStart.Day))
            {
                foreach (DataRow row in dt.Rows)
                {
                    DateTime Start = DateTime.Parse(row["TimeStart"].ToString());
                    DateTime End = DateTime.Parse(row["TimeEnd"].ToString());

                    if (!this.IsValid(Start, End))
                    {
                        throw new Exception("Time slot is already taken");
                    }
                }
            }

            string sql = "INSERT INTO Schedule(TimeStart, TimeEnd, Applicant, HR) OUTPUT INSERTED.ScheduleID "
                         + "VALUES(@TimeStart, @TimeEnd, @Applicant, @HR)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@TimeStart", this.TimeStart.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            param.Add("@TimeEnd", this.TimeEnd.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            param.Add("@Applicant", this.Applicant.ApplicantID);
            param.Add("@HR", this.HR.EmployeeID);

            this.ScheduleID = this.DBHandler.Execute<Int32>(CRUD.CREATE, sql, param);
            return this;
        }

        public Schedule Update(bool recursive = true)
        {
            return this.Update(this.ScheduleID, recursive:recursive);
        }

        public Schedule Update(int ScheduleID, bool byPrimary = true, bool recursive = true)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM Schedule WHERE YEAR(TimeStart) = " + this.TimeStart.Year
                                                                           + " AND MONTH(TimeStart) = "
                                                                           + this.TimeStart.Month
                                                                           + " AND DAY(TimeStart) = "
                                                                           + this.TimeStart.Day))
            {
                foreach (DataRow row in dt.Rows)
                {
                    DateTime Start = DateTime.Parse(row["TimeStart"].ToString());
                    DateTime End = DateTime.Parse(row["TimeEnd"].ToString());

                    if (!this.IsValid(Start, End))
                    {
                        throw new Exception("Time slot is already taken");
                    }
                }
            }

            string sql = "UPDATE Schedule SET TimeStart = @TimeStart, TimeEnd = @TimeEnd, "
                         + "Applicant = @Applicant, HR = @HR OUTPUT INSERTED.ScheduleID";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@TimeStart", this.TimeStart.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            param.Add("@TimeEnd", this.TimeEnd.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            param.Add("@Applicant", this.Applicant.ApplicantID);
            param.Add("@HR", this.HR.EmployeeID);

            this.ScheduleID = this.DBHandler.Execute<Int32>(CRUD.UPDATE, sql, param);
            return this;
        }

        public Schedule Delete()
        {
            this.DBHandler.Execute<Int32>(CRUD.DELETE, "DELETE FROM Schedule WHERE ScheduleID = " + this.ScheduleID);
            return this;
        }

        public Boolean IsValid(DateTime Start, DateTime End)
        {
            return (this.TimeEnd.CompareTo(Start) <= 0 && this.TimeStart.CompareTo(Start) <= 0)
                   || (this.TimeStart.CompareTo(End) >= 0 && this.TimeStart.CompareTo(Start) >= 0);
        }
    }
}