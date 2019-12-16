using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Company
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OpenCompany { get; set; }

        [Required, StringLength(50)]
        public string Email { get; set; }

        [Required, StringLength(50)]
        public string TelNumber { get; set; }

        [Required, StringLength(200)]
        public string Addres { get; set; }
        
        public virtual PoctIndex PoctIndex { get; set; }
        public int PoctIndexId { get; set; }

        public DateTime SalaryDate { get; set; }

        public virtual ICollection<CompanyDepartament> CompanyDepartaments { get; set; }
        public virtual ICollection<Shop> Shops { get; set; }
        public virtual ICollection<Salary> Salaries { get; set; }
        public virtual ICollection<CompanyProfit> CompanyProfits { get; set; }

        //public static implicit operator CompanyEditViewModel(Company company)
        //{
        //    return new CompanyEditViewModel
        //    {
        //        Id = company.Id,
        //        Name = company.Name,
        //        OpenCompany = company.OpenCompany,
        //        Addres = company.Addres,
        //        Email = company.Email,
        //        TelNumber = company.TelNumber,
        //        PoctIndexId = company.PoctIndexId,
        //        PoctIndex = company.PoctIndex,
        //    };
        //}

    }
}
