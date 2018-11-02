using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Collections;
    using System.Data;

    public enum SexType
    {
        Male = 1,
        Female = 2,
        Other = 3
    }

    public enum CivilStatusType
    {
        Married = 1,
        Single = 2,
        Divorced = 3,
        Separated = 4,
        Widowed = 5
    }

    public class Profile
    {
        private DBHandler DBHandler;
        public int ProfileID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public SexType Sex { get; set; }
        public CivilStatusType CivilStatus { get; set; }
        public Education Education { get; set; }
        public string Contact { get; set; }
        public string ContactPerson { get; set; }
        public string CPersonNo { get; set; }
        public string CPersonRel { get; set; }

        public string HouseNo { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Province { get; set; }

        public ArrayList EmploymentHistories { get; set; }

        public Profile(int ProfileID = -1)
        {
            this.EmploymentHistories = new ArrayList();
            this.DBHandler = new DBHandler();

            if (ProfileID != -1)
            {
                this.ProfileID = ProfileID;
                this.Find(this.ProfileID);
            }
        }

        public Profile Find(int ProfileID)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM Profile WHERE ProfileID = " + ProfileID))
            {
                DataRow row = dt.Rows[0];
                
                try
                {
                    this.Education = new Education(Int32.Parse(row["Educaton"].ToString()));
                }
                catch (Exception e)
                {
                    this.Education = null;
                }

                this.FirstName = row["FirstName"].ToString();
                this.MiddleName = row["MiddleName"].ToString();
                this.LastName = row["LastName"].ToString();

                this.HouseNo = row["HouseNo"].ToString();
                this.Street = row["Street"].ToString();
                this.City = row["City"].ToString();
                this.Province = row["Province"].ToString();

                this.Sex = (SexType)Enum.Parse(typeof(SexType), row["Sex"].ToString(), true);
                this.CivilStatus = (CivilStatusType)Enum.Parse(typeof(CivilStatusType), row["CivilStatus"].ToString(), true);
                this.BirthDate = DateTime.Parse(row["BirthDate"].ToString());

                this.Contact = row["Contact"].ToString();
                this.ContactPerson = row["ContactPerson"].ToString();
                this.CPersonNo = row["CPersonNo"].ToString();
                this.CPersonRel = row["CPersonRel"].ToString();
                this.ProfileID = ProfileID;
            }

            return this;
        }

        public Profile Create()
        {
            if (this.Education != null)
                this.Education.Create();
            string columns = "INSERT INTO [dbo].[Profile] (FirstName, LastName, MiddleName, BirthDate, Sex, "
                             + "CivilStatus, Contact, ContactPerson, CPersonNo, CPersonRel, "
                             + "HouseNo, Street, City, Province, Education) OUTPUT INSERTED.ProfileID ";
            string values = "VALUES(@FirstName, @LastName, @MiddleName, @BirthDate, @Sex, @CivilStatus, @Contact,"
                            + " @ContactPerson, @CPersonNo, @CPersonRel, @HouseNo, @Street, @City, @Province, @Education)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@FirstName", this.FirstName);
            param.Add("@LastName", this.LastName);
            param.Add("@MiddleName", this.MiddleName);
            param.Add("@BirthDate", this.BirthDate.ToString("yyyy-MM-dd"));
            param.Add("@Sex", (int)this.Sex);
            param.Add("@CivilStatus", (int)this.CivilStatus);
            param.Add("@Contact", this.Contact);
            param.Add("@ContactPerson", this.ContactPerson);
            param.Add("@CPersonNo", this.CPersonNo);
            param.Add("@CPersonRel", this.CPersonRel);
            param.Add("@HouseNo", this.HouseNo);
            param.Add("@Street", this.Street);
            param.Add("@City", this.City);
            param.Add("@Province", this.Province);
            if (this.Education != null)
                param.Add("@Education", this.Education.EducationID);
            else
                param.Add("@Education", DBNull.Value);

            this.ProfileID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public Profile Update(bool recursive = true)
        {
            return this.Update(this.ProfileID, recursive);
        }
        public Profile Update(int ProfileID, bool recursive = true)
        {
            if (recursive)
                if (this.Education != null) this.Education.Update(recursive);

            string set = "UPDATE Profile SET FirstName = @FirstName, MiddleName = @MiddleName, "
                         + "LastName = @LastName, HouseNo = @HouseNo, Street = @Street, "
                         + "City = @City, Province = @Province, Sex = @Sex, CivilStatus = @CivilStatus, "
                         + "BirthDate = @BirthDate, Contact = @Contact, ContactPerson = @ContactPerson, "
                         + "CPersonNo = @CPersonNo, CPersonRel = @CPersonRel OUTPUT INSERTED.ProfileID WHERE ProfileID = " + ProfileID;

            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@FirstName", this.FirstName);
            param.Add("@MiddleName", this.MiddleName);
            param.Add("@LastName", this.LastName);

            param.Add("@HouseNo", this.HouseNo);
            param.Add("@Street", this.Street);
            param.Add("@City", this.City);
            param.Add("@Province", this.Province);
            param.Add("@Sex", (int)this.Sex);
            param.Add("@CivilStatus", (int)this.CivilStatus);
            param.Add("@BirthDate", this.BirthDate.ToString("yyyy-MM-dd"));

            param.Add("@Contact", this.Contact);
            param.Add("@ContactPerson", this.ContactPerson);
            param.Add("@CPersonNo", this.CPersonNo);
            param.Add("@CPersonRel", this.CPersonRel);

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public Profile Delete()
        {
            if (this.Education != null)
                this.Education.Delete();

            this.DBHandler.Execute<Int32>(CRUD.DELETE, "DELETE FROM Profile WHERE ProfileID = " + this.ProfileID);
            return this;
        }
    }
}