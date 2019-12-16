using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Enum
{
    public enum Education
    {
        [Display(Name = "Bakalavr", Order = 0)]
        Bakalavr,
        [Display(Name = "Magistr", Order = 1)]
        Magistr,
        Yoxdur,
        Texnikum
    }
}
