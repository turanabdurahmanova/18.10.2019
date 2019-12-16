using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class ShopBonus
    {
        public int Id { get; set; }

        public virtual Shop Shop { get; set; }
        public int ShopId { get; set; }

        [Required]
        public decimal PromotionAmount { get; set; }

        [Required]
        public decimal MinAmount { get; set; }

        [Required]
        public decimal MaxAmount { get; set; }

        [Column(TypeName = "date")]
        public DateTime WhenStarted { get; set; }

        [Column(TypeName = "date")]
        public DateTime WhenLeft { get; set; }

    }
}
