using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.Enum;
using FinalProject.Areas.PayrollAdmin.Models;
using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Areas.PayrollAdmin.Controllers
{
    [Area("PayrollAdmin")]
    [Authorize(Roles = "Admin,HR")]
    public class AbsenceController : Controller
    {
        private readonly PayrollDbContext _context;

        public AbsenceController(PayrollDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {

            AbsenceViewModel data = new AbsenceViewModel
            {
                Recruitments = await _context.Recruitments.Include(r => r.Employee).Where(r => !r.Continuities.Any(c => c.Date.Date == DateTime.Now.Date))
                                                                                       .OrderBy(e => e.Employee.Firstname)
                                                                                           .ToListAsync(),
                Continuity = new Continuity
                {
                    Date = DateTime.Now
                }
            };

            return View(data);
        }

        public async Task<IActionResult> SearchList(DateTime date)
        {
            if (date.Month == DateTime.Now.Month && date.Day<=DateTime.Now.Day)
            {
                var recruitments = await _context.Recruitments.Where(r => !r.Continuities.Any(c => c.Date.Date == date.Date))
                                                               .Select(d => new
                                                               {
                                                                   d.Id,
                                                                   d.Employee.Firstname,
                                                                   d.Employee.Lastname,
                                                                   d.Employee.Image,
                                                               }).OrderBy(e => e.Firstname).ToListAsync();

                return Json(new { status = 200, data = recruitments });

            }
            else
            {
                return Json(new { status = 400, data = 0 });
            }
        }

        public async Task<IActionResult> Attendance(DateTime date)
        {
            if (date == new DateTime(0001, 01, 01)) { date = DateTime.Now; }

            var nowMonth = DateTime.Now.Month;
            var selectedMonth = date.Month;

            var mydays = DateTime.DaysInMonth(date.Year, date.Month);

            List<string> mylist = new List<string>();

            for (int i = 1; i <= mydays; i++)
            {
                mylist.Add($"{date.Month}/{i}/{date.Year}");
            }

            ViewBag.mydates = mylist;
            ViewBag.nowMonth = nowMonth;
            ViewBag.selectedMonth = selectedMonth;

            AbsenceViewModel data = new AbsenceViewModel
            {

                Continuities = await _context.Continuity.Include(c => c.Recruitment).ToListAsync(),

                Employees = await _context.Recruitments.Where(r => r.WhenLeft == new DateTime(0001, 01, 01))
                                                                         .Distinct().Select(r => r.Employee)
                                                                              .Include(e => e.Recruitments)
                                                                                  .OrderBy(r => r.Firstname)
                                                                                     .ToListAsync(),
                Continuity = new Continuity
                {
                    Date = DateTime.Now
                }

            };

            return View(data);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAjax(List<Continuity> continuities)
        {
            if (continuities.Count() != 0)
            {
                foreach (var continuity in continuities)
                {
                    if (continuity.Reason == null)
                    {
                        return Json(new { message = 300 });
                    }

                    var recruitmentContinuities = await _context.Continuity.Where(c => c.Date.Month == continuity.Date.Month && c.PermissionType == Permission.Üzürsüz && c.RecruitmentId == continuity.RecruitmentId)
                                                .OrderBy(c => c.Date).ToListAsync();

                    if(recruitmentContinuities.Count() != 0)
                    {
                        int count = 0;
                        var lastDay = recruitmentContinuities[0].Date.Day;

                        foreach (var rc in recruitmentContinuities)
                        {
                            if (rc.Date.Day - 1 == lastDay)
                            {
                                lastDay++;
                                count++;
                                if (count >= 4)
                                {
                                    Recruitment recruitment = await _context.Recruitments.FirstOrDefaultAsync(r => r.Id == continuity.RecruitmentId);
                                    recruitment.WhenLeft = DateTime.Now;
                                    await _context.SaveChangesAsync();
                                }
                            }
                        }
                    }
                    await _context.Continuity.AddAsync(continuity);
                }

                await _context.SaveChangesAsync();
                return Json(new { message = 200 });
            }

            return Json(new { message = 400 });
        }
    }
}