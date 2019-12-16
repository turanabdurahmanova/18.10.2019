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
    public class PenaltyController : Controller
    {
        private readonly PayrollDbContext _context;

        public PenaltyController(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List(int page = 1)
        {
            int take = 6;

            PenaltyViewModel data = new PenaltyViewModel
            {

                Penalties = await _context.Penalties.Where(b => b.Date.Month == DateTime.Now.Month).Include(b => b.Recruitment).Include(r => r.Recruitment.Employee)
                                                                        .Include(r => r.Recruitment.Position)
                                                                        .Include(r => r.Recruitment.Shop)
                                                                        .OrderBy(r => r.Recruitment.Employee.Firstname)
                                                                                .Skip((page - 1) * take)
                                                                                    .Take(take)
                                                                                         .ToListAsync(),


                PaginationModel = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = take,
                    TotalItems = _context.Penalties.Count()
                }
            };
            
            return View(data);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int page = 3)
        {
            int take = 6;

            PenaltyViewModel bonusViewModel = new PenaltyViewModel
            {
                Recruitments = await _context.Recruitments.Where(r => r.Bonus.Count() == 0).Include(e => e.Employee).Include(p => p.Position).Include(s => s.Shop)
                                                                                                          .OrderBy(r => r.Employee.Firstname)
                                                                                                            .Skip((page - 1) * take)
                                                                                                              .Take(take)
                                                                                                               .ToListAsync(),

                PaginationModel = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = take,
                    TotalItems = _context.Recruitments.Where(r => r.Bonus.Count() == 0).Count()
                }

            };

            return View(bonusViewModel);
        }

        [HttpPost]
        public IActionResult Create(int id, decimal amount, string reason)
        {
            var penalty = new Penalty
            {
                RecruitmentId = id,
                Amount = amount,
                Reason = reason,
                Date = DateTime.Now
            };

            _context.Penalties.Add(penalty);
            _context.SaveChanges();

            return Json(new { status = 200, message = "Cərimə əlavə edildi..." });
        }

        [HttpGet]
        public async Task<JsonResult> Edit(int? id)
        {
            if (id != null)
            {
                var data = await _context.Penalties.LastOrDefaultAsync(e => e.RecruitmentId == id);

                if (data != null)
                {
                    return Json(new { bonusDb = data, message = 200 });
                }
            }

            return Json(new { message = 400 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, decimal amount, string reason)
        {
            var data = await _context.Penalties.LastOrDefaultAsync(e => e.RecruitmentId == id);

            if (data == null) return View();

            if (data != null)
            {
                data.Amount = amount;
                data.Reason = reason;

                await _context.SaveChangesAsync();

                return Json(new { bonusDb = data, message = 200 });
            }

            return Json(new { message = 400 });
        }

    }
}