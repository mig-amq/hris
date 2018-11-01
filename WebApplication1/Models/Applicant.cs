﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Collections;
    using System.Data;
    using WebApplication1.Models.Supers;

    public enum ApplicantStatus
    {
        Undecided = 1,
        Accepted = 2,
        Rejected = 3
    }

    public class Applicant : ProfiledObject
    {
        public int ApplicantID { get; set; }
        public string Skills { get; set; }
        public string DesiredPosition { get; set; }
        public ApplicantStatus Status { get; set; }
        
        public Applicant() {}
        public Applicant(int ProfileID, bool byPrimary = false)
            : base(ProfileID, byPrimary){}

        public override ProfiledObject FindProfile(int TableID, bool recursive = true, bool byPrimary = false)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT * FROM Applicant WHERE " + (byPrimary ? " ApplicantID " : " Profile ") + " = " + TableID))
            {
                DataRow row = dt.Rows[0];

                // Fill Applicant Object
                this.ApplicantID = Int32.Parse(row["ApplicantID"].ToString());
                this.Skills = row["Skills"].ToString();
                this.DesiredPosition = row["DesiredPosition"].ToString();
                this.Status = (ApplicantStatus)Int32.Parse(row["Status"].ToString());

                if (recursive)
                {
                    this.Profile = new Profile(Int32.Parse(row["Profile"].ToString()));
                }
            }

            return this;
        }

        public Applicant Create()
        {
            if (this.Profile != null)
            {
                this.Profile.Create();

                string columns = "INSERT INTO Applicant(Profile, Skills, DesiredPosition, Status) OUTPUT INSERTED.ApplicantID";
                string values = " Values(@Profile, @Skills, @DesiredPosition, @Status)";
                Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

                param.Add("@Profile", this.Profile.ProfileID);
                param.Add("@Skills", this.Skills);
                param.Add("@DesiredPositon", this.DesiredPosition);
                param.Add("@Status", (int)this.Status);

                this.ApplicantID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            }
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

            string set = "UPDATE Applicant SET Skills = @Skills, "
                         + "DesiredPosition = @DesiredPosition, Status = @Status OUTPUT INSERTED.ApplicantID WHERE ApplicantID = " + this.ApplicantID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Skills", this.Skills);
            param.Add("@DesiredPosition", this.DesiredPosition);
            param.Add("@Status", (int)this.Status);

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
    }
}