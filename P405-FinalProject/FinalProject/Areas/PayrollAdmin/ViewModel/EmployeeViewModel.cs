using FinalProject.Areas.PayrollAdmin.Enum;
using FinalProject.Areas.PayrollAdmin.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class EmployeeViewModel
    {
        public IEnumerable<Employee> Employees { get; set; }

        public Employee Employee { get; set; }

        public List<FormerWork> FormerWorks { get; set; }

        public Recruitment Recruitment { get; set; }

        public IEnumerable<Recruitment> Recruitments { get; set; }
        public FormerWork FormerWork { get; set; }

    }
}
