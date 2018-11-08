using System;
using System.Collections.Generic;

namespace WebApplication1.Models
{
    using System.Data;
    using System.Diagnostics;
    using System.Web.UI.WebControls;

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
        public DateTime? DateInactive { get; set; }
        public StatusType Status { get; set; }
        public Department Department { get; set; }
        public string Code { get; set; }
        public string Position { get; set; }

        public Employee()
        {}

        public Employee(int ProfileID, bool byPrimary = false)
            : base(ProfileID, byPrimary)
        {}

        public override ProfiledObject FindProfile(int TableID, bool recursive = true, bool byPrimary = false)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT * FROM Employee WHERE " + (byPrimary ? " EmployeeID " : " Profile ") + " = " + TableID))
            {
                if (dt.Rows.Count > 0)
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
                        this.DateInactive = null;

                    if (recursive)
                    {
                        this.Profile = new Profile(Int32.Parse(row["Profile"].ToString()));
                        try
                        {
                            this.Department = new Department(Int32.Parse(row["Department"].ToString()), recursive: true);
                        }
                        catch (Exception e)
                        {
                            this.Department = null;
                        }
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

        public Employee Create(int DepartmentID = -1, bool recursive = false)
        {
            if (this.Profile != null && recursive)
            {
                this.Profile.Create();
            }

            this.Department = new Department();
            if (DepartmentID != -1)
            {
                this.Department.Find(DepartmentID, byPrimary: false);
            }
            else
            {
                this.Department.Find((int)DepartmentType.None, false, false);
            }

            string columns = "INSERT INTO [dbo].[Employee](Profile, Department, EmploymentDate, DateInactive, Status, Code, Position)";
            string values = "OUTPUT INSERTED.EmployeeID VALUES(@Profile, @Department, @EmploymentDate, @DateInactive, @Status, @Code, @Position)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            if (this.Profile != null)
                param.Add("@Profile", this.Profile.ProfileID);

            param.Add("@EmploymentDate", this.EmploymentDate.ToString("yyyy-MM-dd"));
            if (this.DateInactive.HasValue)
                param.Add("@DateInactive", this.DateInactive.Value.ToString("yyyy-MM-dd"));
            else
                param.Add("@DateInactive", DBNull.Value);

            param.Add("@Department", this.Department.DepartmentID);
            param.Add("@Status", (int)this.Status);
            param.Add("@Code", Code);
            param.Add("@Position", Position);

            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM Employee WHERE Code = @Code",
                param))
            {
                if (dt.Rows.Count > 0)
                {
                    throw new Exception("That emplpoyee code is already taken");
                }
                else
                {
                    this.EmployeeID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
                }
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
                         + "DateInactive = @DateInactive, Status = @Status, Department = @Department, Position = @Position"
                         + " OUTPUT INSERTED.EmployeeID WHERE EmployeeID = " + EmployeeID;

            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@EmploymentDate", this.EmploymentDate.ToString("yyyy-MM-dd"));
            if (this.DateInactive.HasValue)
                param.Add("@DateInactive", this.DateInactive.Value.ToString("yyyy-MM-dd"));
            else
                param.Add("@DateInactive", DBNull.Value);
            param.Add("@Status", (int)this.Status);
            param.Add("@Code", Code);
            param.Add("@Position", Position);

            // Update Department ID as well because Departments can't be updated through the Employee
            // yet the Employee can CHANGE Departments, unlike other Foreign Keys, the relationship
            // of Employee and Department is Many-to-One
            param.Add("@Department", this.Department.DepartmentID);

            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM Employee WHERE Code = @Code",
                param))
            {
                if (dt.Rows.Count >= 2)
                {
                    throw new Exception("That emplpoyee code is already taken");
                }
                else
                {
                    this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
                }
            }
            return this;
        }

        public Employee Delete(bool recursive = false)
        {
            this.DBHandler.Execute<Int32>(CRUD.DELETE, "DELETE FROM Employee WHERE EmployeeID = " + this.EmployeeID);

            if (recursive)
                this.Profile.Delete();
            return this;
        }

        public Boolean OnLeave(DateTime Date)
        {
            int m = Date.Month;
            int d = Date.Day;
            int y = Date.Year;

            string sql = "SELECT COUNT(E.EmployeeID) AS Count " + "FROM Employee E INNER JOIN Leave L ON L.Employee = E.EmployeeID "
                                     + " WHERE L.Status = " + ((int)LeaveStatus.Approved) + " AND EmployeeID = "
                                     + this.EmployeeID + " AND MONTH(L.StartDate) >= " + m
                                     + " AND YEAR(L.StartDate) >= " + y + " AND DAY(L.StartDate) >= " + d + " AND " + m
                                     + " <= MONTH(L.EndDate) AND " + y + " <= Year(L.EndDate) AND " + d
                                     + " <= DAY(L.EndDate)";
            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, sql))
            {
                return dt.Rows[0]["Count"].ToString().ToLower() != "0";
            }
        }

        public int GetNumLeaves(DateTime Date)
        {
            string sql = "SELECT COUNT(*) AS Count FROM Leave WHERE Status = " + ((int)LeaveType.Paid) + " AND Employee = " + this.EmployeeID
                         + " AND MONTH(StartDate) = "
                         + Date.Month
                         + " AND YEAR(StartDate) = "
                         + Date.Year;

            using (DataTable dt = this.DBHandler.Execute<DataTable>(CRUD.READ, sql))
            {
                return Int32.Parse(dt.Rows[0]["Count"].ToString());
            }

        } 
    }
}