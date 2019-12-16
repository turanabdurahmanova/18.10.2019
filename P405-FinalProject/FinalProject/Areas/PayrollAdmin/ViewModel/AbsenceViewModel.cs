using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class AbsenceViewModel
    {
        public Employee Employee { get; set; }
        public Continuity Continuity { get; set; }
        public IEnumerable<Continuity> Continuities { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Recruitment> Recruitments { get; set; }
    }
}
