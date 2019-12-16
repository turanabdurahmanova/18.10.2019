using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.Models;
using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.PayrollAdmin.Controllers
{
    [Area("PayrollAdmin")]
    [Authorize(Roles = "Department Head, Admin")]
    public class BonusController : Controller
    {
        private readonly PayrollDbContext _context;
        private readonly UserManager<AppUser> userManager;

        public BonusController(PayrollDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            this.userManager = userManager;
        }

        public async Task<IActionResult> List()
        {
            if (User.IsInRole("Admin"))
            {
                var dataAdmin = await _context.Bonus.Where(b => b.Date.Month == DateTime.Now.Month)
                                                                        .Include(b => b.Recruitment)
                                                                             .Include(r => r.Recruitment.Employee)
                                                                                 .Include(r => r.Recruitment.Position)
                                                                                     .Include(r => r.Recruitment.Shop)
                                                                                         .OrderBy(r => r.Recruitment.Employee.Firstname)
                                                                                              .ToListAsync();
                return View(dataAdmin);
            }
            var user = await userManager.FindByNameAsync(User.Identity.Name);

            var rec = await _context.Recruitments.Include(r => r.Position).FirstOrDefaultAsync(r => r.WhenLeft == new DateTime(0001, 01, 01) && r.EmployeeId == user.EmployeeId);
            var data = await _context.Bonus.Where(b => b.Date.Month == DateTime.Now.Month && b.Recruitment.Position.DepartmantId == rec.Position.DepartmantId)
                                                .Include(b => b.Recruitment)
                                                     .Include(r => r.Recruitment.Employee)
                                                          .Include(r => r.Recruitment.Position)
                                                               .Include(r => r.Recruitment.Shop)
                                                                     .OrderBy(r => r.Recruitment.Employee.Firstname)
                                                                        .ToListAsync();
            return View(data);
        }

        [Authorize(Roles = "Department Head,Admin")]
        public async Task<IActionResult> Create()
        {
            var user = await userManager.FindByNameAsync(User.Identity.Name);
            var rec = await _context.Recruitments.Include(r => r.Position).FirstOrDefaultAsync(r => r.WhenLeft == new DateTime(0001, 01, 01) && r.EmployeeId == user.EmployeeId);

            var data = await _context.Recruitments.Include(r => r.Bonus).Where(r => !r.Bonus.Any(b => b.Date.Year == DateTime.Now.Year && b.Date.Month == DateTime.Now.Month)).Where(r => r.Position.DepartmantId == rec.Position.DepartmantId)
                                                                            .Include(r => r.Employee)
                                                                                .Include(p => p.Position)
                                                                                    .Include(s => s.Shop)
                                                                                        .OrderBy(r => r.Employee.Firstname)
                                                                                             .ToListAsync();
            return View(data);
        }

        [HttpPost]
        [Authorize(Roles = "Department Head,Admin")]
        public IActionResult Create(int id, decimal amount, string reason)
        {
            var bonus = new Bonus
            {
                RecruitmentId = id,
                Amount = amount,
                Reason = reason,
                Date = DateTime.Now
            };

            _context.Bonus.Add(bonus);
            _context.SaveChanges();

            return Json(new { status = "200", message = "Bonus əlavə edildi..." });
        }

        [HttpGet]
        [Authorize(Roles = "Department Head, Admin")]
        public async Task<JsonResult> Edit(int? id)
        {
            if (id != null)
            {
                var data = await _context.Bonus.FirstOrDefaultAsync(b => b.RecruitmentId == id && b.Date.Month == DateTime.Now.Month);

                if (data != null)
                {
                    return Json(new { bonusDb = data, message = 200 });
                }
            }

            return Json(new { message = 400 });
        }

        [Authorize(Roles = "Department Head, Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, decimal amount, string reason)
        {
            var data = await _context.Bonus.LastOrDefaultAsync(e => e.RecruitmentId == id);

            if (data != null)
            {
                data.Amount = amount;
                data.Reason = reason;

                await _context.SaveChangesAsync();

                return Json(new { bonusDb = data, message = 200 });
            }

            return Json(new { message = 400 });
        }

        [HttpGet]
        public async Task<IActionResult> ShopBonus()
        {
            ShopBonusViewModel data = new ShopBonusViewModel
            {
                Shops = new SelectList(await _context.Shops.ToListAsync(), "Id", "Name")
            };

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ShopBonus(ShopBonusViewModel shopBonusViewModel)
        {
            if (!ModelState.IsValid) return View(shopBonusViewModel);

            ShopBonus data = new ShopBonus
            {
                ShopId = shopBonusViewModel.ShopId,
                MaxAmount = shopBonusViewModel.ShopBonus.MaxAmount,
                MinAmount = shopBonusViewModel.ShopBonus.MinAmount,
                PromotionAmount = shopBonusViewModel.ShopBonus.PromotionAmount,
                WhenLeft = shopBonusViewModel.ShopBonus.WhenLeft,
                WhenStarted = shopBonusViewModel.ShopBonus.WhenStarted,
            };

            _context.ShopBonus.Add(data);
            _context.SaveChanges();

            return RedirectToAction("ShopProfitList", "Shop");
        }

    }
}