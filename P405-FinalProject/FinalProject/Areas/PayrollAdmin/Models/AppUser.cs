using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class AppUser: IdentityUser
    {
        public Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        public List<AppUserRole> AppUserRoles { get; set; }
    }
}
