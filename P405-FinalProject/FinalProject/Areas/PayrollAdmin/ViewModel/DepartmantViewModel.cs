using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class DepartmantViewModel
    {
        public IEnumerable<Departmant> Departmants { get; set; }
        public PaginationModel PaginationModel { get; set; }
    }
}
