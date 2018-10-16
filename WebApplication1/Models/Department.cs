﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;

    public enum DepartmentType
    {
        // Corporate Services Branch
        SpecialProjects = 0,
        InternalAuditing = 1,
        TaxMatters = 2,
        ParaLegal = 3,

        // Administration Branch
        HumanResources = 4,
        SocialEnvironment = 5,
        GeneralService = 6,
        IT = 7,
        Security = 8,

        // Sales & Marketing
        SalesMarketing = 9,
        QualityAssurance = 10,
        Shipping = 11,
        ResearchDevelopment = 12,

        // Operations
        ProductionPlanning = 13,
        ProductionFurniture = 14,
        Logistics = 15,

        // Finance
        Accounting = 16,
        Finance = 17,
    }

    public class Department
    {
        private DBHandler DBHandler;
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public Employee DepartmentHead { get; set; }
        public Branch Branch { get; set; }
        public DepartmentType Type { get; set; }

        public Department(int DepartmentID = -1)
        {
            this.DBHandler = new DBHandler();

            if (this.DepartmentID != -1)
            {
                this.DepartmentID = DepartmentID;
                this.Find(this.DepartmentID);
            }
        }

        public Department Find(int DepartmentID, bool recursive = true)
        {
            using (DataTable dt = this.DBHandler.Execute<DataTable>(
                CRUD.READ,
                "SELECT * FROM Department WHERE DepartmentID = " + DepartmentID))
            {
                DataRow row = dt.Rows[0];

                this.DepartmentID = DepartmentID;
                this.DepartmentName = row["DepartmentName"].ToString();
                this.Type = (DepartmentType)Int32.Parse(row["Type"].ToString());

                if (recursive)
                {
                    this.Branch = new Branch(Int32.Parse(row["Branch"].ToString()));
                    this.DepartmentHead = (Employee) new Employee().FindProfile(Int32.Parse(row["DepartmentHead"].ToString()), byPrimary:true);
                }
            }
            return this;
        }

        public Department Create()
        {
            string columns = "INSERT INTO Department(DepartmentName, DepartmentHead, Branch) "
                             + "OUPUT INSERTED.DepartmentID";
            string values = " VALUES(@DepartmentName, @DepartmentHead, @Branch)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@DepartmentName", this.DepartmentName);
            param.Add("@DepartmentHead", this.DepartmentHead.EmployeeID);
            param.Add("@Branch", this.Branch.BranchID);
            param.Add("@Type", (Int32)this.Type);

            this.DepartmentID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public Department Update(bool recursive = true)
        {
            return this.Update(this.DepartmentID, recursive);
        }

        public Department Update(int DepartmentID, bool recursive = true)
        {
            if (recursive)
            {
                this.Branch.Update(recursive);
                this.DepartmentHead.Update(recursive);
            }

            string set = "UPDATE Department SET DepartmentName = @DepartmentName AND "
                         + "DepartmentHead = @DepartmentHead AND Branch = @Branch AND "
                         + "Type = @Type WHERE DepartmentID = " + this.DepartmentID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@DepartmentName", this.DepartmentName);
            param.Add("@DepartmentHead", this.DepartmentHead.EmployeeID);
            param.Add("@Branch", this.Branch.BranchID);
            param.Add("@Type", (Int32)this.Type);
            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public Department Delete()
        {
            this.DBHandler.Execute<Int32>(
                CRUD.DELETE,
                "DELETE FROM Department WHERE DepartmentID = " + this.DepartmentID);
            return this;
        }
    }
}