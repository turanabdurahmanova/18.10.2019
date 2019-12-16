using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.PayrollAdmin.Controllers
{
    [Area("PayrollAdmin")]
    public class AjaxController : Controller
    {
        private readonly PayrollDbContext _context;

        public AjaxController(PayrollDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<JsonResult> CompanySearch(string name)
        {
            if (name != null)
            {
                List<Company> companies = await _context.Companies.Where(c => c.Name.StartsWith(name))
                                                                            .ToListAsync();
                if (companies.Count() == 0)
                {
                    return Json(new { status = 400, data = companies });
                }

                return Json(new { status = 200, data = companies });
            }

            return Json(new { status = 400 });
        }

        [HttpGet]
        public async Task<JsonResult> DepartamentSearch(string name)
        {
            if (name != null)
            {
                List<Departmant> departmants = await _context.Departmants.Where(c => c.Name.StartsWith(name))
                                                                            .ToListAsync();
                if (departmants.Count() == 0)
                {
                    return Json(new { status = 400, data = departmants });
                }

                return Json(new { status = 200, data = departmants });
            }

            return Json(new { status = 400 });
        }

        [HttpGet]
        public async Task<JsonResult> ShopSearch(string name)
        {
            if (name != null)
            {
                List<Shop> shops = await _context.Shops.Where(c => c.Name.StartsWith(name)).ToListAsync();

                if (shops.Count() == 0)
                {
                    return Json(new { status = 400, data = shops });
                }

                return Json(new { status = 200, data = shops });
            }

            return Json(new { status = 400 });
        }

        [HttpGet]
        public async Task<JsonResult> PositionSearch(string name)
        {
            if (name != null)
            {
                var positions = await _context.Positions.Select(d => new
                {

                    d.Id,
                    d.Name,
                    departamentName = d.Departmant.Name

                }).Where(c => c.Name.StartsWith(name)).ToListAsync();

                if (positions.Count() == 0)
                {
                    return Json(new { status = 400, data = positions });
                }

                return Json(new { status = 200, data = positions });
            }

            return Json(new { status = 400 });
        }

        [HttpGet]
        public async Task<JsonResult> PositionChangedSearch(string name)
        {
            var recruitments = await _context.Recruitments.Select(r => new
            {
                r.Id,
                r.Employee.Firstname,
                r.Employee.Lastname,
                r.Employee.Phone,
                r.Employee.Email,
                r.Employee.Image,
                r.Position.Name,
                r.WhenStarted

            }).Where(e => e.Firstname.StartsWith(name)).OrderBy(e => e.Firstname).ToListAsync();

            if (recruitments.Count() == 0)
            {
                return Json(new { status = 400, data = recruitments });
            }

            return Json(new { status = 200, data = recruitments });
        }

        [HttpGet]
        public async Task<JsonResult> EmployeeSearch(string name)
        {
            if (name != null)
            {
                List<Employee> employees = await _context.Employees.Where(e => e.Recruitments.Count() == 0)
                                                                        .Where(e => e.Firstname.StartsWith(name))
                                                                            .ToListAsync();
                if (employees.Count() == 0)
                {
                    return Json(new { status = 400, data = employees });
                }

                return Json(new { status = 200, data = employees });
            }

            return Json(new { status = 400 });
        }

        [HttpGet]
        public async Task<JsonResult> RecruitmentSearch(string name)
        {
            if (name != null)
            {
                var recruitments = await _context.Recruitments.Select(r => r.Employee).Include(emp => emp.AppUser)
                .Select(e => new
                {
                    e.Firstname,
                    e.Lastname,
                    e.Image,
                    role = (e.AppUser == null || e.AppUser.AppUserRoles == null) ? "Role Yoxdur" : e.AppUser.AppUserRoles.Select(aur => aur.AppRole.Name).FirstOrDefault(),
                    e.Phone,
                    e.Email,
                    e.Id
                }).Where(r => r.Firstname.StartsWith(name)).ToListAsync();

                if (recruitments.Count() == 0)
                {
                    return Json(new { status = 400, data = recruitments });
                }

                return Json(new { status = 200, data = recruitments });
            }

            return Json(new { status = 400 });
        }

        [HttpGet]
        public async Task<JsonResult> VacationSearch(string name)
        {
            if (name != null)
            {
                var vacations = await _context.Vacations.Select(v=>v.Recruitment)
                .Select(r => new
                {
                    r.Employee.Firstname,
                    r.Employee.Lastname,
                    r.Employee.Image,
                    r.Shop.Company.Name,
                    positionName = r.Position.Name,
                    r.Employee.Email,
                    r.Id
                }).Where(r => r.Firstname.StartsWith(name)).ToListAsync();

                if (vacations.Count() == 0)
                {
                    return Json(new { status = 400, data = vacations });
                }

                return Json(new { status = 200, data = vacations });
            }

            return Json(new { status = 400 });
        }

        [HttpGet]
        public async Task<JsonResult> BonusListSearch(string name)
        {
            if (name != null)
            {
                var bonus = await _context.Bonus.Where(b => b.Date.Month == DateTime.Now.Month && b.Recruitment.WhenLeft == new DateTime(0001, 01, 01)).Select(b => new
                {
                    b.Recruitment.Employee.Firstname,
                    b.Recruitment.Employee.Lastname,
                    b.Recruitment.Employee.Email,
                    b.Recruitment.Employee.Image,
                    b.Recruitment.Shop.Name,
                    b.RecruitmentId,
                    positionName = b.Recruitment.Position.Name,
                    b.Amount,
                    b.Reason,
                    b.Id
                }).Where(f => f.Firstname.StartsWith(name)).ToListAsync();


                return Json(new { status = 200, data = bonus });
            }

            return Json(new { status = 400 });
        }

        [HttpGet]
        public async Task<JsonResult> PenaltyListSearch(string name)
        {
            if (name != null)
            {
                var penalty = await _context.Penalties.Where(b => b.Date.Month == DateTime.Now.Month && b.Recruitment.WhenLeft == new DateTime(0001, 01, 01))
                .Select(b => new
                {
                    b.Recruitment.Employee.Firstname,
                    b.Recruitment.Employee.Lastname,
                    b.Recruitment.Employee.Email,
                    b.Recruitment.Employee.Image,
                    b.Recruitment.Shop.Name,
                    b.RecruitmentId,
                    positionName = b.Recruitment.Position.Name,
                    b.Amount,
                    b.Reason,
                    b.Id
                }).Where(f => f.Firstname.StartsWith(name)).ToListAsync();

                if (penalty.Count() == 0)
                    return Json(new { status = 400, data = penalty });

                return Json(new { status = 200, data = penalty });
            }

            return Json(new { status = 400 });
        }

        [HttpGet]
        public async Task<JsonResult> BonusCreateSearch(string name)
        {
            if (name != null)
            {
                var bonus = await _context.Recruitments.Where(r => r.WhenLeft == new DateTime(0001, 01, 01)).Where(r => !r.Bonus.Any(b => b.Date.Year == DateTime.Now.Year && b.Date.Month == DateTime.Now.Month))
                .Select(r => new
                {
                    r.Employee.Firstname,
                    r.Employee.Lastname,
                    r.Employee.Fathername,
                    r.Employee.Image,
                    r.Employee.Email,
                    r.Shop.Name,
                    positionName = r.Position.Name,
                    r.Id
                }).Where(f => f.Firstname.StartsWith(name)).ToListAsync();

                return Json(new { status = 200, data = bonus });
            }

            return Json(new { status = 400 });
        }

        [HttpGet]
        public async Task<JsonResult> PenaltyCreateSearch(string name)
        {
            if (name != null)
            {
                var bonus = await _context.Recruitments.Where(r => r.WhenLeft == new DateTime(0001, 01, 01)).Where(r => !r.Penalties.Any(b => b.Date.Year == DateTime.Now.Year && b.Date.Month == DateTime.Now.Month))
                .Select(r => new
                {
                    r.Employee.Firstname,
                    r.Employee.Lastname,
                    r.Employee.Fathername,
                    r.Employee.Image,
                    r.Employee.Email,
                    r.Shop.Name,
                    positionName = r.Position.Name,
                    r.Id
                }).Where(f => f.Firstname.StartsWith(name)).ToListAsync();

                return Json(new { status = 200, data = bonus });
            }

            return Json(new { status = 400 });
        }

        public async Task<IActionResult> Departament(int? id)
        {
            if (id != null)
            {
                var data = await _context.CompanyDepartaments.Where(cd => cd.CompanyId == id).Select(cd => new { id = cd.DepartmantId, name = cd.Departmant.Name }).ToListAsync();

                if (data.Count() != 0)
                {
                    return Json(new { departament = data, message = 200 });
                }
            }
            return Json(new { message = 400 });
        }

        public async Task<JsonResult> Shop(int? id)
        {
            if (id != null)
            {
                var data = await _context.Shops.Where(x => x.CompanyId == id).ToListAsync();
                if (data.Count != 0)
                {
                    return Json(new { shop = data, message = 200 });
                }
            }

            return Json(new { message = 400 });
        }

        public async Task<JsonResult> Position(int? id)
        {
            if (id != null)
            {
                //var data = await _context.ShopPositions.Where(x => x.ShopId == id).Select(sp => new { id = sp.PositionId, name = sp.Position.Name }).ToListAsync();

                var data = await _context.Positions.Where(p => p.DepartmantId == id).ToListAsync();

                if (data.Count() != 0)
                {
                    return Json(new { shop = data, message = 200 });
                }
            }

            return Json(new { message = 400 });
        }

        public async Task<JsonResult> Salary(int? id)
        {
            if (id != null)
            {
                var data = await _context.Salaries.Where(p => p.PositionId == id).ToListAsync();

                if (data.Count() != 0)
                {
                    return Json(new { salary = data, message = 200 });
                }
            }

            return Json(new { message = 400 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShopProfitWrite(int shopId, DateTime date, decimal profit)
        {
            var dbShop = _context.Shops.Where(s => s.Id == shopId).FirstOrDefault();

            ShopProfit data = new ShopProfit
            {
                ShopId = shopId,
                Date = date,
                Profit = profit
            };

            await _context.ShopProfits.AddAsync(data);
            await _context.SaveChangesAsync();

            return Json(new { message = 200 });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ShopBonus(List<ShopBonus> shopBonus, decimal mountProfit)
        {
            if (shopBonus.Count() != 0)
            {
                foreach (var item in shopBonus)
                {
                    if (mountProfit > item.MinAmount)
                    {
                        var shops = await _context.Shops.Where(s => s.Id == item.ShopId)
                                                                    .Select(s => s.Recruitments)
                                                                         .ToListAsync();
                        foreach (var recruitments in shops)
                        {
                            foreach (var recruitment in recruitments)
                            {
                                try
                                {
                                    var dbRecruitment = await _context.Recruitments.FirstOrDefaultAsync(r => r.Id == recruitment.Id);

                                    RecruitmentShopBonus recruitmentShopBonus = new RecruitmentShopBonus
                                    {
                                        RecruitmentId = dbRecruitment.Id,
                                        ShopBonusAmount = item.PromotionAmount,
                                        Date = DateTime.Now,
                                    };

                                    await _context.RecruitmentShopBonus.AddAsync(recruitmentShopBonus);

                                    await _context.SaveChangesAsync();
                                }
                                catch (Exception exp)
                                {

                                    throw;
                                }
                            }
                        }
                    }
                }

                return Json(new { message = 200 });

            }

            return Json(new { message = 400 });
        }

    }
}