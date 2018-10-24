using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    using System.Data;

    using WebApplication1.Models.Supers;

    public enum StatusType
    {
        Active = 1,
        Inactive = 2
    }

    public class Employee : ProfiledObject
    {
        public int EmployeeID { get; set; }
        public DateTime EmploymentDate { get; set; }
        public DateTime DateInactive { get; set; }
        public StatusType Status { get; set; }
        public Department Department { get; set; }
        public Branch Branch { get; set; }
        public string Code { get; set; }
        public string Position { get; set; }

        public Employee()
        {}

        public Employee(int ProfileID)
            : base(ProfileID)
        {}

        public override ProfiledObject FindProfile(int TableID, bool recursive = true, bool byPrimary = false)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT * FROM Employee WHERE " + (byPrimary ? " EmployeeID " : " Profile ") + " = " + TableID))
            {
                DataRow row = dt.Rows[0];
                
                // Fill Employee Object
                this.EmployeeID = Int32.Parse(row["EmployeeID"].ToString());
                this.Status = (StatusType)Enum.Parse(typeof(StatusType), row["Status"].ToString(), true);
                this.Code = row["Code"].ToString();
                this.Position = row["Position"].ToString();
                this.EmploymentDate = DateTime.Parse(row["EmploymentDate"].ToString());

                if (this.Status == StatusType.Inactive)
                    this.DateInactive = DateTime.Parse(row["DateInactive"].ToString());
                else
                    this.DateInactive = DateTime.MaxValue;

                if (recursive)
                {
                    this.Profile = new Profile(Int32.Parse(row["Profile"].ToString()));
                    try
                    {
                        this.Department = new Department(Int32.Parse(row["Department"].ToString()), recursive:false);
                    }
                    catch (Exception e)
                    {
                        this.Department = null;
                    }

                    try
                    {
                        this.Branch = new Branch(Int32.Parse(row["Branch"].ToString()), recursive: false);
                    }
                    catch (Exception e)
                    {
                        this.Branch = null;
                    }
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

                string columns = "INSERT INTO [dbo].[Employee](Profile, Department, EmploymentDate, DateInactive, Status, Code, Position)";
                string values = "OUTPUT INSERTED.EmployeeID VALUES(@Profile, @Department, @EmploymentDate, @DateInactive, @Status, @Code, @Position)";
                Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

                param.Add("@Profile", this.Profile.ProfileID);
                param.Add("@Department", this.Department.DepartmentID);
                param.Add("@EmploymentDate", this.EmploymentDate.ToString("yyyy-MM-dd"));
                param.Add("@DateInactive", this.DateInactive.ToString("yyyy-MM-dd"));
                if (this.Department == null)
                    param.Add("@Department", null);
                else 
                    param.Add("@Department", this.Department.DepartmentID);
                param.Add("@Status", (int)this.Status);
                param.Add("@Code", Code);
                param.Add("@Position", Position);
                if (this.Branch == null)
                    param.Add("@Branch", null);
                else
                    param.Add("@Branch", this.Branch.BranchID);

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

            string set = "UPDATE Employee SET EmploymentDate = @EmploymentDate, "
                         + "DateInactive = @DateInactive, Status = @Status, Department = @Department OUTPUT INSERTED.EmployeeID WHERE EmployeeID = " + EmployeeID;

            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@EmploymentDate", this.EmploymentDate.ToString("yyyy-MM-dd"));
            param.Add("@DateInactive", this.DateInactive.ToString("yyyy-MM-dd"));
            param.Add("@Status", (int)this.Status);
            param.Add("@Code", Code);
            param.Add("@Position", Position);

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