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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using static FinalProject.Areas.PayrollAdmin.Utilities.Utilities;


namespace FinalProject.Areas.PayrollAdmin.Controllers
{
    [Area("PayrollAdmin")]
    [Authorize(Roles = "Admin,HR")]

    public class RecruitmentController : Controller
    {
        private readonly PayrollDbContext _context;
        private readonly IHostingEnvironment _env;
        private UserManager<AppUser> _userManager;
        private RoleManager<AppRole> _roleManager;

        public RecruitmentController(PayrollDbContext context, IHostingEnvironment env,
                                        UserManager<AppUser> userManager,
                                        RoleManager<AppRole> roleManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> List(int page = 3)
        {
            int take = 6;

            RecruitmentPaginationViewModel data = new RecruitmentPaginationViewModel
            {
                RecruitmentList = await _context.Recruitments.Where(r => r.WhenLeft == new DateTime(0001, 01, 01)).Select(r => r.Employee)
                                                    .OrderBy(e => e.Firstname)
                                                          .Skip((page - 1) * take).Take(take)
                                                            .Include(e => e.AppUser)
                                                                .Select(e => new RecruitmentViewModel()
                                                                {
                                                                    Employee = e,
                                                                    Role = (e.AppUser == null || e.AppUser.AppUserRoles == null) ? "Role Yoxdur" : e.AppUser.AppUserRoles.Select(aur => aur.AppRole.Name).FirstOrDefault()
                                                                }).ToListAsync(),

                PaginationModel = new PaginationModel
                {
                    CurrentPage = page,
                    ItemsPerPage = take,
                    TotalItems = _context.Recruitments.Count()
                }

            };

            return View(data);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var data = await _context.Recruitments.Where(r => r.EmployeeId == id).Include(r => r.Employee).FirstOrDefaultAsync();

            if (data == null) return NotFound();

            return View(data);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var data = await _context.Recruitments.Where(r => r.EmployeeId == id).Include(r => r.Employee).FirstOrDefaultAsync();

            if (data == null) return BadRequest();

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, EmployeeEditViewModel employeeEditViewModel)
        {
            if (!ModelState.IsValid)
            {
                var data = await _context.Employees.FindAsync(id);

                if (data == null) return BadRequest();

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
        public async Task<IActionResult> Create(EmployeeViewModel employeeViewModel)
        {
            if (employeeViewModel.Employee.Firstname == null ||
                employeeViewModel.Employee.Lastname == null ||
                employeeViewModel.Employee.Email == null ||
                employeeViewModel.Employee.Photo == null ||
                employeeViewModel.Employee.CurrentAddres == null ||
                employeeViewModel.Employee.DistrictRegistration == null ||
                employeeViewModel.Employee.PassportNumber == null ||
                employeeViewModel.Employee.PassportExpirationDate == null)
            {
                if (!ModelState.IsValid) return View(employeeViewModel);
            }

            if (!employeeViewModel.Employee.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "Photo should be selected for slider");
                return View();
            }

            if (!employeeViewModel.Employee.Photo.IsLessThan(2))
            {
                ModelState.AddModelError("Photo", "Photo size should be lass than 2 mb");
                return View();
            }

            string fileName = await employeeViewModel.Employee.Photo.SaveAsync(_env.WebRootPath, "employee");
            employeeViewModel.Employee.Image = fileName;

            var dbemployee = new Employee
            {
                Firstname = employeeViewModel.Employee.Firstname,
                Lastname = employeeViewModel.Employee.Lastname,
                Fathername = employeeViewModel.Employee.Fathername,
                Birthday = employeeViewModel.Employee.Birthday,
                CurrentAddres = employeeViewModel.Employee.CurrentAddres,
                EducationType = employeeViewModel.Employee.EducationType,
                Phone = employeeViewModel.Employee.Phone,
                GenderType = employeeViewModel.Employee.GenderType,
                MarialStatusType = employeeViewModel.Employee.MarialStatusType,
                DistrictRegistration = employeeViewModel.Employee.DistrictRegistration,
                PassportExpirationDate = employeeViewModel.Employee.PassportExpirationDate,
                PassportNumber = employeeViewModel.Employee.PassportNumber,
                Image = employeeViewModel.Employee.Image,
                Email = employeeViewModel.Employee.Email

            };

            _context.Employees.Add(dbemployee);
            _context.SaveChanges();

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
                                EmployeeId = dbemployee.Id,
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

            if (employeeViewModel.Recruitment.PositionId != 0)
            {
                if (!ModelState.IsValid) return View(employeeViewModel);

                Recruitment recruitment = new Recruitment
                {
                    EmployeeId = dbemployee.Id,
                    WhenStarted = employeeViewModel.Recruitment.WhenStarted,
                    PositionId = employeeViewModel.Recruitment.PositionId,
                    ShopId = employeeViewModel.Recruitment.ShopId
                };

                _context.Recruitments.Add(recruitment);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(List));

        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id != null)
            {
                var data = await _context.Employees.Where(e => e.Id == id).FirstOrDefaultAsync();

                if (data != null)
                {
                    return Json(new { recruitmentDb = data, message = 200 });
                }
            }
            return Json(new { message = 400 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(int? id)
        {
            var data = await _context.Recruitments.FirstOrDefaultAsync(r => r.EmployeeId == id);

            if (data != null)
            {
                _context.Recruitments.Remove(data);

                await _context.SaveChangesAsync();

                return Json(new { recruitmentDb = data, message = 200 });
            }
            return Json(new { message = 400 });
        }

        [HttpGet]
        public async Task<IActionResult> AddRole(int? id)
        {
            if (id.HasValue)
            {
                Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);

                if (employee == null)
                {
                    return NotFound();
                }

                AddRoleViewModel data = new AddRoleViewModel
                {
                    Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name"),
                    Employee = employee,
                };
                return View(data);
            }
            return BadRequest();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(int? id, AddRoleViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (id.HasValue)
                    {
                        Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);
                        if (employee == null)
                        {
                            return NotFound();
                        }
                        AppUser appUser = new AppUser()
                        {
                            Email = employee.Email,
                            PhoneNumber = employee.Phone,
                            UserName = model.AppUser.UserName,
                            EmployeeId = employee.Id
                        };

                        IdentityResult result = await _userManager.CreateAsync(appUser, model.Password);

                        if (result.Succeeded)
                        {
                            AppRole role = await _roleManager.FindByIdAsync(model.RoleId);
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                    }
                    return RedirectToAction("List", "Recruitment");
                }
            }
            catch (Exception exp)
            {
                throw;
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(int? id)
        {
            if (id.HasValue)
            {
                Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == id);


                if (employee == null)
                {
                    return NotFound();
                }

                AddRoleViewModel data = new AddRoleViewModel
                {
                    Roles = new SelectList(await _context.Roles.ToListAsync(), "Id", "Name"),
                    Employee = employee,
                };

                return View(data);
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> EditRole(AddRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = await _context.Employees.FirstOrDefaultAsync(e => e.Id == model.EmployeeId);

                if (employee == null)
                {
                    return NotFound();
                }

                AppUser appUser = await _context.Users.Include(u => u.AppUserRoles)
                                                        .SingleOrDefaultAsync(u => u.EmployeeId == employee.Id);

                var oldRole = (await _context.UserRoles.Include(ur => ur.AppRole).Where(ur => ur.UserId == appUser.Id).FirstOrDefaultAsync()).AppRole.Name;

                string newRole = (await _roleManager.FindByIdAsync(model.RoleId)).Name;

                await _userManager.RemoveFromRoleAsync(appUser, oldRole);

                await _userManager.AddToRoleAsync(appUser, newRole);

                await _context.SaveChangesAsync();
                return RedirectToAction("List", "Recruitment");
            }
            return View();
        }


    }
}