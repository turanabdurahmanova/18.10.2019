using FinalProject.Areas.PayrollAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class ShopProfitListViewModel
    {
        public Shop Shop { get; set; }
        public ShopBonus ShopBonus { get; set; }
        public decimal ShopProfits { get; set; }
        public decimal MinBonusAmount { get; set; }
        public decimal MaxBonusAmount { get; set; }
        public DateTime Date { get; set; }
        public decimal PromotionAmount { get; set; }
    }
}
