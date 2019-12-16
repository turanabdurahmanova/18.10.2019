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
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static FinalProject.Areas.PayrollAdmin.Utilities.Utilities;

namespace FinalProject.Areas.PayrollAdmin.Controllers
{
    [Area("PayrollAdmin")]
    public class ShopController : Controller
    {
        private readonly PayrollDbContext _context;

        public ShopController(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List(int page = 1)
        {
            int take = 12;

            ShopPaginationViewModel data = new ShopPaginationViewModel
            {
                Shops = await _context.Shops.OrderBy(s => s.Id)
                                        .Skip((page - 1) * take)
                                          .Take(take).ToListAsync(),

                PaginationModel = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = take,
                    TotalItems = _context.Shops.Count()
                },

                Componies = new SelectList(await _context.Companies.ToListAsync(), "Id", "Name")

            };
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(string name, int companyId)
        {
            Shop data = new Shop
            {
                CompanyId = companyId,
                Name = name
            };

            await _context.Shops.AddAsync(data);
            await _context.SaveChangesAsync();

            return Json(new { message = 200 });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var data = await _context.Shops.Where(c => c.Id == id).FirstOrDefaultAsync();

                if (data != null)
                {
                    return Json(new { message = 200, shopDb = data });
                }
            }
            return Json(new { message = 400 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, string name)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var shopDb = await _context.Shops.Include(c => c.Company).Where(c => c.Id == id).FirstOrDefaultAsync();

            shopDb.Name = name;
            shopDb.CompanyId = shopDb.CompanyId;

            await _context.SaveChangesAsync();

            return Json(new { message = 200 });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var data = await _context.Shops.FindAsync(id);

                if (data != null)
                {
                    return Json(new { shopDb = data, message = 200 });
                }
            }

            return Json(new { message = 400 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var data = await _context.Shops.FindAsync(id);

            if (data != null)
            {
                _context.Shops.Remove(data);

                await _context.SaveChangesAsync();

                return Json(new { shopDb = data, message = 200 });
            }

            return Json(new { message = 400 });
        }

        public async Task<IActionResult> ShopProfitList(DateTime date)
        {
            date = DateTime.Now;

            var data = await _context.ShopBonus.Include(sb => sb.Shop).Include(sb => sb.Shop.Company)
                                                    .Include(sb => sb.Shop.Recruitments)
                                                        .Select(s => new ShopProfitListViewModel()
                                                        {
                                                            Shop = J(s.Shop, s.Shop.Company, s.Shop.Recruitments.ToList()),
                                                            ShopProfits = s.Shop.ShopProfits.Where(sp => sp.Date.Month == date.Month).Sum(sp => sp.Profit),
                                                            MinBonusAmount = s.MinAmount,
                                                            MaxBonusAmount = s.MaxAmount,
                                                            PromotionAmount = s.PromotionAmount,
                                                        }).ToListAsync();

            return View(data);
        }

        public Shop J(Shop shop, Company company, List<Recruitment> recruitments)
        {
            shop.Company = company;
            shop.Recruitments = recruitments;
            return shop;
        }

    }
}