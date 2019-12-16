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
    [ViewComponent(Name = "Employee")]
    public class EmployeeComponent : ViewComponent
    {
        private readonly PayrollDbContext _context;

        public EmployeeComponent(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IViewComponentResult> InvokeAsync(Position position, Shop shop)
        {
            SelectListItemViewModel viewModel = new SelectListItemViewModel
            {
                SelectListCompanies = await _context.Companies.Select(d => new SelectListItem
                {
                    Text = d.Name,
                    Value = d.Id.ToString()
                }).ToListAsync()
            };

            return View(viewModel);
        }
    }
}
