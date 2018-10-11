using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
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
        public Account Head { get; set; }
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

        public Department Find(int DepartmentID)
        {
            return this;
        }
    }
}