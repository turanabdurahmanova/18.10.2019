using FinalProject.Areas.PayrollAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class CompanyCreateViewModel
    {
        public Company Company { get; set; }

        public List<SelectListItem> SelectListPonctIndex { get; set; }
    }
}
