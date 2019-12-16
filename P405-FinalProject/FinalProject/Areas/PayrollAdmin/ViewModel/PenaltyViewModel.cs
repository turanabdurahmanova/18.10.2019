using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class PenaltyViewModel
    {
        public Penalty Penalty { get; set; }
        public IEnumerable<Penalty> Penalties { get; set; }
        public Recruitment Recruitment { get; set; }
        public ICollection<Recruitment> Recruitments { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public PaginationModel PaginationModel { get; set; }
    }
}
