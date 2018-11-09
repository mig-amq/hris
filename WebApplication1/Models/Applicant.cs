using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Collections;
    using System.Data;

    using Calfurn.Models;

    using WebApplication1.Models.Supers;

    public class Applicant : ProfiledObject
    {
        public int ApplicantID { get; set; }
        public string Skills { get; set; }
        public string[] SupportingFiles { get; set; }
        
        public Applicant() {}
        public Applicant(int ProfileID, bool byPrimary = false)
            : base(ProfileID, byPrimary){}

        public override ProfiledObject FindProfile(int TableID, bool recursive = true, bool byPrimary = false)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT * FROM Applicant WHERE " + (byPrimary ? " ApplicantID " : " Profile ") + " = " + TableID))
            {
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    // Fill Applicant Object
                    this.ApplicantID = Int32.Parse(row["ApplicantID"].ToString());
                    this.Skills = row["Skills"].ToString();
                    this.SupportingFiles = row["SupportingFiles"].ToString().Split(';');

                    if (recursive)
                    {
                        this.Profile = new Profile(Int32.Parse(row["Profile"].ToString()));
                    }
                }
                else
                {
                    throw new Exception(
                        "Cannot find employee with the " + (byPrimary ? " primary " : " profile ") + "id: " + TableID);
                }
            }

            return this;
        }

        public Applicant Create(bool recursive = false)
        {
            if (this.Profile != null && recursive)
            {
                this.Profile.Create();
            }
            
            string columns = "INSERT INTO Applicant(Profile, Skills, SupportingFiles) OUTPUT INSERTED.ApplicantID";
            string values = " Values(@Profile, @Skills, @SupportingFiles)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Profile", this.Profile.ProfileID);
            param.Add("@Skills", this.Skills);
            param.Add("@SupportingFiles", String.Join(";", this.SupportingFiles));

            this.ApplicantID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public Applicant Update(bool recursive = true)
        {
            return this.Update(this.ApplicantID, recursive);
        }

        public Applicant Update(int ApplicantID, bool recursive = true)
        {
            if (recursive)
            {
                this.Profile.Update(recursive);
            }

            string set = "UPDATE Applicant SET Skills = @Skills, Profile = @Profile, SupportingFiles = @SupportingFiles"
                         + " OUTPUT INSERTED.ApplicantID WHERE ApplicantID = " + this.ApplicantID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Profile", this.Profile.ProfileID);
            param.Add("@Skills", this.Skills);
            param.Add("@SupportingFiles", String.Join(";", this.SupportingFiles));

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public Applicant Delete(bool recursive = false)
        {
            this.DBHandler.Execute<Int32>(CRUD.DELETE, "DELETE FROM Applicant WHERE ApplicantID = " + this.ApplicantID);

            if (recursive)
                this.Profile.Delete();
            return this;
        }

        public List<JobApplication> GetApplications(int ApplicantID)
        {
            List<JobApplication> Applications = new List<JobApplication>();

            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT JobApplicationID FROM JobApplication WHERE Applicant = " + ApplicantID))
            {
                foreach (DataRow row in dt.Rows)
                {
                    Applications.Add(new JobApplication(Int32.Parse(row["JobApplicationID"].ToString())));
                }
            }

            return Applications;
        }
    }
}