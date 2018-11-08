using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;
    using System.Diagnostics;

    public class JobPosting
    {
        private DBHandler DBHandler;
        public int PostingID { get; set; }
        public string JobTitle { get; set; }
        public string JobDescription { get; set; }
        public string Requirements { get; set; }
        public DateTime DatePosted { get; set; }

        public JobPosting(int PostingID = -1)
        {
            this.DBHandler = new DBHandler();

            if (PostingID != -1)
            {
                this.PostingID = PostingID;
                this.Find(this.PostingID);
            }
        }

        public JobPosting Find(int PostingID, bool recursive = true)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM JobPosting WHERE PostingID = " + PostingID))
            {
                DataRow row = dt.Rows[0];

                this.JobTitle = row["JobTitle"].ToString();
                this.JobDescription = row["JobDescription"].ToString();
                this.Requirements = row["Requirements"].ToString();
                this.DatePosted = DateTime.Parse(row["DatePosted"].ToString());
                this.PostingID = PostingID;

            }

            return this;
        }

        public JobPosting Create()
        {
            string columns = "INSERT INTO JobPosting(JobTitle, JobDescription, "
                             + "Requirements, DatePosted) OUTPUT INSERTED.PostingID ";
            string values = " VALUES(@JobTitle, @JobDescription, @Requirements, @DatePosted)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@JobTitle", this.JobTitle);
            param.Add("@JobDescription", this.JobDescription);
            param.Add("@Requirements", this.Requirements);
            param.Add("@DatePosted", this.DatePosted.ToString("yyyy-MM-dd"));

            this.PostingID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public JobPosting Update(bool recursive = true)
        {
            return this.Update(this.PostingID, recursive);
        }

        public JobPosting Update(int PostingID, bool recursive = true)
        {
            string set = "UPDATE JobPosting SET JobTitle = @JobTitle, "
                         + "JobDescription = @JobDescription, "
                         + "Requirements = @Requirements, DatePosted = @DatePosted OUTPUT INSERTED.PostingID WHERE PostingID = " + PostingID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@JobTitle", this.JobTitle);
            param.Add("@JobDescription", this.JobDescription);
            param.Add("@Requirements", this.Requirements);
            param.Add("@DatePosted", this.DatePosted.ToString("yyyy-MM-dd"));

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public JobPosting Delete()
        {
            Debug.WriteLine("DELETE FROM JobPosting WHERE PostingID = " + this.PostingID);
            this.DBHandler.Execute<Int32>(CRUD.DELETE, "DELETE FROM JobPosting WHERE PostingID = " + this.PostingID);
            return this;
        }

    }
}