using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Position
    {

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        public virtual Departmant Departmant { get; set; }
        public int DepartmantId { get; set; }

        public virtual ICollection<Recruitment> Recruitments { get; set; }

        public virtual ICollection<Salary> Salaries { get; set; }

    }
}
