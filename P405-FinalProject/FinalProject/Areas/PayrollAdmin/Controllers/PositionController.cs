using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.Models;
using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.PayrollAdmin.Controllers
{
    [Area("PayrollAdmin")]
    public class PositionController : Controller
    {
        private readonly PayrollDbContext _context;

        public PositionController(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List(int page = 1)
        {
            int take = 6;

            PositionPaginationViewModel data = new PositionPaginationViewModel
            {
                Positions = await _context.Positions.Include(p => p.Departmant).OrderBy(p => p.Id)
                                                                                    .Skip((page - 1) * take)
                                                                                        .Take(take).ToListAsync(),
                PaginationModel = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = take,
                    TotalItems = _context.Positions.Count()
                },

                Departaments = new SelectList(await _context.Departmants.ToListAsync(), "Id", "Name")
            };

            return View(data);
        }

        public IActionResult Details()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, string name)
        {
            Position data = new Position
            {
                Name = name,
                DepartmantId = id
            };

            await _context.Positions.AddAsync(data);
            await _context.SaveChangesAsync();

            return Json(new { message = 200 });
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                var data = await _context.Positions.Where(c => c.Id == id).FirstOrDefaultAsync();

                if (data != null)
                {
                    return Json(new { message = 200, positionDb = data });
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

            var positionDb = await _context.Positions.Include(c => c.Departmant).Where(c => c.Id == id).FirstOrDefaultAsync();

            positionDb.Name = name;
            positionDb.DepartmantId = positionDb.DepartmantId;

            await _context.SaveChangesAsync();

            return Json(new { message = 200 });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var data = await _context.Positions.FindAsync(id);

                if (data != null)
                {
                    return Json(new { positionDb = data, message = 200 });
                }
            }

            return Json(new { message = 400 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var data = await _context.Positions.FindAsync(id);

            if (data != null)
            {
                _context.Positions.Remove(data);

                await _context.SaveChangesAsync();

                return Json(new { positionDb = data, message = 200 });
            }

            return Json(new { message = 400 });
        }

        [HttpGet]
        public async Task<IActionResult> PositionChanged(int id)
        {

            PositionChangedViewModel data = new PositionChangedViewModel
            {
                Recruitment = await _context.Recruitments.Include(r => r.Employee).Include(r => r.Position).Include(r => r.Position.Departmant).Include(r => r.Shop).Include(r => r.Shop.Company).Where(r => r.Id == id).FirstOrDefaultAsync(),

                SelectListItemViewModel = new SelectListItemViewModel
                {
                    SelectListPositions = await _context.Positions.Select(d => new SelectListItem
                    {

                        Value = d.Id.ToString(),
                        Text = d.Name

                    }).ToListAsync(),

                    SelectListCompanies = await _context.Companies.Select(d => new SelectListItem
                    {

                        Value = d.Id.ToString(),
                        Text = d.Name

                    }).ToListAsync(),

                    SelectListDepartmans = await _context.Departmants.Select(d => new SelectListItem
                    {

                        Value = d.Id.ToString(),
                        Text = d.Name

                    }).ToListAsync()
                },

                WorkerNewWhenStarted = DateTime.Now,

                WorkerWhenLeft = DateTime.Now

            };
            ViewBag.Positions = data.SelectListItemViewModel.SelectListPositions;
            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PositionChanged(int id, PositionChangedViewModel positionChangedViewModel)
        {
            if (ModelState.IsValid)
            {
                var recruitment = await _context.Recruitments.FindAsync(id);

                recruitment.WhenLeft = positionChangedViewModel.WorkerWhenLeft;

                await _context.SaveChangesAsync();

                Recruitment newdata = new Recruitment
                {
                    PositionId = positionChangedViewModel.newPositionId,
                    ShopId = recruitment.ShopId,
                    EmployeeId = recruitment.EmployeeId,
                    Amount = positionChangedViewModel.newSalary,
                    WhenStarted = positionChangedViewModel.WorkerNewWhenStarted
                };

                if (newdata.Amount != 0 && newdata.PositionId != 0)
                {
                    await _context.Recruitments.AddAsync(newdata);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(Recruitment));
                }

                return BadRequest();
            }
            PositionChangedViewModel data = new PositionChangedViewModel()
            {
                SelectListItemViewModel = new SelectListItemViewModel()
                {
                    SelectListPositions = ViewBag.Positions
                }
            };
            return View(data);
        }

        public async Task<IActionResult> Recruitment(int page = 3)
        {
            int take = 6;

            RecruitmentPaginationViewModel data = new RecruitmentPaginationViewModel
            {
                Recruitments = await _context.Recruitments.Where(r => r.WhenLeft == new DateTime(0001, 01, 01)).Include(r => r.Position).Include(r => r.Employee)
                                                            .OrderBy(r => r.Employee.Firstname)
                                                                .Skip((page - 1) * take)
                                                                    .Take(take).ToListAsync(),

                PaginationModel = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = take,
                    TotalItems = _context.Recruitments.Count()
                },

                Recruitment = await _context.Recruitments.FirstOrDefaultAsync()
            };

            return View(data);
        }


    }
}