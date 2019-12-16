using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Utilities
{
    public static class SD
    {
        public enum Roles
        {
            HR,
            Admin,
            PayrollSpecalist,
            Worker,
            DepartmentHead
        }
        public const string HR = "HR";
        public const string Admin = "Admin";
        public const string PayrollSpecalist = "PayrollSpecalist";
        public const string DepartmentHead = "DepartmentHead";
        public const string Worker = "Worker";
    }
}
