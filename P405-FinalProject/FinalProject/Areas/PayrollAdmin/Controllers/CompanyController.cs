using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.Extensions;
using FinalProject.Areas.PayrollAdmin.Models;
using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static FinalProject.Areas.PayrollAdmin.Utilities.Utilities;

namespace FinalProject.Areas.PayrollAdmin.Controllers
{
    [Area("PayrollAdmin")]
    [Authorize]
    public class CompanyController : Controller
    {
        private readonly PayrollDbContext _context;

        public CompanyController(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List(int page = 1)
        {
            int take = 16;
            CompanyPaginationViewModel data = new CompanyPaginationViewModel
            {
                Companies = await _context.Companies.OrderBy(c => c.Id)
                                                .Skip((page - 1) * take)
                                                .Take(take).ToListAsync(),
                PaginationModel = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = take,
                    TotalItems = _context.Companies.Count()
                }
              
            };

            return View(data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            Company company = await _context.Companies.Include(p => p.PoctIndex).Where(c => c.Id == id).FirstOrDefaultAsync();
            if (company == null) return NotFound();

            return View(company);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Company company)
        {
            if (!ModelState.IsValid)
                return View(company);

            await _context.Companies.AddAsync(company);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var data = await _context.Companies.Include(p => p.PoctIndex).Where(c => c.Id == id).FirstOrDefaultAsync();

            if (data == null) return BadRequest();

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Company company)
        {
            if (!ModelState.IsValid)
            {
                return View(company);
            }

            var companyDb = await _context.Companies.Include(p => p.PoctIndex).Where(c => c.Id == id).FirstOrDefaultAsync();

            companyDb.Name = company.Name;
            companyDb.OpenCompany = company.OpenCompany;
            companyDb.PoctIndexId = company.PoctIndexId;
            companyDb.TelNumber = company.TelNumber;
            companyDb.Email = company.Email;
            companyDb.Addres = company.Addres;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var data = await _context.Companies.FindAsync(id);

                if (data != null)
                {
                    return Json(new { companyDb = data, message = 200 });
                }
            }

            return Json(new { message = 400 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            try
            {
                var data = await _context.Companies.FindAsync(id);

                if (data != null)
                {
                    _context.Companies.Remove(data);

                    await _context.SaveChangesAsync();
                }

                return Json(new { companyDb = data, message = 200 });
            }
            catch (Exception exp)
            {
                return Json(new { error = exp.Message });
            }
        }

    }
}