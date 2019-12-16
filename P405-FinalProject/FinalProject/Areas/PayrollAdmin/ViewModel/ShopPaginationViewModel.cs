using FinalProject.Areas.PayrollAdmin.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class ShopPaginationViewModel
    {
        public PaginationModel PaginationModel { get; set; }

        public SelectList Componies { get;set; }

        public Shop Shop { get; set; }

        public IEnumerable<Shop> Shops { get; set; }

        public ShopProfit ShopProfit { get; set; }
    }
}
