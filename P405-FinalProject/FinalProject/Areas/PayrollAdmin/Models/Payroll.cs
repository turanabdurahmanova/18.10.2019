using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Payroll
    {
        public int Id { get; set; }

        public virtual Recruitment Recruitment { get; set; }
        public int RecruitmentId { get; set; }

        public DateTime Date { get; set; }

        public decimal TotalSalary { get; set; }

    }
}
