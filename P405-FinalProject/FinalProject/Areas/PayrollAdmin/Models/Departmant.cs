using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Departmant
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        public virtual ICollection<CompanyDepartament> CompanyDepartaments { get; set; }

        public virtual ICollection<Position> Positions { get; set; }

        //public virtual Company Company { get; set; }
        //public int? CompanyId { get; set; }

        //[Required, StringLength(50)]
        //public string DepartmantHead { get; set; }
    }
}
