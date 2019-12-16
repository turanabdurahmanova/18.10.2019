using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.Enum;
using FinalProject.Areas.PayrollAdmin.ViewModel;
using Microsoft.AspNetCore.Http;

namespace FinalProject.Areas.PayrollAdmin.Models
{
    public class Employee
    {
        public Employee()
        {
            FormerWork = new HashSet<FormerWork>();
        }

        public int Id { get; set; }
        [Required, StringLength(100)]
        public string Firstname { get; set; }

        [Required, StringLength(100)]
        public string Lastname { get; set; }

        [Required, StringLength(100)]
        public string Fathername { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Birthday{ get; set; }

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

        [Required(ErrorMessage = "Select at least one option")]
        [Display(Name = "Education")]
        [EnumDataType(typeof(Education))]
        public Education EducationType { get; set; }

        [Required(ErrorMessage = "OrderAction must be selected"), EnumDataType(typeof(MarialStatus))]
        public MarialStatus MarialStatusType { get; set; }

        [Required(ErrorMessage = "OrderAction must be selected"), EnumDataType(typeof(Gender))]
        public Gender GenderType { get; set; }

        [Required(ErrorMessage = "OrderAction must be selected"), EnumDataType(typeof(Status))]
        public Status StatusType { get; set; }

        public string AppUserId { get; set; }

        public AppUser AppUser { get; set; }

        public ICollection<FormerWork> FormerWork { get; set; }

        public ICollection<Recruitment> Recruitments { get; set; }

        [StringLength(200)]
        public string Image { get; set; }

        [Required, NotMapped]
        public IFormFile Photo { get; set; }

        public static implicit operator EmployeeEditViewModel (Employee employee)
        {
            return new EmployeeEditViewModel
            {
                Firstname = employee.Firstname,
                Lastname = employee.Lastname,
                Fathername = employee.Fathername,
                Birthday = employee.Birthday,
                CurrentAddres = employee.CurrentAddres,
                DistrictRegistration = employee.DistrictRegistration,
                EducationType = employee.EducationType,
                Email = employee.Email,
                GenderType = employee.GenderType,
                MarialStatusType = employee.MarialStatusType,
                PassportExpirationDate = employee.PassportExpirationDate,
                PassportNumber = employee.PassportNumber,
                Phone = employee.Phone,
                Image = employee.Image
            };
        }
    }
}
