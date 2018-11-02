using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;
    using System.Diagnostics;

    public class Training
    {
        private DBHandler DBHandler;
        public int TrainingHistoryID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Facilitator { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Location { get; set; }

        public Training(int TrainingHistoryID = -1)
        {
            this.DBHandler = new DBHandler();

            if (TrainingHistoryID != -1)
            {
                this.TrainingHistoryID = TrainingHistoryID;
                this.Find(this.TrainingHistoryID);
            }
        }

        public Training Find(int TrainingHistoryID)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT * FROM TrainingHistory WHERE TrainingHistoryID = " + TrainingHistoryID))
            {
                DataRow row = dt.Rows[0];

                this.TrainingHistoryID = Int32.Parse(row["TrainingHistoryID"].ToString());
                this.Title = row["Title"].ToString();
                this.Description = row["Description"].ToString();
                this.Facilitator = row["Facilitator"].ToString();
                this.StartDate = DateTime.Parse(row["StartDate"].ToString());
                this.EndDate = DateTime.Parse(row["EndDate"].ToString());
                this.Location = row["Location"].ToString();

            }

            return this;
        }

        public Training Create()
        {
            string columns = "INSERT INTO TrainingHistory(Title, Description, Facilitator, "
                             + "StartDate, EndDate, Location) OUTPUT INSERTED.TrainingHistoryID ";
            string values = " VALUES (@Title, @Description, @Facilitator, @StartDate, @EndDate, @Location)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Title", this.Title);
            param.Add("@Description", this.Description);
            param.Add("@Facilitator", this.Facilitator);
            param.Add("@StartDate", this.StartDate.ToString("yyyy-MM-dd"));
            param.Add("@EndDate", this.EndDate.ToString("yyyy-MM-dd"));
            param.Add("@Location", this.Location);

            this.TrainingHistoryID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public Training Update(bool recursive = true)
        {
            return this.Update(this.TrainingHistoryID, recursive);
        }

        public Training Update(int TrainingHistoryID, bool recursive = true)
        {
            string set = "UPDATE TrainingHistory SET Title = @Title, "
                         + "Description = @Description, Facilitator = @Facilitator, "
                         + "StartDate = @StartDate, EndDate = @EndDate, "
                         + "Location = @Location OUTPUT INSERTED.TrainingHistoryID WHERE TrainingHistoryID = " + TrainingHistoryID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            Debug.WriteLine(this.StartDate.ToString("yyyy-MM-dd"));
            param.Add("@Title", this.Title);
            param.Add("@Description", this.Description);
            param.Add("@Facilitator", this.Facilitator);
            param.Add("@StartDate", this.StartDate.ToString("yyyy-MM-dd"));
            param.Add("@EndDate", this.EndDate.ToString("yyyy-MM-dd"));
            param.Add("@Location", this.Location);

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public Training Delete()
        {
            this.DBHandler.Execute<Int32>(
                CRUD.DELETE,
                "DELETE FROM TrainingHistory OUTPUT DELETED.TrainingHistoryID WHERE TrainingHistoryID = " + this.TrainingHistoryID);
            return this;
        }
    }
}