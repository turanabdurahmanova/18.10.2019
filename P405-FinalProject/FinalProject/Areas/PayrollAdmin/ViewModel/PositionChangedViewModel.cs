using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class PositionChangedViewModel
    {
        public virtual Shop Shop { get; set; }
        public int? ShopId { get; set; }

        public virtual Position Position { get; set; }
        public int? PositionId { get; set; }

        public virtual Employee Employee { get; set; }
        public int? EmployeeId { get; set; }

        public SelectListItemViewModel SelectListItemViewModel { get; internal set; }
        public DateTime WorkerWhenLeft { get; set; }
        public DateTime WorkerNewWhenStarted { get; set; }

        [Required]
        public int newPositionId { get; set; }

        [Required]
        public int newSalary { get; set; }

        public Recruitment Recruitment { get; set; }

    }
}
