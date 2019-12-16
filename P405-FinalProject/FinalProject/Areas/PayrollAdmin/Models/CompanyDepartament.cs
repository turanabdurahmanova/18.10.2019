using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class CompanyDepartament
    {
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }

        public virtual Departmant Departmant { get; set; }
        public int DepartmantId { get; set; }
    }
}
