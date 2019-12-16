using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class FormerWork
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string WorkName { get; set; }

        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        //[Column(TypeName = "date")]
        //public DateTime WhenStarted { get; set; }

        [Required, DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime WhenStarted { get; set; }

        [Required, DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime WhenLeft { get; set; }

        [Required, StringLength(250)]
        public string WhyLeftReason { get; set; }
    }
}
