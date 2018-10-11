using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Collections;
    using System.Data;
    using WebApplication1.Models.Supers;

    public class Applicant : ProfiledObject
    {
        public int ApplicantID { get; set; }
        public string Skills { get; set; }
        public string DesiredPosition { get; set; }
       
        public Applicant() {}
        public Applicant(int ProfileID)
            : base(ProfileID){}

        public override ProfiledObject FindProfile(int ProfileID, bool recursive = false)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT * FROM [Profile] P, [Applicant] A INNER JOIN ON P.ProfileID = A.Profile WHERE P.ProfileID = " + ProfileID))
            {
                DataRow row = dt.Rows[0];

                // Fill Profile Object
                this.Profile.FirstName = row["FirstName"].ToString();
                this.Profile.MiddleName = row["MiddleName"].ToString();
                this.Profile.LastName = row["LastName"].ToString();
                this.Profile.BirthDate = DateTime.Parse(row["BirthDate"].ToString());
                this.Profile.CivilStatus = (CivilStatusType)Enum.Parse(
                    typeof(CivilStatusType),
                    row["CivilStatus"].ToString(),
                    true);
                this.Profile.Sex = (SexType)Enum.Parse(typeof(SexType), row["Sex"].ToString(), true);
                this.Profile.Contact = Int32.Parse(row["Contact"].ToString());
                this.Profile.ContactPerson = row["Contact Person"].ToString();
                this.Profile.CPersonNo = Int32.Parse(row["CPersonNo"].ToString());
                this.Profile.CPersonRel = row["CPersonRel"].ToString();
                this.Profile.City = row["City"].ToString();
                this.Profile.HouseNo = row["HouseNo"].ToString();
                this.Profile.Province = row["Province"].ToString();
                this.Profile.Street = row["Street"].ToString();

                // Fill Applicant Object

                if (recursive)
                {
                    this.Profile.Education = new Education(Int32.Parse(row["Education"].ToString()));
                }
            }

            return this;
        }

        public Applicant Create()
        {
            if (this.Profile != null)
            {
                this.Profile.Create();

                string columns = "INSERT INTO Applicant(Profile, Skills, DesiredPosition) OUTPUT INSERTED.ApplicantID";
                string values = " Values(@Profile, @Skills, @DesiredPosition)";
                Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

                param.Add("@Profile", this.Profile.ProfileID);
                param.Add("@Skills", this.Skills);
                param.Add("@DesiredPositon", this.DesiredPosition);

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
            return this;
        }

        public Applicant Delete()
        {
            this.DBHandler.Execute<Int32>(CRUD.DELETE, "DELETE FROM Applicant WHERE ApplicantID = " + this.ApplicantID);
            this.Profile.Delete();
            return this;
        }
    }
}