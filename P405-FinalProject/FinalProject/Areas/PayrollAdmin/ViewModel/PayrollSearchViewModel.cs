using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class PayrollSearchViewModel
    {
        public IEnumerable<Position> Positions { get; set; }
        public IEnumerable<Company> Companies { get; set; }
        public IEnumerable<Employee> Employees { get; set; }
        public IEnumerable<Payroll> Payrolls { get; set; }
        public IEnumerable<Recruitment> Recruitments { get; set; }
        public PaginationModel PaginationModel { get; set; }

    }
}
