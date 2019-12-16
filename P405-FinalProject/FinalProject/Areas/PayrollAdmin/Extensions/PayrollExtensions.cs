using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Extensions
{
    public static class PayrollExtensions
    {
        public static decimal CalcSalary(this Recruitment recruitment, decimal salary)
        {
            return recruitment.SalaryByDay(salary);
        }

        public static decimal SalaryByDay(this Recruitment recruitment, decimal salary)
        {
            return salary / (decimal)30.4;
        }
    }
}
