using FinalProject.Areas.PayrollAdmin.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Continuity
    {
        public int Id { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [EnumDataType(typeof(Permission))]
        public Permission PermissionType { get; set; }

        public virtual Recruitment Recruitment { get; set; }
        public int RecruitmentId { get; set; }

        public string Reason { get; set; }

    }
}
