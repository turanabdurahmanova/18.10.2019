using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class AppUserRole : IdentityUserRole<string> 
    {
        public AppUser AppUser { get; set; }
        public AppRole AppRole { get; set; }
    }
}
