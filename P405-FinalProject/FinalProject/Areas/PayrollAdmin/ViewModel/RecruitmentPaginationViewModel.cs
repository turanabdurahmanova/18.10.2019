using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class RecruitmentPaginationViewModel
    {
        public PaginationModel PaginationModel { get; set; }

        public IEnumerable<Recruitment> Recruitments { get; set; }

        public Recruitment Recruitment { get; set; }

        public List<RecruitmentViewModel> RecruitmentList { get; set; }
    }
}
