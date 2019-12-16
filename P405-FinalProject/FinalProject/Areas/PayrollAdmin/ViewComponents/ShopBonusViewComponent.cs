using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewComponents
{
    public class ShopBonusViewComponent : ViewComponent
    {
        private readonly PayrollDbContext _context;

        public ShopBonusViewComponent(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            SelectListItemViewModel viewModel = new SelectListItemViewModel
            {
                SelectListShop = await _context.Shops.Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                }).ToListAsync()
            };

            return View(viewModel);
        }
    }
}
