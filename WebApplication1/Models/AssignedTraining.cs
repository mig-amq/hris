using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;

    public enum TrainingStatus
    {
        Pending = 1,
        Attended = 2,
        Suggested = 3
    }

    public class AssignedTraining
    {
        private DBHandler DBHandler;
        public int AssignedTrainingID { get; set; }
        public Profile Profile { get; set; }
        public Training Training { get; set; }
        public TrainingStatus Status { get; set; }

        public AssignedTraining(int AssignedTrainingID = -1, bool recursive = true)
        {

            this.DBHandler = new DBHandler();
            if (AssignedTrainingID != -1)
            {
                this.AssignedTrainingID = AssignedTrainingID;
                this.Find(AssignedTrainingID, recursive);
            }
        }

        public AssignedTraining Create(bool recursive = true)
        {
            if (recursive)
            {
                this.Profile.Create();
                this.Training.Create();
            }

            string sql = "INSERT INTO AssignedTraining(Training, Profile, Status) "
                         + "OUTPUT INSERTED.AssignedTrainingID VALUES(@Training, @Profile, @Status)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Training", this.Training.TrainingHistoryID);
            param.Add("@Profile", this.Profile.ProfileID);
            param.Add("@Status", (int)this.Status);

            this.AssignedTrainingID = this.DBHandler.Execute<Int32>(CRUD.CREATE, sql, param);
            return this;
        }

        public AssignedTraining Find(int AssignedTrainingID = -1, bool recursive = true, bool byPrimary = true, bool byProfile = false)
        {
            string sql = "SELECT * FROM AssignedTraining WHERE "
                         + (byPrimary ? " AssignedTrainingID " : (byProfile) ? " Profile " : " Training ") + " = "
                         + AssignedTrainingID;

            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, sql))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    this.Status = (TrainingStatus)Int32.Parse(row["Status"].ToString());

                    if (recursive)
                    {
                        this.Profile = new Profile(Int32.Parse(row["Profile"].ToString()));
                        this.Training = new Training(Int32.Parse(row["Training"].ToString()));
                    }
                }
            }
            return this;
        }

        public List<AssignedTraining> FindAll(int TableID, bool recursive, bool byProfile = false)
        {
            List<AssignedTraining> ast = new List<AssignedTraining>();

            string sql = "SELECT * FROM AssignedTraining WHERE " + (byProfile ? " Profile " : " Training ") + " = "
                         + TableID;

            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, sql))
            {
                foreach (DataRow row in dt.Rows)
                {
                    AssignedTraining at = new AssignedTraining();
                    at.Status = (TrainingStatus)Int32.Parse(row["AssignedTrainingID"].ToString());

                    if (recursive)
                    {
                        at.Profile = new Profile(Int32.Parse(row["Profile"].ToString()));
                        at.Training = new Training(Int32.Parse(row["Training"].ToString()));
                    }

                    ast.Add(at);
                }
            }

            return ast;
        }

        public AssignedTraining Update(bool recursive = true)
        {
            return this.Update(this.AssignedTrainingID, recursive: recursive);
        }

        public AssignedTraining Update(int AssignedTrainingID = -1, bool recursive = true, bool byPrimary = true, bool byProfile = false)
        {
            string sql = "UPDATE AssignedTraining SET Training = @Training, Profile = @Profile, Status = @Status OUTPUT INSERTED.AssignedTrainingID WHERE " 
                         + (byPrimary ? " AssignedTrainingID " : (byProfile) ? " Profile " : " Training ") + " = " + AssignedTrainingID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            if (recursive)
            {
                this.Profile.Update(recursive);
                this.Training.Update(recursive);
            }

            param.Add("@Training", this.Training.TrainingHistoryID);
            param.Add("@Profile", this.Profile.ProfileID);
            param.Add("@Status", (int)this.Status);

            this.AssignedTrainingID = this.DBHandler.Execute<Int32>(CRUD.UPDATE, sql, param);
            return this;
        }
    }
}