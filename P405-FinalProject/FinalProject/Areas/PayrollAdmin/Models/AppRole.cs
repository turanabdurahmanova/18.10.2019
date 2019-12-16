using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class AppRole : IdentityRole
    {
        public List<AppUserRole> AppUserRoles { get; set; }
    }
}
