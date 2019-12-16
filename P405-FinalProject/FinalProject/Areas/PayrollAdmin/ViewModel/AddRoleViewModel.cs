using FinalProject.Areas.PayrollAdmin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class AddRoleViewModel
    {
        public Employee Employee { get; set; }
        public AppUser AppUser { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public SelectList Roles { get; set; }
        public string RoleId { get; set; }
        public int EmployeeId { get; set; }

    }
}
