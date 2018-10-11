using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class EmploymentHistory
    {
        public int EmploymentID { get; set; }
        public string Company { get; set; }
        public string Address { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeavingReason { get; set; }
        public string ContactName { get; set; }
        public string ContactNo { get; set; }

        // TODO
        public EmploymentHistory Find()
        {
            return this;
        }

        public EmploymentHistory Create()
        {
            return this;
        }

        public EmploymentHistory Update(bool recursive = true)
        {
            return this.Update(this.EmploymentID, recursive);
        }

        public EmploymentHistory Update(int EmploymentHistoryID, bool recursive = true)
        {
            return this;
        }

        public EmploymentHistory Delete()
        {
            return this;
        }
    }
}