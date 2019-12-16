using FinalProject.Areas.PayrollAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class SelectListItemViewModel
    {
        public List<SelectListItem> SelectListCompanies { get; set; }
        public List<SelectListItem> SelectListDepartmans { get; set; }
        public List<SelectListItem> SelectListShop { get; set; }
        public List<SelectListItem> SelectListEmployeeFirstName { get; set; }
        public List<SelectListItem> SelectListEmployeeLastName { get; set; }
        public List<SelectListItem> SelectListPositions { get; set; }
        public List<SelectListItem> SelectListEmployee { get; set; }
        public List<SelectListItem> SelectListItemsPoctIndex { get; set; }
        public Company Company { get; set; }
        public Departmant Departmant { get; set; }
        public Employee Employee { get; set; }
        public Position Position { get; set; }
        public PoctIndex PoctIndex { get; set; }
        public Salary Salary { get; set; }
        public Shop Shop { get; set; }

    }
}
