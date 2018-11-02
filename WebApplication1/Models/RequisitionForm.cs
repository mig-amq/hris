using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;

    using WebApplication1.Models.Supers;

    public enum RequisitionStatus
    {
        Pending = 1,
        Requisitioned = 2,
    }

    public class RequisitionForm
    {
        private DBHandler DBHandler;
        public int RequisitionID { get; set; }
        public DateTime Date { get; set; }
        public Department Department { get; set; }
        public string Position { get; set; }
        public Employee RequestedBy { get; set; }
        public string ReasonforVacancy { get; set; }
        public string Type { get; set; }
        public string Qualification { get; set; }
        public string ExperienceRequired { get; set; }
        public DateTime ExpectedJoiningDate { get; set; }
        public Employee UnderSupervision { get; set; }
        public string Description { get; set; }
        public string SkillsRequired { get; set; }
        public RequisitionStatus Status { get; set; }

        public RequisitionForm(int RequisitionID = -1)
        {
            this.DBHandler = new DBHandler();

            if (RequisitionID != -1)
            {
                this.RequisitionID = RequisitionID;
                this.Find(this.RequisitionID);
            }
        }

        public RequisitionForm Find(int RequisitionID, bool recursive = true)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM RequisitionForm WHERE RequisitionID = " + this.RequisitionID))
            {
                DataRow row = dt.Rows[0];

                this.Date = DateTime.Parse(row["Date"].ToString());
                this.Position = row["Position"].ToString();
                this.ReasonforVacancy = row["ReasonforVacancy"].ToString();
                this.Type = row["Type"].ToString();
                this.Qualification = row["Qualification"].ToString();
                this.ExperienceRequired = row["ExperienceRequired"].ToString();
                this.SkillsRequired = row["SkillsRequired"].ToString();
                this.ExpectedJoiningDate = DateTime.Parse(row["ExpectedJoiningDate"].ToString());
                this.Description = row["BriefDescriptionofWorks"].ToString();
                this.Status = (RequisitionStatus)Int32.Parse(row["Status"].ToString());

                this.RequisitionID = RequisitionID;

                if (recursive)
                {
                    this.Department = new Department(Int32.Parse(row["Department"].ToString()));
                    this.RequestedBy = (Employee)new Employee().FindProfile(Int32.Parse(row["RequestedBy"].ToString()), byPrimary: true);
                    this.UnderSupervision = (Employee)new Employee().FindProfile(Int32.Parse(row["UnderSupervision"].ToString()), byPrimary: true);
                }
            }
            return this;
        }

        public RequisitionForm Create()
        {
            string columns = "INSERT INTO RequisitionForm(Date, Department, Position, "
                             + "RequestedBy, ReasonforVacancy, Type, Qualification,"
                             + " ExperienceRequired, SkillsRequired, ExpectedJoiningDate, "
                             + "UnderSupervision, BriefDescriptionofWorks, Status) OUTPUT INSERTED.RequisitionID ";
            string values = " Values(@Date, @Department, @Position, @RequestedBy, @ReasonforVacancy, "
                            + "@Type, @Qualification, @ExperienceRequired, @SkillsRequired, "
                            + "@ExpectedJoiningDate, @UnderSupervision, @BriefDescriptionofWorks, @Status)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Date", this.Date.ToString("yyyy-MM-dd"));
            param.Add("@Department", this.Department.DepartmentID);
            param.Add("@Position", this.Position);
            param.Add("@RequestedBy", this.RequestedBy.EmployeeID);
            param.Add("@ReasonforVacancy", this.ReasonforVacancy);
            param.Add("@Type", this.Type);
            param.Add("@Qualification", this.Qualification);
            param.Add("@ExperienceRequired", this.ExperienceRequired);
            param.Add("@SkillsRequired", this.SkillsRequired);
            param.Add("@ExpectedJoiningDate", this.ExpectedJoiningDate.ToString("yyyy-MM-dd"));
            param.Add("@UnderSupervision", this.UnderSupervision.EmployeeID);
            param.Add("@BriefDescriptionofWorks", this.Description);
            param.Add("@Status", (int)this.Status);

            this.RequisitionID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }
        public RequisitionForm Update(bool recursive = true)
        {
            return this.Update(this.RequisitionID, recursive);
        }

        public RequisitionForm Update(int RequisitionID, bool recursive = true)
        {
            if (recursive)
            {
                this.RequestedBy.Update();
                this.UnderSupervision.Update();
            }

            string set = "UPDATE RequisitionForm SET Date = @Date, Position = @Position, "
                         + "RequestedBy = @RequestedBy, ReasonforVacancy = @ReasonforVacancy"
                         + ", Type = @Type, Qualification = @Qualification, "
                         + "ExperienceRequired = @ExperienceRequired, "
                         + "SkillsRequired = @SkillsRequired, Department = @Department,"
                         + "ExpectedJoiningDate = @ExpectedJoiningDate, "
                         + "UnderSupervision = @UnderSupervision, Status = @Status,"
                         + "BriefDescriptionofWorks = @BriefDescriptionofWorks OUTPUT INSERTED.RequisitionID "
                         + "WHERE RequisitionID = " + RequisitionID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@Date", this.Date.ToString("yyyy-MM-dd"));
            param.Add("@Department", this.Department.DepartmentID);
            param.Add("@Position", this.Position);
            param.Add("@RequestedBy", this.RequestedBy.EmployeeID);
            param.Add("@ReasonforVacancy", this.ReasonforVacancy);
            param.Add("@Type", this.Type);
            param.Add("@Qualification", this.Qualification);
            param.Add("@ExperienceRequired", this.ExperienceRequired);
            param.Add("@SkillsRequired", this.SkillsRequired);
            param.Add("@ExpectedJoiningDate", this.ExpectedJoiningDate.ToString("yyyy-MM-dd"));
            param.Add("@UnderSupervision", this.UnderSupervision.EmployeeID);
            param.Add("@BriefDescriptionofWorks", this.Description);
            param.Add("@Status", (int)this.Status);

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public RequisitionForm Delete()
        {
            this.DBHandler.Execute<Int32>(
                CRUD.DELETE,
                "DELETE FROM RequisitionForm WHERE RequisitionID = " + this.RequisitionID);
            return this;
        }
    }
}