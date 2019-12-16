using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Penalty
    {
        public int Id { get; set; }

        public decimal Amount { get; set; }

        public virtual Recruitment Recruitment { get; set; }
        public int? RecruitmentId { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        public string Reason { get; set; }
    }
}
