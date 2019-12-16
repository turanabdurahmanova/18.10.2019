using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class RecruitmentShopBonus
    {
        public int Id { get; set; }
        public Recruitment Recruitment { get; set; }
        public int RecruitmentId { get; set; }

        public decimal ShopBonusAmount { get; set; }

        public DateTime Date { get; set; }

    }
}
