using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.Enum;
using FinalProject.Areas.PayrollAdmin.Extensions;
using FinalProject.Areas.PayrollAdmin.Models;
using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.PayrollAdmin.Controllers
{
    [Area("PayrollAdmin")]
    [Authorize(Roles = "Payroll Specialist, Admin")]
    public class PayrollController : Controller
    {
        private readonly PayrollDbContext _context;

        public PayrollController(PayrollDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> List()
        {
            DateTime date = new DateTime(2019, 01, 01);

            //var data = await _context.Payrolls.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month)
            //.Select(p => new
            //{
            //    p.RecruitmentId,
            //    p.Recruitment.Employee.Firstname,
            //    p.Recruitment.Employee.Lastname,
            //    p.Recruitment.Employee.Image,
            //    p.Recruitment.Shop.Company.Name,
            //    shopname = p.Recruitment.Shop.Name,
            //    positionName = p.Recruitment.Position.Name,
            //    p.Recruitment.Bonus.FirstOrDefault(b => b.RecruitmentId == p.RecruitmentId).Amount,
            //    penalty = p.Recruitment.Penalties.FirstOrDefault(pe => pe.RecruitmentId == p.RecruitmentId).Amount

            //}).ToListAsync();

            var data = await _context.Payrolls.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month)
                                                                           .Include(p => p.Recruitment)
                                                                              .Include(r => r.Recruitment.Position)
                                                                                 .Include(r => r.Recruitment.Employee)
                                                                                    .Include(r => r.Recruitment.Shop.Company)
                                                                                       .OrderBy(p => p.Recruitment.Employee.Firstname)
                                                                                         .ToListAsync();



            return View(data);

        }

        [HttpGet]
        public async Task<JsonResult> SearchList(string name, DateTime date)
        {
            if (name == null)
            {
                var recruitments = await _context.Payrolls.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month)
                .Select(p => new
                {
                    p.RecruitmentId,
                    p.Recruitment.Employee.Firstname,
                    p.Recruitment.Employee.Lastname,
                    p.Recruitment.Employee.Image,
                    p.Recruitment.Shop.Company.Name,
                    shopname = p.Recruitment.Shop.Name,
                    positionName = p.Recruitment.Position.Name,
                    p.TotalSalary
                }).ToListAsync();

                if (recruitments.Count() == 0)
                {
                    return Json(new { status = 400, data = recruitments });
                }

                return Json(new { status = 200, data = recruitments });
            }
            else
            {
                var recruitments = await _context.Payrolls.Where(p => p.Date.Year == date.Year && p.Date.Month == date.Month)
                .Select(p => new
                {
                    p.RecruitmentId,
                    p.Recruitment.Employee.Firstname,
                    p.Recruitment.Employee.Lastname,
                    p.Recruitment.Employee.Image,
                    p.Recruitment.Shop.Company.Name,
                    shopname = p.Recruitment.Shop.Name,
                    positionName = p.Recruitment.Position.Name,
                    p.Recruitment.Position.Salaries.FirstOrDefault(s => s.PositionId == p.Recruitment.PositionId).SalaryAmount
                })
                .Where(e => e.Firstname.StartsWith(name))
                   .ToListAsync();

                if (recruitments.Count() == 0)
                {
                    var recruitmentName = await _context.Payrolls.Select(p => p.Recruitment).Where(r => r.Employee.Firstname.StartsWith(name))
                    .Select(p => new
                    {
                        p.Employee.Firstname,
                        p.Employee.Lastname,
                        p.Employee.Image,
                        p.Shop.Company.Name,
                        shopname = p.Shop.Name,
                        positionName = p.Position.Name,
                        p.Position.Salaries.FirstOrDefault(s => s.PositionId == p.PositionId).SalaryAmount
                    }).ToListAsync();

                    return Json(new { status = 400, data = recruitmentName });
                }

                return Json(new { status = 200, data = recruitments });
            }
        }

        public async Task<IActionResult> Salary()
        {

            PayrollViewModel payrollViewModel = new PayrollViewModel
            {
                Recruitments = await _context.Recruitments.Include(r => r.Payrolls).Where(r => !r.Payrolls
                                            .Any(p => p.Date.Year == DateTime.Now.Year && p.Date.Month == DateTime.Now.Month) && r.WhenLeft == new DateTime(0001, 01, 01))
                                                                                     .Include(r => r.Employee)
                                                                                         .Include(r => r.Position)
                                                                                             .Include(r => r.Shop)
                                                                                                  .Include(r => r.Shop.Company)
                                                                                                       .OrderBy(r => r.Employee.Firstname)
                                                                                                            .ToListAsync()


            };

            return View(payrollViewModel);
        }

        [HttpGet]
        public async Task<JsonResult> SalarySearch(DateTime date)
        {
            if (date.Month < DateTime.Now.Month) return Json(new { message = 400, data = 0 });

            var recruitments = await _context.Recruitments.Where(r => r.WhenLeft == new DateTime(0001, 01, 01)).Where(r => !r.Payrolls.Any(p => p.Date.Year == date.Year && p.Date.Month == date.Month))
            .Select(p => new
            {
                p.Id,
                p.Employee.Firstname,
                p.Employee.Lastname,
                p.Employee.Image,
                p.Shop.Company.Name,
                shopname = p.Shop.Name,
                positionName = p.Position.Name,
                p.Position.Salaries.FirstOrDefault(s => s.PositionId == p.PositionId).SalaryAmount
            }).ToListAsync();

            if (recruitments.Count() == 0)
            {
                return Json(new { status = 200, data = recruitments });
            }

            return Json(new
            {
                status = 400,
                data = recruitments
            });

        }

        public async Task<JsonResult> CalcSalary(List<int> id, DateTime date)
        {
            if (date == new DateTime(0001,01,01))
            {
                date = DateTime.Now;
            }

            foreach (int recruitmentId in id)
            {
                Recruitment recruitment = await _context.Recruitments.Include(r => r.Shop.Company).Include(r => r.Payrolls)
                                                                                .FirstOrDefaultAsync(r => r.Id == recruitmentId && r.WhenLeft == new DateTime(0001, 01, 01));

                if (recruitment == null)
                {
                    continue;
                }

                if (!recruitment.Payrolls.Any(p => p.Date.Year == date.Year && p.Date.Month == date.Month) && date.Day >= recruitment.Shop.Company.SalaryDate.Day)
                {
                    decimal salary = (await _context.Salaries.FirstOrDefaultAsync(s => s.PositionId == recruitment.PositionId && s.CompanyId == recruitment.Shop.CompanyId)).SalaryAmount;

                    var bonusAmount = (await _context.Bonus.FirstOrDefaultAsync(b => b.Date.Month == date.Month && b.RecruitmentId == recruitment.Id))?.Amount;

                    var penaltyAmount = (await _context.Penalties.FirstOrDefaultAsync(p => p.Date.Month == date.Month && p.RecruitmentId == recruitment.Id))?.Amount;

                    var shopBonusAmount = (await _context.RecruitmentShopBonus.FirstOrDefaultAsync(r => r.Date.Month == date.Month && r.RecruitmentId == recruitment.Id))?.ShopBonusAmount;

                    var vacationDayCount = _context.Vacations.Where(v => v.WhenStarted.Month == date.Month && v.RecruitmentId == recruitment.Id).Select(v => (v.WhenLeft.Day - v.WhenStarted.Day)).Count();

                    var continuity = _context.Continuity.Where(c => c.Date.Month == date.Month && c.PermissionType == Permission.Üzürsüz && c.RecruitmentId == recruitment.Id).Count();

                    //var continuityCount = continuity.Count();

                    //int count = 0;
                    //var lastDay = continuity[0].Date.Day;
                    //int counts = 0;
                    //foreach (var con in continuity)
                    //{
                    //    if (con.Date.Day - 1 == lastDay)
                    //    {
                    //        lastDay++;
                    //        count++;
                    //    }
                    //    else
                    //    {
                    //        if (count > 2 && count < 5)
                    //        {
                    //            counts += count;
                    //        }
                    //        lastDay = con.Date.Day;
                    //        count = 0;
                    //    }
                    //}

                    //continuityCount += counts;

                    var workDay = (DateTime.DaysInMonth(date.Year, date.Month) - continuity);

                    var bonus = bonusAmount.HasValue ? bonusAmount.Value : 0;

                    var penalty = penaltyAmount.HasValue ? penaltyAmount.Value : 0;

                    var shopBonus = shopBonusAmount.HasValue ? shopBonusAmount.Value : 0;

                    var vacationAmount = (Convert.ToInt32(vacationDayCount) / 2) * (recruitment.SalaryByDay(salary));

                    var totalSalary = (workDay * recruitment.SalaryByDay(salary)) + bonus + penalty + shopBonus + vacationAmount;

                    Payroll payroll = new Payroll()
                    {
                        RecruitmentId = recruitmentId,
                        TotalSalary = totalSalary,
                        Date = date
                    };

                    await _context.Payrolls.AddAsync(payroll);
                }
            }

            await _context.SaveChangesAsync();
            return Json(new { message = 200 });
        }
    }
}