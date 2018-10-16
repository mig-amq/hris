using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    using System.Data;

    using WebApplication1.Models.Supers;

    public enum StatusType
    {
        Active = 0,
        Inactive = 1
    }

    public class Employee : ProfiledObject
    {
        public int EmployeeID { get; set; }
        public DateTime EmploymentDate { get; set; }
        public DateTime DateInactive { get; set; }
        public StatusType Status { get; set; }
        public Department Department { get; set; }

        public Employee()
        {}

        public Employee(int ProfileID)
            : base(ProfileID)
        {}

        public override ProfiledObject FindProfile(int TableID, bool recursive = false, bool byPrimary = false)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT * FROM Employee WHERE " + (byPrimary ? " EmployeeID " : " Profile ") + " = " + TableID))
            {
                DataRow row = dt.Rows[0];
                
                // Fill Employee Object
                this.EmployeeID = Int32.Parse(row["EmployeeID"].ToString());
                this.DateInactive = DateTime.Parse(row["DateInactive"].ToString());
                this.Status = (StatusType)Enum.Parse(typeof(StatusType), row["Status"].ToString(), true);
                
                if (recursive)
                {
                    this.Profile = new Profile(Int32.Parse(row["Profile"].ToString()));
                    this.Department = new Department(Int32.Parse(row["Department"].ToString()));
                }
            }

            return this;
        }

        public Employee Create(int DepartmentID)
        {
            if (this.Profile != null)
            {
                this.Profile.Create();
                this.Department = new Department();
                this.Department.Find(DepartmentID);

                string columns = "INSERT INTO [dbo].[Employee](Profile, Department, EmploymentDate, DateInactive, Status)";
                string values = "OUTPUT INSERTED.EmployeeID VALUES(@Profile, @Department, @EmploymentDate, @DateInactive, @Status)";
                Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

                param.Add("@Profile", this.Profile.ProfileID);
                param.Add("@Department", this.Department.DepartmentID);
                param.Add("@EmploymentDate", this.EmploymentDate.ToShortDateString());
                param.Add("@DateInactive", this.DateInactive.ToShortDateString());
                param.Add("@Status", (int)this.Status);
                
                this.EmployeeID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            }

            return this;
        }

        public Employee Update(bool recursive = true)
        {
            return this.Update(this.EmployeeID, recursive);
        }

        public Employee Update(int EmployeeID, bool recursive)
        {
            if (recursive)
            {
                this.Profile.Update(recursive);
            }

            string set = "UPDATE Employee SET EmploymentDate = @EmploymentDate AND "
                         + "DateInactive = @DateInactive AND Status = @Status AND Department = @Department WHERE EmployeeID = " + EmployeeID;

            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@EmploymentDate", this.EmploymentDate.ToShortDateString());
            param.Add("@DateInactive", this.DateInactive.ToShortDateString());
            param.Add("@Status", (int)this.Status);

            // Update Department ID as well because Departments can't be updated through the Employee
            // yet the Employee can CHANGE Departments, unlike other Foreign Keys, the relationship
            // of Employee and Department is Many-to-One
            param.Add("@Department", this.Department.DepartmentID);
            
            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public Employee Delete()
        {
            this.DBHandler.Execute<Int32>(CRUD.DELETE, "DELETE FROM Employee WHERE EmployeeID = " + this.EmployeeID);
            this.Profile.Delete();
            return this;
        }
    }
}