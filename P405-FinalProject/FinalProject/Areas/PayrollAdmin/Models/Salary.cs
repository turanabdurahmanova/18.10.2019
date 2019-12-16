using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Salary
    {
        public int Id { get; set; }

        public decimal SalaryAmount { get; set; }

        public virtual Position Position { get; set; }
        public int PositionId { get; set; }

        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

    }
}
