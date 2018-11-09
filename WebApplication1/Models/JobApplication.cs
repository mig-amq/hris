using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Calfurn.Models
{
    using System.Data;

    using WebApplication1.Models;

    public enum JobApplicationStatus
    {
        Undecided = 1,
        Unscheduled = 2,
        Schedule = 3,
        Accepted = 4,
        Rejected = 5
    }

    public class JobApplication
    {
        private DBHandler DBHandler;
        public int JobApplicationID { get; set; }
        public Applicant Applicant { get; set; }
        public JobPosting JobPosting { get; set; }
        public Schedule Schedule { get; set; }
        public JobApplicationStatus Status { get; set; }

        public JobApplication(int TableID = -1, bool byPrimary = true)
        {
            this.DBHandler = new DBHandler();

            if (TableID != -1)
            {
                if (byPrimary)
                    this.JobApplicationID = TableID;

                this.Find(TableID, byPrimary, true);
            }
        }

        public JobApplication Find(int TableID, bool byPrimary = true, bool recursive = true)
        {
            string sql = "SELECT * FROM JobApplication WHERE " + (byPrimary ? " JobApplicationID " : " Applicant ")
                                                               + " = " + TableID;

            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    this.JobApplicationID = Int32.Parse(row["JobApplicationID"].ToString());
                    this.Status = (JobApplicationStatus) Int32.Parse(row["Status"].ToString());

                    if (recursive)
                    {
                        this.Applicant = new Applicant(Int32.Parse(row["Applicant"].ToString()), byPrimary: true);

                        this.JobPosting = new JobPosting(Int32.Parse(row["JobPosting"].ToString()));
                    }

                    this.Schedule = this.GetSchedule(this.JobApplicationID);
                }
                else
                {
                    throw new Exception("Cannot find Job Posting with ID: " + TableID);
                }
            }
            return this;
        }

        public JobApplication Create()
        {
            string sql = "INSERT INTO JobApplication(Applicant, JobPosting, Status) "
                         + "OUTPUT INSERTED.JobApplicationID VALUES(@Applicant, @JobPosting, @Status)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Applicant", this.Applicant.ApplicantID);
            param.Add("@JobPosting", this.JobPosting.PostingID);
            param.Add("@Status", (int) this.Status);

            this.JobApplicationID = this.DBHandler.Execute<Int32>(CRUD.CREATE, sql, param);
            return this;
        }

        public JobApplication Update(bool recursive = false)
        {
            return this.Update(this.JobApplicationID, recursive);
        }

        public JobApplication Update(int JobApplicationID, bool recursive = false)
        {
            string sql = "UPDATE JobApplication SET Applicant = @Applicant, JobPosting = @JobPosting, Status = @Status"
                + " OUTPUT INSERTED.JobApplicationID WHERE JobApplicationID = "
                + JobApplicationID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Applicant", this.Applicant.ApplicantID);
            param.Add("@JobPosting", this.JobPosting.PostingID);
            param.Add("@Status", (int)this.Status);

            this.JobApplicationID = this.DBHandler.Execute<Int32>(CRUD.UPDATE, sql, param);
            return this;
        }

        public JobApplication Delete()
        {
            this.DBHandler.Execute<Int32>(
                CRUD.DELETE,
                "DELETE FROM JobApplication WHERE JobApplicationID = " + this.JobApplicationID);
            return this;
        }

        public Schedule GetSchedule(int JobApplicationID)
        {
            Schedule n = null;

            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT ScheduleID FROM Schedule WHERE JobApplication = " + JobApplicationID))
            {
                if (dt.Rows.Count > 0)
                {
                    try
                    {
                        DataRow row = dt.Rows[0];
                        return new Schedule().Find(Int32.Parse(row["ScheduleID"].ToString()), true, false);
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            return n;
        }
    }
}