using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;
    using System.Diagnostics;

    public enum EducationType
    {
        Elementary,
        HighSchool,
        College,
        PostGraduate
    }

    public class EducationLevel
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Start { get; set; }
        public string End { get; set; }
        public EducationType Type { get; set; }
        public bool Exists { get; set; }

        public EducationLevel(EducationType type)
        {
            this.Type = type;
        }
    }

    public class Education
    {
        private DBHandler DBHandler;
        public int EducationID { get; set; }

        public EducationLevel Elementary { get; set; }
        public EducationLevel HighSchool { get; set; }
        public EducationLevel College { get; set; }
        public EducationLevel PostGraduate { get; set; }

        public Education(bool instantiate = true)
        {
            DBHandler = new DBHandler();

            if (instantiate)
            {
                Elementary = new EducationLevel(EducationType.Elementary);
                HighSchool = new EducationLevel(EducationType.HighSchool);
                College = new EducationLevel(EducationType.College);
                PostGraduate = new EducationLevel(EducationType.PostGraduate);
            }
        }

        public Education(int EducationID) : this()
        {
            this.EducationID = EducationID;
            this.Find(this.EducationID);
        }

        public Education Find(int EducationID)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM EducationalBackground WHERE EducationID = " + this.EducationID))
            {
                DataRow row = dt.Rows[0];

                this.EducationID = EducationID;
                this.Elementary.Name = row["Elementary"].ToString();
                this.Elementary.Address = row["ElemAddress"].ToString();
                this.Elementary.Start = row["ElemStart"].ToString();
                this.Elementary.End = row["ElemEnd"].ToString();

                this.HighSchool.Name = row["HighSchool"].ToString();
                this.HighSchool.Address = row["HighSchool"].ToString();
                this.HighSchool.Start = row["HSStartYear"].ToString();
                this.HighSchool.End = row["HSEndYear"].ToString();

                this.College.Name = row["College"].ToString();
                this.College.Address = row["CollegeAddress"].ToString();
                this.College.Start = row["CollegeStartYear"].ToString();
                this.College.End = row["CollegeEndYear"].ToString();

                this.PostGraduate.Name = row["PostGrad"].ToString();
                this.PostGraduate.Address = row["PostGradAddress"].ToString();
                this.PostGraduate.Start = row["PostGradStartYear"].ToString();
                this.PostGraduate.End = row["PostGradEndYear"].ToString();
            }

            return this;
        }

        public Education Create()
        {
            string columns = "INSERT INTO EducationalBackground (";
            string values = " OUTPUT INSERTED.EducationID VALUES(";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            if (this.Elementary != null)
            {
                columns += "Elementary, ElemStartYear, ElemEndYear, ElemAddress";
                param.Add("@Elementary", this.Elementary.Name);
                param.Add("@ElemStartYear", this.Elementary.Start);
                param.Add("@ElemEndYear", this.Elementary.End);
                param.Add("@ElemAddress", this.Elementary.Address);

                if (this.HighSchool != null || this.College != null || this.PostGraduate != null)
                {
                    columns += ", ";
                }
            }

            if (this.HighSchool != null)
            {
                columns += "HighSchool, HSStartYear, HSEndYear, HSAddress";
                param.Add("@HighSchool", this.HighSchool.Name);
                param.Add("@HSStartYear", this.HighSchool.Start);
                param.Add("@HSEndYear", this.HighSchool.End);
                param.Add("@HSAddress", this.HighSchool.Address);

                if (this.College != null || this.PostGraduate != null)
                {
                    columns += ", ";
                }
            }

            if (this.College != null)
            {
                columns += "College, CollegeStartYear, CollegeEndYear, CollegeAddress";
                param.Add("@College", this.College.Name);
                param.Add("@CollegeStartYear", this.College.Start);
                param.Add("@CollegeEndYear", this.College.End);
                param.Add("@CollegeAddress", this.College.Address);

                if (this.PostGraduate != null)
                {
                    columns += ", ";
                }
            }

            if (this.PostGraduate != null)
            {
                columns += "PostGrad, PostGradStartYear, PostGradEndYear, PostGradAddress";
                param.Add("@PostGrad", this.PostGraduate.Name);
                param.Add("@PostGradStartYear", this.PostGraduate.Start);
                param.Add("@PostGradEndYear", this.PostGraduate.End);
                param.Add("@PostGradAddress", this.PostGraduate.Address);
            }

            columns += ")";

            foreach (KeyValuePair<string, dynamic> pair in param)
            {
                values += pair.Key + ",";
            }

            values = values.Substring(0, values.Length - 1) + ")";
            if (param.Keys.Count > 0)
                this.EducationID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public Education Update(bool recursive = true)
        {
            return this.Update(this.EducationID, recursive);
        }

        public Education Update(int EducationID, bool recursive)
        {
            string set = "UPDATE [EducationalBackground] SET ";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            if (this.Elementary != null)
            {
                set = "Elementary = @Elementary AND ElemStartYear = @ElemStartYear AND "
                      + "ElemEndYear = @ElemEndYear AND ElemAddress = @ElemAddress";

                param.Add("@Elementary", this.Elementary.Name);
                param.Add("@ElemStartYear", this.Elementary.Start);
                param.Add("@ElemEndYear", this.Elementary.End);
                param.Add("@ElemAddress", this.Elementary.Address);

                if (this.HighSchool != null || this.College != null || this.PostGraduate != null)
                {
                    set += " AND ";
                }
            }

            if (this.HighSchool != null)
            {
                set += "HighSchool = @HighSchool AND HSStartYear = @HSStartYear AND "
                       + "HSEndYear = @HSEndYear AND HSAddress = @HSAddress";
                param.Add("@HighSchool", this.HighSchool.Name);
                param.Add("@HSStartYear", this.HighSchool.Start);
                param.Add("@HSEndYear", this.HighSchool.End);
                param.Add("@HSAddress", this.HighSchool.Address);

                if (this.College != null || this.PostGraduate != null)
                {
                    set += " AND ";
                }
            }

            if (this.College != null)
            {
                set += "College = @College AND CollegeStartYear = @CollegeStartYear AND"
                       + " CollegeEndYear = @CollegeEndYear AND CollegeAddress = @CollegeAddress";
                param.Add("@College", this.College.Name);
                param.Add("@CollegeStartYear", this.College.Start);
                param.Add("@CollegeEndYear", this.College.End);
                param.Add("@CollegeAddress", this.College.Address);

                if (this.PostGraduate != null)
                {
                    set += " AND ";
                }
            }

            if (this.PostGraduate != null)
            {
                set += "PostGrad = @PostGrad AND PostGradStartYear = @PostGradStartYear AND"
                       + " PostGradEndYear = @PostGradEndYear AND PostGradAddress = @PostGradAddress";
                param.Add("@PostGrad", this.PostGraduate.Name);
                param.Add("@PostGradStartYear", this.PostGraduate.Start);
                param.Add("@PostGradEndYear", this.PostGraduate.End);
                param.Add("@PostGradAddress", this.PostGraduate.Address);
            }

            set += " OUTPUT INSERTED.EducationID WHERE EducationID = " + EducationID;

            this.EducationID = this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public Education Delete()
        {
            this.DBHandler.Execute<Int32>(CRUD.DELETE, "DELETE FROM [EducationalBackground] WHERE EducationID = " + this.EducationID);
            return this;
        }
    }

}