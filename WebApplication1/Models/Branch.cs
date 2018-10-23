using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    using System.Data;

    public enum BranchType
    {
        None = 0,
        CorporateServices = 1,
        Administration = 2,
        SalesMarketing = 3,
        Operations = 4,
        Finance = 5
    }

    public class Branch
    {
        private DBHandler DBHandler;
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public BranchType Type { get; set; }
        public Employee BranchVP { get; set; }

        public Branch(int BranchID = -1, bool recursive = true, bool byPrimary = true)
        {
            this.DBHandler = new DBHandler();

            if (BranchID != -1)
            {
                this.BranchID = BranchID;
                this.Find(BranchID, recursive, byPrimary);
            }
        }

        public Branch Find(int BranchID, bool recursive = true, bool byPrimary = true)
        {
            using (DataTable dt =
                this.DBHandler.Execute<DataTable>(CRUD.READ, "SELECT * FROM Branch WHERE " + (byPrimary ? "BranchID" : "Type") + " = " + BranchID))
            {
                DataRow row = dt.Rows[0];

                this.BranchID = Int32.Parse(row["BranchID"].ToString());
                this.BranchName = row["BranchName"].ToString();
                this.Type = (BranchType)Int32.Parse(row["Type"].ToString());

                if (recursive)
                {
                    this.BranchVP = (Employee)new Employee().FindProfile(Int32.Parse(row["BranchVP"].ToString()), byPrimary:true);
                }
            }
            return this;
        }

        public Branch Create()
        {
            string columns = "INSERT INTO Branch(BranchName, BranchVP, Type) OUTPUT INSERTED.BranchID";
            string values = " VALUES(@BranchName, @BranchVP, @Type)";
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@BranchName", this.BranchName);
            param.Add("@BranchVP", this.BranchVP.EmployeeID);
            param.Add("@Type", (Int32)this.Type);

            this.BranchID = this.DBHandler.Execute<Int32>(CRUD.CREATE, columns + values, param);
            return this;
        }

        public Branch Update(bool recursive = true)
        {
            return this.Update(this.BranchID, recursive);
        }

        public Branch Update(int BranchID, bool recursive = true)
        {
            string set =
                "UPDATE Branch SET BranchName = @BranchName AND "
                + "BranchVP = @BranchVP AND Type = @Type WHERE BranchID = " + BranchID;
            Dictionary<string, dynamic> param = new Dictionary<string, dynamic>();

            param.Add("@BranchName", this.BranchName);
            param.Add("@BranchVP", this.BranchVP.EmployeeID);
            param.Add("@Type", (Int32)this.Type);

            this.DBHandler.Execute<Int32>(CRUD.UPDATE, set, param);
            return this;
        }

        public Branch Delete()
        {
            this.DBHandler.Execute<Int32>(CRUD.DELETE, "DELETE FROM Branch WHERE BranchID = " + this.BranchID);
            return this;
        }
    }
}