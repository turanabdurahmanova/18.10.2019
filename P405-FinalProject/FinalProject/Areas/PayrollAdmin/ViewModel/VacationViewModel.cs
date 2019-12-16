using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class VacationViewModel
    {
        public IEnumerable<Recruitment> Recruitments { get; set; }
        public PaginationModel PaginationModel { get; set; }
        public Vacation Vacation { get; set; }

    }
}
