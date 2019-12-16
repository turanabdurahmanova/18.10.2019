using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Vacation
    {
        public int Id { get; set; }

        public virtual Recruitment Recruitment { get; set; }
        public int RecruitmentId { get; set; }

        [Column(TypeName = "date")]
        public DateTime WhenStarted { get; set; }

        [Column(TypeName = "date")]
        public DateTime WhenLeft { get; set; }

    }
}
