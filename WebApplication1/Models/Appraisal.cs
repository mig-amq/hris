using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Collections;
    using System.Data;
    using System.Diagnostics;

    public enum AppraisalStatus
    {
        Pending = 1,
        Finished = 2
    }

    public enum AppraisalType
    {
        Supervisory = 1,
        NonSupervisory = 2
    }

    public class Appraisal
    {
        private DBHandler DBHandler;
        public int AppraisalID { get; set; }
        public DateTime CoveredPeriod { get; set; }
        public string Criteria { get; set; }
        public double Rating { get; set; }
        public double TechComp { get; set; }
        public double InterSkills { get; set; }
        public double CommComp { get; set; }
        public double Total { get; set; }
        public string Comments { get; set; }
        public Employee Evaluator { get; set; }
        public DateTime? DatePrepared { get; set; }
        public Employee NotedBy { get; set; }
        public DateTime? DateNoted { get; set; }
        public Employee DiscussedWith { get; set; }
        public DateTime? DateDiscussed { get; set; }
        public AppraisalType Type { get; set; }
        public AppraisalStatus Status { get; set; }

        public Appraisal(int TableID = -1, bool recursive = true, bool byDiscussed = false)
        {
            this.DBHandler = new DBHandler();
            if (TableID != -1)
            {
                this.Find(TableID, recursive, byDiscussed);
            }
        }

        public Appraisal Find(int TableID, bool recursive = true, bool byDiscussed = false)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ,
                "SELECT * FROM Appraisal WHERE " +
                (byDiscussed ? " DiscussedWith " : " AppraisalID ") + " = " + TableID))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.AppraisalID = Int32.Parse(row["AppraisalID"].ToString());
                    this.CoveredPeriod = DateTime.Parse(row["CoveredPeriod"].ToString());
                    this.Criteria = row["Criteria"].ToString();
                    this.Rating = Double.Parse(row["Rating"].ToString());
                    this.TechComp = Double.Parse(row["TechComp"].ToString());
                    this.InterSkills = Double.Parse(row["InterSkills"].ToString());
                    this.CommComp = Double.Parse(row["CommComp"].ToString());
                    this.Total = Double.Parse(row["Total"].ToString());
                    this.Comments = row["Comments"].ToString();

                    if (row["DatePrepared"] != DBNull.Value)
                        this.DatePrepared = DateTime.Parse(row["DatePrepared"].ToString());

                    if (row["DateNoted"] != DBNull.Value)
                        this.DateNoted = DateTime.Parse(row["DateNoted"].ToString());

                    if (row["DateDiscussed"] != DBNull.Value)
                        this.DateDiscussed = DateTime.Parse(row["DateDiscussed"].ToString());

                    this.Type = (AppraisalType)Int32.Parse(row["Type"].ToString());
                    this.Status = (AppraisalStatus)Int32.Parse(row["Status"].ToString());

                    if (recursive)
                    {
                        if (row["Evaluator"] != DBNull.Value)
                            this.Evaluator = (Employee)new Employee().FindProfile(Int32.Parse(row["Evaluator"].ToString()), byPrimary: true);

                        if (row["NotedBy"] != DBNull.Value)
                            this.NotedBy = (Employee)new Employee().FindProfile(Int32.Parse(row["NotedBy"].ToString()), byPrimary: true);

                        if (row["DiscussedWith"] != DBNull.Value)
                            this.DiscussedWith = (Employee)new Employee().FindProfile(Int32.Parse(row["DiscussedWith"].ToString()), byPrimary: true);
                    }
                }
                else
                {
                    throw new Exception("Error: Failed to find appraisal.");
                }
            }
            return this;
        }

        public ArrayList FindAll(int DiscussedWith, bool recursive)
        {
            ArrayList appraisals = new ArrayList();
            using (DataTable dt =
                this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT * FROM Appraisal WHERE DiscussedWith = " + DiscussedWith))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Appraisal ap = new Appraisal();

                    ap.AppraisalID = Int32.Parse(row["AppraisalID"].ToString());
                    ap.CoveredPeriod = DateTime.Parse(row["CoveredPeriod"].ToString());
                    ap.Criteria = row["Criteria"].ToString();
                    ap.Rating = Double.Parse(row["Rating"].ToString());
                    ap.TechComp = Double.Parse(row["TechComp"].ToString());
                    ap.InterSkills = Double.Parse(row["InterSkills"].ToString());
                    ap.CommComp = Double.Parse(row["CommComp"].ToString());
                    ap.Total = Double.Parse(row["Total"].ToString());
                    ap.Comments = row["Comments"].ToString();

                    if (row["DatePrepared"] != DBNull.Value)
                        ap.DatePrepared = DateTime.Parse(row["DatePrepared"].ToString());

                    if (row["DateNoted"] != DBNull.Value)
                        ap.DateNoted = DateTime.Parse(row["DateNoted"].ToString());

                    if (row["DateDiscussed"] != DBNull.Value)
                        ap.DateDiscussed = DateTime.Parse(row["DateDiscussed"].ToString());

                    ap.Type = (AppraisalType)Int32.Parse(row["Type"].ToString());
                    ap.Status = (AppraisalStatus)Int32.Parse(row["Status"].ToString());

                    if (recursive)
                    {
                        if (row["Evaluator"] != DBNull.Value)
                            ap.Evaluator = (Employee)new Employee().FindProfile(Int32.Parse(row["Evaluator"].ToString()), byPrimary: true);

                        if (row["NotedBy"] != DBNull.Value)
                            ap.NotedBy = (Employee)new Employee().FindProfile(Int32.Parse(row["NotedBy"].ToString()), byPrimary: true);

                        if (row["DiscussedWith"] != DBNull.Value)
                            ap.DiscussedWith = (Employee)new Employee().FindProfile(Int32.Parse(row["DiscussedWith"].ToString()), byPrimary: true);
                    }

                    appraisals.Add(ap);
                }
            }

            return appraisals;
        }

        public Appraisal Create()
        {
            string columns = "INSERT INTO Appraisal(CoveredPeriod, Criteria, "
                             + "Rating, TechComp, InterSkills, CommComp, Total, "
                             + "Comments, Evaluator, DatePrepared, NotedBy, DateNoted, "
                             + "DiscussedWith, DateDiscussed, Type, Status) OUTPUT INSERTED.AppraisalID ";
            string values = " VALUES(@CoveredPeriod, @Criteria, @Rating, @TechComp,"
                            + " @InterSkills, @CommComp, @Total, @Comments, @Evaluator, "
                            + "@DatePrepared, @NotedBy, @DateNoted, @DiscussedWith, @DateDiscussed, @Type, @Status)";

            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@CoveredPeriod", this.CoveredPeriod.ToString("yyyy-MM-dd"));
            param.Add("@Criteria", this.Criteria);
            param.Add("@Rating", this.Rating);
            param.Add("@TechComp", this.TechComp);
            param.Add("@InterSkills", this.InterSkills);
            param.Add("@CommComp", this.CommComp);
            param.Add("@Total", this.Total);
            param.Add("@Comments", this.Comments);

            if (this.Evaluator != null)
                param.Add("@Evaluator", this.Evaluator.EmployeeID);
            else
                param.Add("@Evaluator", DBNull.Value);

            if (this.DatePrepared != null)
                param.Add("@DatePrepared", this.DatePrepared.Value.ToString("yyyy-MM-dd"));
            else
                param.Add("@DatePrepared", DBNull.Value);

            if (this.NotedBy != null)
                param.Add("@NotedBy", this.NotedBy.EmployeeID);
            else
                param.Add("@NotedBy", DBNull.Value);

            if (this.DateNoted != null)
                param.Add("@DateNoted", this.DateNoted.Value.ToString("yyyy-MM-dd"));
            else
                param.Add("@DateNoted", DBNull.Value);

            if (this.DiscussedWith != null)
                param.Add("@DiscussedWith", this.DiscussedWith.EmployeeID);
            else
                param.Add("@DiscussedWith", DBNull.Value);

            if (this.DateDiscussed != null)
                param.Add("@DateDiscussed", this.DateDiscussed.Value.ToString("yyyy-MM-dd"));
            else
                param.Add("@DateDiscussed", DBNull.Value);

            param.Add("@Type", (int)this.Type);
            param.Add("@Status", (int)this.Status);

            this.AppraisalID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public Appraisal Update(bool recursive = true)
        {
            return this.Update(this.AppraisalID, recursive, false);
        }

        public Appraisal Update(int TableID, bool recursive = true, bool byDiscussed = false)
        {
            string set = "UPDATE Appraisal SET CoveredPeriod = @CoveredPeriod, "
                         + "Criteria = @Criteria, Rating = @Rating, "
                         + "TechComp = @TechComp, InterSkills = @InterSkills, "
                         + "CommComp = @CommComp, Total = @Total, "
                         + "Comments = @Comments, Evaluator = @Evaluator, "
                         + "DatePrepared = @DatePrepared, NotedBy = @NotedBy, "
                         + "DateNoted = @DateNoted, DiscussedWith = @DiscussedWith, "
                         + "DateDiscussed = @DateDiscussed, Type = @Type, Status = @Status OUTPUT INSERTED.AppraisalID WHERE " +
                         (byDiscussed ? " DiscussedWith " : " AppraisalID ") + " = " + TableID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@CoveredPeriod", this.CoveredPeriod.ToString("yyyy-MM-dd"));
            param.Add("@Criteria", this.Criteria);
            param.Add("@Rating", this.Rating);
            param.Add("@TechComp", this.TechComp);
            param.Add("@InterSkills", this.InterSkills);
            param.Add("@CommComp", this.CommComp);
            param.Add("@Total", this.Total);
            param.Add("@Comments", this.Comments);

            if (this.Evaluator != null)
                param.Add("@Evaluator", this.Evaluator.EmployeeID);
            else
                param.Add("@Evaluator", DBNull.Value);

            if (this.DatePrepared != null)
                param.Add("@DatePrepared", this.DatePrepared.Value.ToString("yyyy-MM-dd"));
            else
                param.Add("@DatePrepared", DBNull.Value);

            if (this.NotedBy != null)
                param.Add("@NotedBy", this.NotedBy.EmployeeID);
            else
                param.Add("@NotedBy", DBNull.Value);

            if (this.DateNoted != null)
                param.Add("@DateNoted", this.DateNoted.Value.ToString("yyyy-MM-dd"));
            else
                param.Add("@DateNoted", DBNull.Value);

            if (this.DiscussedWith != null)
                param.Add("@DiscussedWith", this.DiscussedWith.EmployeeID);
            else
                param.Add("@DiscussedWith", DBNull.Value);

            if (this.DateDiscussed != null)
                param.Add("@DateDiscussed", this.DateDiscussed.Value.ToString("yyyy-MM-dd"));
            else
                param.Add("@DateDiscussed", DBNull.Value);

            param.Add("@Type", (int)this.Type);
            param.Add("@Status", (int)this.Status);

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public Appraisal Delete()
        {
            this.DBHandler.Execute<Int32>(
                CRUD.DELETE,
                "DELETE FROM Apparaisal WHERE AppraisalID = " + this.AppraisalID);
            return this;
        }
    }
}