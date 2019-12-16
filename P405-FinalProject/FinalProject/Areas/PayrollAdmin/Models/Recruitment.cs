using FinalProject.Areas.PayrollAdmin.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Recruitment
    {
        public int Id { get; set; }

        public virtual Shop Shop { get; set; }
        public int? ShopId { get; set; }

        public virtual Position Position { get; set; }
        public int PositionId { get; set; }

        public virtual Employee Employee { get; set; }
        public int EmployeeId { get; set; }

        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime WhenStarted { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        [Required]
        public DateTime WhenLeft { get; set; }

        public virtual ICollection<Continuity> Continuities { get; set; }

        public virtual ICollection<Payroll> Payrolls { get; set; }

        public ICollection<Penalty> Penalties { get; set; }

        public ICollection<Bonus> Bonus { get; set; }

        public ICollection<Vacation> Vacations { get; set; }

      
    }
}
