using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Bonus
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public virtual Recruitment Recruitment { get; set; }
        public int? RecruitmentId { get; set; }

        [Required]
        public string Reason { get; set; }

        [DataType(DataType.Date)]
        public DateTime Date { get; set; }

    }
}
