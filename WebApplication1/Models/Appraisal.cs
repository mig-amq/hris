using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Collections;
    using System.Data;

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
        public string Critera { get; set; }
        public int Rating { get; set; }
        public int TechComp { get; set; }
        public int InterSkills { get; set; }
        public int CommComp { get; set; }
        public int Total { get; set; }
        public string Comments { get; set; }
        public Employee Evaluator { get; set; }
        public DateTime DatePrepared { get; set; }
        public Employee NotedBy { get; set; }
        public DateTime DateNoted { get; set; }
        public Employee DiscussedWith { get; set; }
        public DateTime DateDiscussed { get; set; }
        public AppraisalType Type { get; set; }

        public Appraisal(int TableID = -1, bool byDiscussed = false)
        {
            this.DBHandler = new DBHandler();
            if (TableID != -1)
            {
                this.Find(TableID, byDiscussed);
            }
        }

        public Appraisal Find(int TableID, bool recursive = true, bool byDiscussed = false)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, 
                "SELECT * FROM Appraisal WHERE " + 
                (byDiscussed ? " DiscussedWith " : " AppraisalID ") + " = " + TableID))
            {
                DataRow row = dt.Rows[0];
                this.AppraisalID = Int32.Parse(row["AppraisalID"].ToString());
                this.CoveredPeriod = DateTime.Parse(row["CoveredPeriod"].ToString());
                this.Critera = row["Criteria"].ToString();
                this.Rating = Int32.Parse(row["Rating"].ToString());
                this.TechComp = Int32.Parse(row["TechComp"].ToString());
                this.InterSkills = Int32.Parse(row["InterSkills"].ToString());
                this.CommComp = Int32.Parse(row["CommComp"].ToString());
                this.Total = Int32.Parse(row["Total"].ToString());
                this.Comments = row["Comments"].ToString();
                this.DatePrepared = DateTime.Parse(row["DatePrepared"].ToString());
                this.DateNoted = DateTime.Parse(row["DateNoted"].ToString());
                this.DateDiscussed = DateTime.Parse(row["DateDiscussed"].ToString());
                this.Type = (AppraisalType)Int32.Parse(row["Type"].ToString());

                if (recursive)
                {
                    this.Evaluator = new Employee(Int32.Parse(row["Evalutator"].ToString()));
                    this.NotedBy = new Employee(Int32.Parse(row["NotedBy"].ToString()));
                    this.DiscussedWith = new Employee(Int32.Parse(row["DiscussedWith"].ToString()));
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
                    ap.Critera = row["Criteria"].ToString();
                    ap.Rating = Int32.Parse(row["Rating"].ToString());
                    ap.TechComp = Int32.Parse(row["TechComp"].ToString());
                    ap.InterSkills = Int32.Parse(row["InterSkills"].ToString());
                    ap.CommComp = Int32.Parse(row["CommComp"].ToString());
                    ap.Total = Int32.Parse(row["Total"].ToString());
                    ap.Comments = row["Comments"].ToString();
                    ap.DatePrepared = DateTime.Parse(row["DatePrepared"].ToString());
                    ap.DateNoted = DateTime.Parse(row["DateNoted"].ToString());
                    ap.DateDiscussed = DateTime.Parse(row["DateDiscussed"].ToString());
                    ap.Type = (AppraisalType)Int32.Parse(row["Type"].ToString());

                    if (recursive)
                    {
                        ap.Evaluator = (Employee)new Employee().FindProfile(Int32.Parse(row["Evalutator"].ToString()), byPrimary: true);
                        ap.NotedBy = (Employee)new Employee().FindProfile(Int32.Parse(row["NotedBy"].ToString()), byPrimary: true);
                        ap.DiscussedWith = (Employee)new Employee().FindProfile(Int32.Parse(row["DiscussedWith"].ToString()), byPrimary:true);
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
                             + "DiscussedWith, DateDiscussed, Type) OUTPUT INSERTED.AppraisalID ";
            string values = " VALUES(@CoveredPeriod, @Criteria, @Rating, @TechComp,"
                            + " @InterSkills, @CommComp, @Total, @Comments, @Evaluator, "
                            + "@DatePrepare, @NotedBy, @DateNoted, @DiscussedWith, @DateDiscussed, @Type)";

            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@CoveredPeriod", this.CoveredPeriod.ToShortDateString());
            param.Add("@Criteria", this.Critera);
            param.Add("@Rating", this.Rating);
            param.Add("@TechComp", this.TechComp);
            param.Add("@InterSkills", this.InterSkills);
            param.Add("@CommComp", this.CommComp);
            param.Add("@Total", this.Total);
            param.Add("@Comments", this.Comments);
            param.Add("@Evaluator", this.Evaluator.EmployeeID);
            param.Add("@DatePrepared", this.DatePrepared.ToShortDateString());
            param.Add("@NotedBy", this.NotedBy.EmployeeID);
            param.Add("@DateNoted", this.DateNoted.ToShortDateString());
            param.Add("@DiscussedWith", this.DiscussedWith.EmployeeID);
            param.Add("@DateDiscussed", this.DateDiscussed.ToShortDateString());
            param.Add("@Type", (int)this.Type);

            this.AppraisalID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public Appraisal Update(bool recursive = true, bool byDiscussed = false)
        {
            return this.Update(this.AppraisalID, recursive, byDiscussed);
        }

        public Appraisal Update(int TableID, bool recursive = true, bool byDiscussed = false)
        {
            string set = "UPDATE Appraisal SET CoveredPeriod = @CoveredPeriod AND "
                         + "Criteria = @Criteria AND Rating = @Rating AND "
                         + "TechComp = @TechComp AND InterSkills = @InterSkills AND "
                         + "CommComp = @CommComp AND Total = @Total AND "
                         + "Comments = @Comments AND Evaluator = @Evaluator AND "
                         + "DatePrepared = @DatePrepared AND NotedBy = @NotedBy AND "
                         + "DateNoted = @DateNoted AND DiscussedWith = @DiscussedWith AND "
                         + "DateDiscussed = @DateDiscussed AND Type = @Type WHERE " + 
                         (byDiscussed ? " DiscussedWith " : " AppraisalID ") + " = " + TableID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@CoveredPeriod", this.CoveredPeriod.ToShortDateString());
            param.Add("@Criteria", this.Critera);
            param.Add("@Rating", this.Rating);
            param.Add("@TechComp", this.TechComp);
            param.Add("@InterSkills", this.InterSkills);
            param.Add("@CommComp", this.CommComp);
            param.Add("@Total", this.Total);
            param.Add("@Comments", this.Comments);
            param.Add("@Evaluator", this.Evaluator.EmployeeID);
            param.Add("@DatePrepared", this.DatePrepared.ToShortDateString());
            param.Add("@NotedBy", this.NotedBy.EmployeeID);
            param.Add("@DateNoted", this.DateNoted.ToShortDateString());
            param.Add("@DiscussedWith", this.DiscussedWith.EmployeeID);
            param.Add("@DateDiscussed", this.DateDiscussed.ToShortDateString());
            param.Add("@Type", (int)this.Type);

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