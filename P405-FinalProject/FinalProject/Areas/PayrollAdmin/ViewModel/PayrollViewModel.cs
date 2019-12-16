using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class PayrollViewModel
    {
        public Payroll Payrolls { get; set; }
        public Payroll Payroll { get; set; }
        public IEnumerable<Recruitment> Recruitments { get; set; }
    
    }
}
