using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.Models;
using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.PayrollAdmin.Controllers
{
    [Area("PayrollAdmin")]
    public class VacationController : Controller
    {
        private readonly PayrollDbContext _context;

        public VacationController(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List(int page = 3)
        {
            int take = 6;

            VacationViewModel data = new VacationViewModel
            {
                Recruitments = await _context.Recruitments.Where(r => r.WhenLeft == new DateTime(0001, 01, 01)).Where(r => r.Vacations.Count() == 0).Include(r => r.Employee).Include(r => r.Position)
                                                            .Include(r => r.Shop.Company)
                                                                .OrderBy(r => r.Employee.Firstname)
                                                                     .Skip((page - 1) * take)
                                                                        .Take(take).ToListAsync(),

                PaginationModel = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = take,
                    TotalItems = _context.Recruitments.Where(r=>r.Vacations.Count()==0).Count()
                },

                Vacation = new Vacation
                {
                    WhenStarted = DateTime.Now,
                }
            };

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, DateTime whenStarted, DateTime whenLeft)
        {
            Vacation data = new Vacation
            {
                RecruitmentId = id,
                WhenStarted = whenStarted,
                WhenLeft = whenLeft
            };

            await _context.Vacations.AddAsync(data);
            await _context.SaveChangesAsync();

            return Json(new { message = 200 });
        }

    }
}