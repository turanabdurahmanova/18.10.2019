using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.Models;
using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace FinalProject.Areas.PayrollAdmin.Controllers
{
    [Area("PayrollAdmin")]
    public class DepartamentController : Controller
    {
        private readonly PayrollDbContext _context;

        public DepartamentController(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List(int page = 1)
        {
            int take = 16;

            DepartmantViewModel data = new DepartmantViewModel
            {
                Departmants = await _context.Departmants.OrderBy(d => d.Id).
                                                            Skip((page - 1) * take)
                                                                .Take(take).ToListAsync(),

                PaginationModel = new PaginationModel()
                {
                    CurrentPage = page,
                    ItemsPerPage = take,
                    TotalItems = _context.Departmants.Count()
                }
            };

            return View(data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var data = await _context.Departmants.FindAsync(id);
            if (data == null) return NotFound();

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string name)
        {
            var data = new Departmant
            {
                Name = name
            };

            await _context.Departmants.AddAsync(data);
            await _context.SaveChangesAsync();

            return Json(new { message = 200 });
        }

        [HttpGet]
        public async Task<JsonResult> Edit(int? id)
        {
            if (id != null)
            {
                var data = await _context.Departmants.FindAsync(id);

                if (data != null)
                {
                    return Json(new { departmantDb = data, message = 200 });
                }
            }

            return Json(new { message = 400 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id, string name)
        {
            var data = await _context.Departmants.FindAsync(id);

            if (data == null) return View();

            if (data != null)
            {
                data.Name = name;

                await _context.SaveChangesAsync();

                return Json(new { departmantDb = data, message = 200 });
            }

            return Json(new { message = 400 });

        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var data = await _context.Departmants.FindAsync(id);

                if (data != null)
                {
                    return Json(new { departmantDb = data, message = 200 });
                }
            }

            return Json(new { message = 400 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var data = await _context.Departmants.FindAsync(id);

            if (data != null)
            {
                _context.Departmants.Remove(data);

                await _context.SaveChangesAsync();

                return Json(new { departmantDb = data, message = 200 });

            }

            return Json(new { message = 400 });
        }
    }
}