using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class RecruitmentViewModel
    {
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }
        public List<string> Roles { get; set; }
        public string Role { get; set; }
    }
}
