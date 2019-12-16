using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.Enum;
using FinalProject.Areas.PayrollAdmin.Extensions;
using FinalProject.Areas.PayrollAdmin.Models;
using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static FinalProject.Areas.PayrollAdmin.Utilities.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace FinalProject.Areas.PayrollAdmin.Controllers
{
    [Area("PayrollAdmin")]
    [Authorize(Roles = "Admin,HR")]
    public class EmployeeController : Controller
    {
        private readonly PayrollDbContext _context;
        private readonly IHostingEnvironment _env;

        public EmployeeController(PayrollDbContext context, IHostingEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> List(int page = 1)
        {
            int take = 6;

            EmployeePaginationViewModel data = new EmployeePaginationViewModel
            {
                Employees = await _context.Employees.Where(e => e.Recruitments.Count() == 0)
                                                                    .OrderBy(r => r.Id)
                                                                      .Skip((page - 1) * take)
                                                                        .Take(take).ToListAsync(),

                PaginationModel = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = take,
                    TotalItems = _context.Employees.Where(e => e.Recruitments.Count() == 0).Count()
                },

                Employee = await _context.Employees.FirstOrDefaultAsync()
            };

            return View(data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var data = await _context.Employees.FindAsync(id);

            if (data == null) return NotFound();

            return View(data);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var data = await _context.Employees.FindAsync(id);

            if (data == null) return BadRequest();

            EmployeeEditViewModel EmployeeEditViewModel = data;

            return View(EmployeeEditViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, EmployeeEditViewModel employeeEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                var data = await _context.Employees.FindAsync(id);

                EmployeeEditViewModel EmployeeEditViewModel = data;

                return View(EmployeeEditViewModel);
            }

            var employeeDb = await _context.Employees.FindAsync(id);

            if (employeeEditViewModel.Photo != null)
            {
                if (!employeeEditViewModel.Photo.IsImage())
                {
                    ModelState.AddModelError("Photo", "File type is not valid...");
                    return View(employeeEditViewModel);
                }

                if (!employeeEditViewModel.Photo.IsLessThan(2))
                {
                    ModelState.AddModelError("Photo", "File size can not be more than 2 mb");
                    return View(employeeEditViewModel);
                }

                RemoveImage(_env.WebRootPath, employeeDb.Image);

                employeeDb.Image = await employeeEditViewModel.Photo.SaveAsync(_env.WebRootPath, "employee");
            }

            employeeDb.Firstname = employeeEditViewModel.Firstname;
            employeeDb.Lastname = employeeEditViewModel.Lastname;
            employeeDb.Fathername = employeeEditViewModel.Fathername;
            employeeDb.Birthday = employeeEditViewModel.Birthday;
            employeeDb.Birthday = employeeEditViewModel.Birthday;
            employeeDb.CurrentAddres = employeeEditViewModel.CurrentAddres;
            employeeDb.DistrictRegistration = employeeEditViewModel.DistrictRegistration;
            employeeDb.PassportNumber = employeeEditViewModel.PassportNumber;
            employeeDb.PassportExpirationDate = employeeEditViewModel.PassportExpirationDate;
            employeeDb.GenderType = employeeEditViewModel.GenderType;
            employeeDb.EducationType = employeeEditViewModel.EducationType;
            employeeDb.MarialStatusType = employeeEditViewModel.MarialStatusType;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List));
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            if (!ModelState.IsValid) return View(employee);

            if (!employee.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Photo should be selected for slider");
                return View();
            }

            if (!employee.Photo.IsLessThan(2))
            {
                ModelState.AddModelError("Photo", "Photo size should be lass than 2 mb");
                return View();
            }

            string fileName = await employee.Photo.SaveAsync(_env.WebRootPath, "employee");
            employee.Image = fileName;

            await _context.Employees.AddAsync(employee);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(List));

            //if(employeeViewModel.Employee.Firstname == null                ||
            //    employeeViewModel.Employee.Lastname == null                ||
            //    employeeViewModel.Employee.Email == null                   ||
            //    employeeViewModel.Employee.Phone == null                   ||
            //    employeeViewModel.Employee.CurrentAddres == null           ||
            //    employeeViewModel.Employee.DistrictRegistration == null    ||
            //    employeeViewModel.Employee.PassportNumber == null          ||
            //    employeeViewModel.Employee.PassportExpirationDate == null   )
            //{
            //    if (!ModelState.IsValid) return View(employeeViewModel);
            //}

            //if (!employeeViewModel.Employee.Photo.IsImage())
            //{
            //    ModelState.AddModelError("Photo", "Photo should be selected for slider");
            //    return View();
            //}

            //if (!employeeViewModel.Employee.Photo.IsLessThan(2))
            //{
            //    ModelState.AddModelError("Photo", "Photo size should be lass than 2 mb");
            //    return View();
            //}

            //string fileName = await employeeViewModel.Employee.Photo.SaveAsync(_env.WebRootPath, "employee");
            //employeeViewModel.Employee.Image = fileName;

            //var dbemployee = new Employee
            //{
            //    Firstname = employeeViewModel.Employee.Firstname,
            //    Lastname = employeeViewModel.Employee.Lastname,
            //    Fathername = employeeViewModel.Employee.Fathername,
            //    Birthday = employeeViewModel.Employee.Birthday,
            //    CurrentAddres = employeeViewModel.Employee.CurrentAddres,
            //    EducationType = employeeViewModel.Employee.EducationType,
            //    Phone = employeeViewModel.Employee.Phone,
            //    GenderType = employeeViewModel.Employee.GenderType,
            //    MarialStatusType = employeeViewModel.Employee.MarialStatusType,
            //    DistrictRegistration = employeeViewModel.Employee.DistrictRegistration,
            //    PassportExpirationDate = employeeViewModel.Employee.PassportExpirationDate,
            //    PassportNumber = employeeViewModel.Employee.PassportNumber,
            //    Image = employeeViewModel.Employee.Image,
            //    Email = employeeViewModel.Employee.Email

            //};

            //_context.Employees.Add(dbemployee);
            //_context.SaveChanges();

            //if (ModelState["Recruitment"] != null && ModelState["Recruitment"].ValidationState == ModelValidationState.Invalid)
            //    return View(employeeViewModel);

            //if (employeeViewModel.FormerWorks != null)
            //{
            //    if (!ModelState.IsValid) return View(employeeViewModel);

            //    if (employeeViewModel.FormerWorks[0].WorkName != null)
            //    {
            //        foreach (var former in employeeViewModel.FormerWorks)
            //        {
            //            if (former != null)
            //            {
            //                FormerWork formerWork = new FormerWork
            //                {
            //                    EmployeeId = dbemployee.Id,
            //                    WorkName = former.WorkName,
            //                    WhenLeft = former.WhenLeft,
            //                    WhenStarted = former.WhenStarted,
            //                    WhyLeftReason = former.WhyLeftReason
            //                };

            //                _context.FormerWorks.Add(formerWork);
            //            }
            //        }
            //        _context.SaveChanges();
            //    }
            //}

            //if (employeeViewModel.Recruitment.PositionId != 0)
            //{
            //    if (!ModelState.IsValid) return View(employeeViewModel);

            //    Recruitment recruitment = new Recruitment
            //    {
            //        EmployeeId = dbemployee.Id,
            //        WhenStarted = employeeViewModel.Recruitment.WhenStarted,
            //        PositionId = employeeViewModel.Recruitment.PositionId,
            //        ShopId = employeeViewModel.Recruitment.ShopId
            //    };

            //    _context.Recruitments.Add(recruitment);
            //    _context.SaveChanges();
            //}

            //return RedirectToAction(nameof(List));

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var dataEmployee = await _context.Employees.FindAsync(id);

                if (dataEmployee != null)
                {
                    return Json(new { employeeDb = dataEmployee, message = 200 });
                }

            }

            return Json(new { message = 400 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var data = await _context.Employees.FindAsync(id);

            if (data != null)
            {
                _context.Employees.Remove(data);

                await _context.SaveChangesAsync();

                return Json(new { employeeDb = data, message = 200 });
            }

            return Json(new { message = 400 });
        }

        [HttpGet]
        public IActionResult Recruitment()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Recruitment(int id, EmployeeViewModel employeeViewModel)
        {

            if (ModelState["Recruitment"] != null && ModelState["Recruitment"].ValidationState == ModelValidationState.Invalid)
                return View(employeeViewModel);

            if (employeeViewModel.FormerWorks != null)
            {
                if (!ModelState.IsValid) return View(employeeViewModel);

                if (employeeViewModel.FormerWorks[0].WorkName != null)
                {
                    foreach (var former in employeeViewModel.FormerWorks)
                    {
                        if (former != null)
                        {
                            FormerWork formerWork = new FormerWork
                            {
                                EmployeeId = id,
                                WorkName = former.WorkName,
                                WhenLeft = former.WhenLeft,
                                WhenStarted = former.WhenStarted,
                                WhyLeftReason = former.WhyLeftReason
                            };

                            _context.FormerWorks.Add(formerWork);
                        }
                    }
                    _context.SaveChanges();
                }
            }

            if (employeeViewModel.Recruitment.ShopId != null)
            {
                if (!ModelState.IsValid) return View(employeeViewModel);

                Recruitment recruitment = new Recruitment
                {
                    EmployeeId = id,
                    WhenStarted = employeeViewModel.Recruitment.WhenStarted,
                    PositionId = employeeViewModel.Recruitment.PositionId,
                    ShopId = employeeViewModel.Recruitment.ShopId
                };

                _context.Recruitments.Add(recruitment);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(List));

        }

    }
}