using FinalProject.Areas.PayrollAdmin.Enum;
using FinalProject.Areas.PayrollAdmin.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.ViewModel
{
    public class EmployeeEditViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Firstname { get; set; }

        [Required, StringLength(100)]
        public string Lastname { get; set; }

        [Required, StringLength(100)]
        public string Fathername { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Birthday { get; set; }

        [Required, StringLength(200)]
        public string CurrentAddres { get; set; }

        [Required, StringLength(200)]
        public string Phone { get; set; }

        [Required, StringLength(200)]
        public string Email { get; set; }

        [Required, StringLength(200)]
        public string DistrictRegistration { get; set; }

        [Required, StringLength(200)]
        public string PassportNumber { get; set; }

        [Required, DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime PassportExpirationDate { get; set; }

        [Required, EnumDataType(typeof(Education))]
        public Education EducationType { get; set; }

        [Required, EnumDataType(typeof(MarialStatus))]
        public MarialStatus MarialStatusType { get; set; }

        [Required, EnumDataType(typeof(Gender))]
        public Gender GenderType { get; set; }

        [Required, EnumDataType(typeof(Status))]
        public Status StatusType { get; set; }

        [StringLength(200)]
        public string Image { get; set; }

        [NotMapped]
        public IFormFile Photo { get; set; }

        public IEnumerable<Employee> Employees { get; set; }
    }
}
