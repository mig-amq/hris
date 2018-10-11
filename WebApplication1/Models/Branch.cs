using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public enum BranchType
    {
        CorporateServices = 0,
        Administration = 1,
        SalesMarketing = 2,
        Operations = 3,
        Finance = 4
    }

    public class Branch
    {
        private Account Head { get; set; }
        private BranchType Type { get; set; }
    }
}