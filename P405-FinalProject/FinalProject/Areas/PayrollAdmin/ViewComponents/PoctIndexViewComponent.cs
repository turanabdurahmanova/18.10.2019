using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.Models;
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
    public class PoctIndexViewComponent : ViewComponent
    {
        private readonly PayrollDbContext _context;

        public PoctIndexViewComponent(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(PoctIndex poctIndex)
        {
            SelectListItemViewModel data = new SelectListItemViewModel
            {
                SelectListItemsPoctIndex = await _context.PoctIndex.Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()

                }).ToListAsync(),

                PoctIndex = poctIndex
            };

            return View(data);
        }
    }
}
