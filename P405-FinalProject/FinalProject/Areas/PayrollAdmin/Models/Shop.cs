using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Shop
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string Name { get; set; }

        public virtual Company Company { get; set; }
        public int CompanyId { get; set; }

        public bool IsHead { get; set; }

        public virtual ICollection<Recruitment> Recruitments { get; set; }
        public virtual ICollection<ShopProfit> ShopProfits { get; set; }
        public virtual ICollection<ShopBonus> ShopBonus { get; set; }

    }
}
