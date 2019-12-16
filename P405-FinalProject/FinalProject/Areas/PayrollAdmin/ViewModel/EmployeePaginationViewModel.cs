using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class EmployeePaginationViewModel
    {
        public PaginationModel PaginationModel { get; set; }

        public IEnumerable<Employee> Employees { get; set; }

        public Employee Employee { get; set; }
    }
}
