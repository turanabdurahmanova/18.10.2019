using FinalProject.Areas.PayrollAdmin.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinalProject.Areas.PayrollAdmin.DAL
{
    public class PayrollDbContext : IdentityDbContext<AppUser, AppRole, string,
                                                        IdentityUserClaim<string>, AppUserRole,
                                                        IdentityUserLogin<string>, IdentityRoleClaim<string>,
                                                        IdentityUserToken<string>>
    {
        public PayrollDbContext(DbContextOptions<PayrollDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public virtual DbSet<Continuity> Continuity { get; set; }
        public DbSet<Departmant> Departmants { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<FormerWork> FormerWorks { get; set; }
        public DbSet<Recruitment> Recruitments { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<ShopBonus> ShopBonus { get; set; }
        public DbSet<Vacation> Vacations { get; set; }
        public DbSet<Shop> Shops { get; set; }
        public DbSet<CompanyDepartament> CompanyDepartaments { get; set; }
        public DbSet<Salary> Salaries { get; set; }
        public DbSet<Bonus> Bonus { get; set; }
        public DbSet<PoctIndex> PoctIndex { get; set; }
        public DbSet<Penalty> Penalties { get; set; }
        public DbSet<CompanyProfit> CompanyProfit { get; set; }
        public DbSet<ShopProfit> ShopProfits { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }
        public DbSet<RecruitmentShopBonus> RecruitmentShopBonus { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                        .HasOne(au => au.Employee)
                            .WithOne(e => e.AppUser).
                                HasForeignKey<AppUser>(au => au.EmployeeId);

            builder.Entity<AppUserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.AppRole)
                    .WithMany(r => r.AppUserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.AppUser)
                    .WithMany(r => r.AppUserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<Bonus>().HasOne(b => b.Recruitment).WithMany(r => r.Bonus).HasForeignKey(b => b.RecruitmentId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CompanyDepartament>().HasKey(cd => new { cd.CompanyId, cd.DepartmantId });
            builder.Entity<CompanyDepartament>().HasOne(cd => cd.Departmant).WithMany(d => d.CompanyDepartaments).HasForeignKey(cd => cd.DepartmantId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CompanyDepartament>().HasOne(cd => cd.Company).WithMany(c => c.CompanyDepartaments).HasForeignKey(cd => cd.CompanyId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<CompanyProfit>().HasOne(cp => cp.Company).WithMany(c => c.CompanyProfits).HasForeignKey(cp => cp.CompanyId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Continuity>().HasOne(c => c.Recruitment).WithMany(r => r.Continuities).HasForeignKey(c => c.RecruitmentId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<FormerWork>().HasOne(f => f.Employee).WithMany(e => e.FormerWork).HasForeignKey(f => f.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Payroll>().HasOne(p => p.Recruitment).WithMany(r => r.Payrolls).HasForeignKey(c => c.RecruitmentId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Penalty>().HasOne(p => p.Recruitment).WithMany(r => r.Penalties).HasForeignKey(c => c.RecruitmentId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Position>().HasOne(p => p.Departmant).WithMany(d => d.Positions).HasForeignKey(p => p.DepartmantId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Recruitment>().HasOne(r => r.Employee).WithMany(e => e.Recruitments).HasForeignKey(r => r.EmployeeId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Recruitment>().HasOne(r => r.Position).WithMany(p => p.Recruitments).HasForeignKey(r => r.PositionId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Recruitment>().HasOne(r => r.Shop).WithMany(s => s.Recruitments).HasForeignKey(r => r.ShopId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Salary>().HasOne(s => s.Company).WithMany(c => c.Salaries).HasForeignKey(s => s.CompanyId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Salary>().HasOne(s => s.Position).WithMany(c => c.Salaries).HasForeignKey(s => s.PositionId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Shop>().HasOne(s => s.Company).WithMany(c => c.Shops).HasForeignKey(s => s.CompanyId).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Vacation>().HasOne(v => v.Recruitment).WithMany(r => r.Vacations).HasForeignKey(v => v.RecruitmentId).OnDelete(DeleteBehavior.Cascade);

        }

        public async Task SeedAsync(IServiceScope serviceScope)
        {
            var employeeAny = await Employees.AnyAsync();

            if (!employeeAny)
            {
                List<Employee> employees = new List<Employee>()
                {
                    new Employee()
                    {
                        Firstname = "Turan",
                        Lastname = "Abdurahmanova",
                        Fathername = "Chingiz",
                        Birthday = new DateTime(1996, 09, 11),
                        CurrentAddres = "Bakı şəhər, Yasamal qəsəbəsi, alatava iki ev 74",
                        DistrictRegistration = "Zaqatala rayon, Bəhmətli kəndi",
                        PassportNumber = "14406809",
                        PassportExpirationDate = new DateTime(2021, 09, 11),
                        Image = "employee/turan.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 50 350 42 90",
                        Email = "turancha@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Fidan",
                        Lastname = "Rzayeva",
                        Fathername = "Nadir",
                        Birthday = new DateTime(2019,05,31),
                        CurrentAddres = "Nərimanov rayon, Həsən əliyev keçəsi N514",
                        DistrictRegistration = "Cəlilabad rayon, Privonloye kəndi",
                        PassportNumber = "N 15174901",
                        PassportExpirationDate = new DateTime(2022,06,15),
                        Image = "employee/fidan.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 50 992 31 54",
                        Email = "fidanrz@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Dinara",
                        Lastname = "Mustafayeva",
                        Fathername = "Vilayət",
                        Birthday = new DateTime(1996,03,09),
                        CurrentAddres = "Bakı şəh., Yasamal ray., Abdulla Şaiq ev 154",
                        DistrictRegistration = "Şəmkir şəh., M.Əhmədov küç., ev 12",
                        PassportNumber = "AZE 09157616",
                        PassportExpirationDate = new DateTime(2021,03,09),
                        Image = "employee/dinara.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 699 49 97",
                        Email = "dinaramustafayeva39@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Həsən",
                        Lastname = "Nağıyev",
                        Fathername = "Vahid",
                        Birthday = new DateTime(1999,07,03),
                        CurrentAddres = "Bakı şəh., Yasamal ray., Abdulla Şaiq ev 155",
                        DistrictRegistration = "Azərbaycan, Yevlax şəh. ",
                        PassportNumber = "AZE № 15295196",
                        PassportExpirationDate = new DateTime(2021,03,09),
                        Image = "employee/hesen.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 55 779 49 23",
                        Email = "dinaramustafayeva39@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Mətin",
                        Lastname = "Nuri",
                        Fathername = "Natiq",
                        Birthday = new DateTime(1998,11,15),
                        CurrentAddres = "Binəqədi rayon, Biləcəri qəsəbəsi, Yeni yaşayış massivi",
                        DistrictRegistration = "Şəki rayon, Qoxmuq kəndi",
                        PassportNumber = "№ 00105801",
                        PassportExpirationDate = new DateTime(2022,06,14),
                        Image = "employee/metin.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 70 200 92 60",
                        Email = "metinnn@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Məhəmmədqulu",
                        Lastname = "Novruzov",
                        Fathername = "Azər",
                        Birthday = new DateTime(1998,07,01),
                        CurrentAddres = "Azərbaycan, Fizuli ray., Sərdərli K",
                        DistrictRegistration = "Bakı şəhəri, Yeni yasamal qəsəbəsi",
                        PassportNumber = "AZE № 14170959",
                        PassportExpirationDate = new DateTime(2023,07,01),
                        Image = "employee/qulu.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 50 700 77 02",
                        Email = "mahammadquluna@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Türkan",
                        Lastname = "Nasirli",
                        Fathername = "Əflatun",
                        Birthday = new DateTime(1997,7,01),
                        CurrentAddres = "Bakı şəhəri, Yeni yasamal qəsəbəsi",
                        DistrictRegistration = "Şəki rayon, Vərəzət kəndi",
                        PassportNumber = " № 00105802",
                        PassportExpirationDate = new DateTime(2022,06,14),
                        Image = "employee/turkan.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 55 988 28 69",
                        Email = "turkanna@code.edu.az"

                    },
                    new Employee
                    {
                        Firstname = "Afiq",
                        Lastname = "Mehdizadə",
                        Fathername = "Nazim",
                        Birthday = new DateTime(1994,06,14),
                        CurrentAddres = "Bakı şəhəri, Naximov 33, Ev 51",
                        DistrictRegistration = "Biləsuvar rayonu, Mübariz İbrahimov küçəsi",
                        PassportNumber = " № 00105802",
                        PassportExpirationDate = new DateTime(2022,06,14),
                        Image = "employee/afiq.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 509 05 15",
                        Email = "afigmn@code.edu.az"

                    },
                    new Employee
                    {
                        Firstname = "Məryam",
                        Lastname = "Ələkbərli",
                        Fathername = "Yaşar",
                        Birthday = new DateTime(1999,06,14),
                        CurrentAddres = "Baki şəhəri, Suraxanı rayonu, Tofiq Bayramov küçəsi ev №38",
                        DistrictRegistration = "Baki şəhəri, Suraxanı rayonu",
                        PassportNumber = "№ 00105801",
                        PassportExpirationDate = new DateTime(2027,06,14),
                        Image = "employee/meryam.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 50 899 46 16",
                        Email = "maryamya@code.edu.az"

                    },
                    new Employee
                    {
                        Firstname = "Ravil",
                        Lastname = "Yahyayev",
                        Fathername = "İlqar",
                        Birthday = new DateTime(1997,07,17),
                        CurrentAddres = "Bakı şəhəri, Adil Məmmədov küçəsi 11",
                        DistrictRegistration = "Şamaxı rayonu, Göylər kəndi",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/ravil.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 637 87 27",
                        Email = "ravilyi@code.edu.az"

                    },
                    new Employee
                    {
                        Firstname = "Əmrah",
                        Lastname = "Hüseynov",
                        Fathername = "Zakir",
                        Birthday = new DateTime(1994,09,2),
                        CurrentAddres = "Bakı şəhəri, Adil Məmmədov küçəsi 11",
                        DistrictRegistration = "Naxçıvan",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/emrah.jpg",
                        MarialStatusType = Enum.MarialStatus.Evli,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 55 582 15 66",
                        Email = "emrahhz@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Fuad",
                        Lastname = "Qasımov",
                        Fathername = "Natiq",
                        Birthday = new DateTime(1997,09,1),
                        CurrentAddres = "Bakı şəhəri Nizami rayonu, B.Nuriyev 65, 66.",
                        DistrictRegistration = "Bakı şəhəri Nizami rayonu, B.Nuriyev 65, 66.",
                        PassportNumber = "AZE 13760250",
                        PassportExpirationDate = new DateTime(2022,09,01),
                        Image = "employee/fuad.jpg",
                        MarialStatusType = Enum.MarialStatus.BaşıBağlı,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 55 534 34 53",
                        Email = "fuadng@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Resul",
                        Lastname = "Ağarızayev",
                        Fathername = "....",
                        Birthday = new DateTime(1999,03,8),
                        CurrentAddres = "Bakı şəhəri, Adil Məmmədov küçəsi 11",
                        DistrictRegistration = "Quba",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/resul.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 55 758 04 85",
                        Email = "rasula@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Samir",
                        Lastname = "Dadaşzadə",
                        Fathername = "....",
                        Birthday = new DateTime(1991,11,20),
                        CurrentAddres = "Bakı şəhəri, cccc",
                        DistrictRegistration = "şşşççç",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/samir.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 55 654 94 92",
                        Email = "samir.d@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Aqil",
                        Lastname = "Atakişiyev",
                        Fathername = "....",
                        Birthday = new DateTime(1999,01,26),
                        CurrentAddres = "Bakı şəhəri, cccc",
                        DistrictRegistration = "şşşççç",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/aqil.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 55 686 62 84",
                        Email = "aqilad@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Ətraf",
                        Lastname = "İsayev",
                        Fathername = "Məmməd",
                        Birthday = new DateTime(1997,04,12),
                        CurrentAddres = "Bakı şəhəri, cccc",
                        DistrictRegistration = "şşşççç",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/etraf.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Yoxdur,
                        Phone = "+994 55 686 62 84",
                        Email = "etraf@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Könül",
                        Lastname = "Məmmədova",
                        Fathername = "Məmməd",
                        Birthday = new DateTime(1995,04,11),
                        CurrentAddres = "Sumqatıt şəhəri",
                        DistrictRegistration = "Zaqatala",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/konul.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 55 686 62 84",
                        Email = "konul@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Kübalə",
                        Lastname = "Yusifova",
                        Fathername = "Samir",
                        Birthday = new DateTime(1999,08,11),
                        CurrentAddres = "Bakı şəhəri",
                        DistrictRegistration = "Zaqatala",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/kubale.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 55 686 62 84",
                        Email = "kubale@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Aynure",
                        Lastname = "Jafarova",
                        Fathername = "Fikrət",
                        Birthday = new DateTime(1994,04,20),
                        CurrentAddres = "Bakı şəhəri, Ə.Naxçıvani, 39 A, m.7",
                        DistrictRegistration = "Bakı şəhəri, Ə.Naxçıvani, 39 A, m.7",
                        PassportNumber = "AA07997669",
                        PassportExpirationDate = new DateTime(2029,05,21),
                        Image = "employee/aynure.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 55 686 62 84",
                        Email = "aynurafj@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Rakif",
                        Lastname = "Ramazanov",
                        Fathername = "Samir",
                        Birthday = new DateTime(1998,04,11),
                        CurrentAddres = "Sumqayıt şəhəri",
                        DistrictRegistration = "Lənkəran",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/rakif.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 678 76 28",
                        Email = "rakifgr@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Günel",
                        Lastname = "Məmmədova",
                        Fathername = "Ulvi",
                        Birthday = new DateTime(1999,09,11),
                        CurrentAddres = "Sumqayıt şəhəri",
                        DistrictRegistration = "Sumqayıt",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/gunel1.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 493 59 10",
                        Email = "gunelum@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Murad",
                        Lastname = "Nasibli",
                        Fathername = "İlqar",
                        Birthday = new DateTime(1999,09,16),
                        CurrentAddres = "Bakı şəhəri, Xətai rayonu, Sabit orucov küçəsi 2",
                        DistrictRegistration = "Bakı şəhəri, Xətai rayonu, Sabit orucov küçəsi 2",
                        PassportNumber = "AZE 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/murad.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 973 08 27",
                        Email = "muradin@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Rafet",
                        Lastname = "Rzayev",
                        Fathername = "Raset",
                        Birthday = new DateTime(1997,01,25),
                        CurrentAddres = "Bakı şəhəri",
                        DistrictRegistration = "İmişli",
                        PassportNumber = "AZE 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/rafet.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 496 20 75",
                        Email = "rafetrza@outlook.com"
                    },
                    new Employee
                    {
                        Firstname = "Kənan",
                        Lastname = "Rüstəmov",
                        Fathername = "Devran",
                        Birthday = new DateTime(1996,10,20),
                        CurrentAddres = "Polşa, Ul strajku lodzkich studentow 1981 r1",
                        DistrictRegistration = "Bakı şəhər, Qaracuxur qəsəbəsi, V.Quliyev küçəsi, 18/18",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2021,10,20),
                        Image = "employee/kenan.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 51 366 00 14",
                        Email = "kenanrustem20@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Orxan",
                        Lastname = "Şabanov",
                        Fathername = "Bəşir",
                        Birthday = new DateTime(1994,04,06),
                        CurrentAddres = "Omiski",
                        DistrictRegistration = "Zaqatala rayon Bəhmətli kəndi",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/orxan.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Yoxdur,
                        Phone = "+994 55 881 52 40",
                        Email = "orhan@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Bəşir",
                        Lastname = "Azizov",
                        Fathername = "Cavid",
                        Birthday = new DateTime(1999,09,11),
                        CurrentAddres = "Bakı şəhər, Yasamal qəsəbəsi, Bina 321",
                        DistrictRegistration = "Xaqan Dadaşov 237",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/besir.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 516 60 79",
                        Email = "bashir@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Nicat",
                        Lastname = "Əliyev",
                        Fathername = "Şəadət",
                        Birthday = new DateTime(1989,02,06),
                        CurrentAddres = "Azərbaycan, Abşeron rayon, Xırdalan qəsəbəsi",
                        DistrictRegistration = "Bakı şəh., Nərimanov ray., A.Neymətulla küç, ev.52,m.10",
                        PassportNumber = "AZE № 14837122",
                        PassportExpirationDate = new DateTime(2024,02,06),
                        Image = "employee/nicat.jpg",
                        MarialStatusType = Enum.MarialStatus.Evli,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 70 880 20 06",
                        Email = "nijatsh@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Aziz",
                        Lastname = "Elkhanoglu",
                        Fathername = "Bəşir",
                        Birthday = new DateTime(1987,09,11),
                        CurrentAddres = "Bakı",
                        DistrictRegistration = "Bakı",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/aziz.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 70 880 20 06",
                        Email = "aziz@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Real",
                        Lastname = "Hüseynov",
                        Fathername = "Sənani",
                        Birthday = new DateTime(1997,02,25),
                        CurrentAddres = "Bakı, Binəqədi rayonu",
                        DistrictRegistration = "Ucar rayonu, Qaradağlı kəndi",
                        PassportNumber = "AZE13635321",
                        PassportExpirationDate = new DateTime(2022,02,25),
                        Image = "employee/real.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 70 202 02 45",
                        Email = "realhuseynli@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Şəmistan",
                        Lastname = "Vəliyev",
                        Fathername = "Məhəmməd",
                        Birthday = new DateTime(1997,01,20),
                        CurrentAddres = "Bakı şəhər, Yasamal qəsəbəsi, Şərifzadə  ev 42.",
                        DistrictRegistration = "Ağdaş rayon, Ləki qəsəbəsi",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/semistan.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Yoxdur,
                        Phone = "+994 70 880 20 06",
                        Email = "shamistanmv@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Yasin",
                        Lastname = "Uğur",
                        Fathername = "Bəşir",
                        Birthday = new DateTime(2002,09,11),
                        CurrentAddres = "Bakı",
                        DistrictRegistration = "Bakı",
                        PassportNumber = "№ 00105802",
                        PassportExpirationDate = new DateTime(2019,08,15),
                        Image = "employee/yasin.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 70 880 20 06",
                        Email = "yasin@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Elvin",
                        Lastname = "Məmmədov",
                        Fathername = "Eyvaz",
                        Birthday = new DateTime(1998,12,16),
                        CurrentAddres = "Bakı şəhər, Nizami rayon, Behruz Quluyev küçəsi",
                        DistrictRegistration = "Azərbaycan, Ağdam rayon, Xıdırlı kəndi",
                        PassportNumber = "AZE № 04040938",
                        PassportExpirationDate = new DateTime(2023,12,16),
                        Image = "employee/elvin1.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 70 880 20 06",
                        Email = "elvinem@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Fremk",
                        Lastname = "Scofield",
                        Fathername = "Brain",
                        Birthday = new DateTime(1994,03,28),
                        CurrentAddres = "Bakı şəhər, Nizami rayon, Bəhruz Nuruyev 32",
                        DistrictRegistration = "Baki şəhər, Xətai rayon, Xudu Məmmədov 1244 A 136",
                        PassportNumber = "AZE № 008572AA",
                        PassportExpirationDate = new DateTime(2023,03,28),
                        Image = "employee/kamran.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Yoxdur,
                        Phone = "+994 55 554 65 65 ",
                        Email = "kamranaa@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Elvin",
                        Lastname = "Əkbərov",
                        Fathername = "Nuraddin",
                        Birthday = new DateTime(1998,04,09),
                        CurrentAddres = "Bakı şəhər, Xətai rayon, Aşıq Qurban küçəsi, ev 25V",
                        DistrictRegistration = "Bakı şəhər, Xətai rayon, Aşıq Qurban küçəsi, ev 25V",
                        PassportNumber = "AZE № 00105802",
                        PassportExpirationDate = new DateTime(2026,04,09),
                        Image = "employee/elvin2.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 70 708 80 06",
                        Email = "elvinna@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Cavid",
                        Lastname = "Bəşirov",
                        Fathername = "Ceyhun",
                        Birthday = new DateTime(1993,05,10),
                        CurrentAddres = "Bakı şəhər, Xətai rayon, Telnov-10, m-1",
                        DistrictRegistration = "Siyəzən şəhər, T.İsmayılov 19.m-12",
                        PassportNumber = "AZE № 04758745",
                        PassportExpirationDate = new DateTime(2028,05,10),
                        Image = "employee/yasin.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 561 38 83",
                        Email = "javidjb@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Rəşad",
                        Lastname = "Gözəlov",
                        Fathername = "Vahib",
                        Birthday = new DateTime(1991,12,23),
                        CurrentAddres = "Bakı şəhər, Nəsimi rayon, M.Mirqasimov 5",
                        DistrictRegistration = "Bakı şəhər, Sabuncu rayon, Bakixanov qəs., H.Agayev15",
                        PassportNumber = "AZE № 17144796",
                        PassportExpirationDate = new DateTime(2027,12,12),
                        Image = "employee/resad.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 50 765 05 04",
                        Email = "rashadvg@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Kənan",
                        Lastname = "Behbudov",
                        Fathername = "Novruz",
                        Birthday = new DateTime(1996,09,29),
                        CurrentAddres = "Bakı şəhər, Nerimanov Həsən Əliyev 94B",
                        DistrictRegistration = "Bakı şəhər, Yevlax Rayonu İ.Dəmirov 67",
                        PassportNumber = "AZE № 17144796",
                        PassportExpirationDate = new DateTime(2022,09,29),
                        Image = "employee/kenan2.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Texnikum,
                        Phone = "+994 55 952 24 42",
                        Email = "kanannbe@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Rüstəm",
                        Lastname = "Fətullayev",
                        Fathername = "Rəşid",
                        Birthday = new DateTime(1999,10,04),
                        CurrentAddres = "Bakı şəhər, M. Rüstəmov 35 mən 35",
                        DistrictRegistration = "Bakı şəhər, Naftalan rayonu",
                        PassportNumber = "AZE № 15346605",
                        PassportExpirationDate = new DateTime(2022,09,29),
                        Image = "employee/rustem.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 55 822 48 15",
                        Email = "rustamrf@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Pərvin",
                        Lastname = "İbadullayev",
                        Fathername = "Şahin",
                        Birthday = new DateTime(1997,06,18),
                        CurrentAddres = "Bakı şəhər, Məmməd Cəfər Cəfərov küçəsi 16, menzil 174",
                        DistrictRegistration = "Bakı şəhər, Məmməd Cəfər Cəfərov küçəsi 16, menzil 174",
                        PassportNumber = "AZE № 15346605",
                        PassportExpirationDate = new DateTime(2022,06,18),
                        Image = "employee/pervin.jpg",
                        MarialStatusType = Enum.MarialStatus.BaşıBağlı,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 55 304 95 12",
                        Email = "Ibdpervin@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Pərviz",
                        Lastname = "Muxçanov",
                        Fathername = "Hidayət",
                        Birthday = new DateTime(1996,03,20),
                        CurrentAddres = "Bakı şəhər, Bakıxanov küçəsi 31",
                        DistrictRegistration = "Bakı şəhər, Sahib Zeynalov 15",
                        PassportNumber = "AZE № 15346605",
                        PassportExpirationDate = new DateTime(2022,03,20),
                        Image = "employee/perviz.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Yoxdur,
                        Phone = "+994 51 414 40 16",
                        Email = "muxcanovparviz@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Nazlı",
                        Lastname = "Mahmudlu",
                        Fathername = "Sevdiyar",
                        Birthday = new DateTime(1996,12,29),
                        CurrentAddres = "Bakı şəhər, Həsənbəy Zərdabi 97",
                        DistrictRegistration = "Wüstenrotstraße 16, 4020 Linz, Austria",
                        PassportNumber = "AZE № 15346605",
                        PassportExpirationDate = new DateTime(2022,03,20),
                        Image = "employee/nazli.jpg",
                        MarialStatusType = Enum.MarialStatus.Evli,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 70 341 15 05",
                        Email = "Mahmudlunaz@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Səbuhi",
                        Lastname = "Şəmiyev",
                        Fathername = "Hikmət",
                        Birthday = new DateTime(1992,07,18),
                        CurrentAddres = "Bakı şəhər, Nərimanov rayon, Əhməd Rəcəbli küçəsi, Bina 10, Mənzil 22",
                        DistrictRegistration = "Azərbaycan, Oğuz rayon, Xalxal kəndi",
                        PassportNumber = "AZE № 16984447",
                        PassportExpirationDate = new DateTime(2030,07,18),
                        Image = "employee/sebuhi.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 51 540 54 72",
                        Email = "SabuhiHsh@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Elnur",
                        Lastname = "Soltanov",
                        Fathername = "Elşən",
                        Birthday = new DateTime(1991,05,02),
                        CurrentAddres = "Bakı şəhər, Nərimanov rayon, Əhməd Rəcəbli küçəsi, Bina 10, Mənzil 22",
                        DistrictRegistration = "Agdam, Suraxanı rayonu, Qaraçuxur Qəsəbəsi",
                        PassportNumber = "AZE № 15346605",
                        PassportExpirationDate = new DateTime(2031,05,02),
                        Image = "employee/elnur.jpg",
                        MarialStatusType = Enum.MarialStatus.Evli,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 55 966 10 16",
                        Email = "Soltanov.e.e@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Hacı",
                        Lastname = "Mammadli",
                        Fathername = "Emil",
                        Birthday = new DateTime(1995,11,26),
                        CurrentAddres = "Bakı şəhər, Nərimanov rayon, Faiq Yusufov küçəsi, Mənzil 43",
                        DistrictRegistration = "Bakı şəhər, Nərimanov rayon, Faiq Yusufov küçəsi, Mənzil 43",
                        PassportNumber = "AZE № 15346605",
                        PassportExpirationDate = new DateTime(2020,11,26),
                        Image = "employee/haci.jpg",
                        MarialStatusType = Enum.MarialStatus.BaşıBağlı,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 50 882 52 77",
                        Email = "hajiem@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Valeh",
                        Lastname = "Hüseynov",
                        Fathername = "Məmmədəmin",
                        Birthday = new DateTime(1995,06,03),
                        CurrentAddres = "Bakı şəhər, Yasamal qəsəbəsi, Həsən bəy Zərdabi 92",
                        DistrictRegistration = "Azərbaycan, Ağdaş şəhər",
                        PassportNumber = "AZE № 15346605",
                        PassportExpirationDate = new DateTime(2020,11,26),
                        Image = "employee/valeh.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 50 404 82 11",
                        Email = "valehmh@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Mədinə",
                        Lastname = "Əliyeva",
                        Fathername = "İsfəndiyar",
                        Birthday = new DateTime(1999,10,06),
                        CurrentAddres = "Bakı şəhər, Sabunçu rayon, R.Qemberov küçəsi 8, Mənzil 27 ",
                        DistrictRegistration = "Bakı şəhər, Sabunçu rayon, R.Qemberov küçəsi 8, Mənzil 27 ",
                        PassportNumber = "AZE № 15346605",
                        PassportExpirationDate = new DateTime(2020,11,26),
                        Image = "employee/medine.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 50 343 66 22",
                        Email = "medine_1999@inbox.ru"
                    },
                    new Employee
                    {
                        Firstname = "Nuranə",
                        Lastname = "Pənahlı ",
                        Fathername = "Nadir",
                        Birthday = new DateTime(1999,08,02),
                        CurrentAddres = "Bakı şəhər, Yasamal rayon, H.Zərdabi 92 ",
                        DistrictRegistration = "Bakı şəhər, Yasamal rayon, H.Zərdabi 92 ",
                        PassportNumber = "AZE № 15346605",
                        PassportExpirationDate = new DateTime(2024,08,02),
                        Image = "employee/nurana.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 70 633 49 64",
                        Email = "nurananp@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Cəlil",
                        Lastname = "Süleymanlı ",
                        Fathername = "Faiq",
                        Birthday = new DateTime(1999,05,16),
                        CurrentAddres = "Bakı şəhər, Binəqədi rayon, Rəsulzadə qəsəbəsi, İlham Haciyev küçəsi, ev 13",
                        DistrictRegistration = "Bakı şəhər, Binəqədi rayonu, Rəsulzadə qəsəbəsi, İlham Haciyev küçəsi, ev 13",
                        PassportNumber = "AZE № 15070089",
                        PassportExpirationDate = new DateTime(2024,05,16),
                        Image = "employee/celil.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 50 324 81 32",
                        Email = "jalilfs@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Ülkər",
                        Lastname = "Axverdiyeva",
                        Fathername = "Rövşən",
                        Birthday = new DateTime(1997,01,01),
                        CurrentAddres = "Bakı şəhər, Yasamal rayon, Mireli Seyidov 28/30",
                        DistrictRegistration = "Şirvan şəhəri, Şirvan küçəsi ev 20",
                        PassportNumber = "AZE № 13310537",
                        PassportExpirationDate = new DateTime(2022,01,01),
                        Image = "employee/ulker.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 51 649 87 79",
                        Email = "ulkar.axverdiyeva@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Günel",
                        Lastname = "Heydərova",
                        Fathername = "Samid",
                        Birthday = new DateTime(1996,11,26),
                        CurrentAddres = "Bakı şəhər, Yasamal rayon, Cəfərcabbarlı küçəsi, bina 2, menzil 11",
                        DistrictRegistration = "Ağdaş rayon, Xətai küçəsi, ev 20",
                        PassportNumber = "AZE № 13387759",
                        PassportExpirationDate = new DateTime(2021,11,26),
                        Image = "employee/gunel2.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 51 689 13 63",
                        Email = "gunelheyderova19@gmail.com "
                    },
                    new Employee
                    {
                        Firstname = "Elşad",
                        Lastname = "Rüstəmov",
                        Fathername = "Zülfüqar",
                        Birthday = new DateTime(1997,03,01),
                        CurrentAddres = "Bakı şəhər, Nəsimi rayon, bina 81, bina 81, menzil 52",
                        DistrictRegistration = "Şəmkir rayon, Göyməmmədli kəndi",
                        PassportNumber = "AZE № 13195850.",
                        PassportExpirationDate = new DateTime(2022,03,01),
                        Image = "employee/elsad.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 309 71 25",
                        Email = "elshadzr@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Gültac",
                        Lastname = "Zeynalzadə",
                        Fathername = "Zeynalabdin",
                        Birthday = new DateTime(1996,07,20),
                        CurrentAddres = "Azərbaycan, Baki sheher, Abay Kunanbayev kucesi, bina 24, ev 10",
                        DistrictRegistration = "Naxçıvan MR, Şahbuz rayon, Mahmudoba kəndi",
                        PassportNumber = "AZE № 09422412",
                        PassportExpirationDate = new DateTime(2021,07,20),
                        Image = "employee/gultac.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 51 309 71 25",
                        Email = "gultaczeynalzade@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Şəbnəm",
                        Lastname = "Bağırova",
                        Fathername = "Yalçın",
                        Birthday = new DateTime(1999,07,18),
                        CurrentAddres = "Bakı şəhər, Binəqədi rayon, Biləcəri qəsəbəsi, Ordubadi küçəsi",
                        DistrictRegistration = "Naxçıvan Mr, Naxçıvan şəhəri, H.Cavid küçəsi",
                        PassportNumber = "AZE № 15685088",
                        PassportExpirationDate = new DateTime(2024,07,18),
                        Image = "employee/sebnem.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 333 32 61",
                        Email = "7.sebnem@mail.ru"
                    },
                    new Employee
                    {
                        Firstname = "Elxan",
                        Lastname = "Oruclu",
                        Fathername = "Mübariz",
                        Birthday = new DateTime(1993,12,13),
                        CurrentAddres = "Bakı şəhər, Yasamal rayon, Metbuat pr 58c",
                        DistrictRegistration = "Sabunçu rayon, Bakıxanov qəsəbəsi",
                        PassportNumber = "AA 0218976",
                        PassportExpirationDate = new DateTime(2029,01,30),
                        Image = "employee/elxan.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Kişi,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 55 831 40 07",
                        Email = "elkhanmo@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Məlihə ",
                        Lastname = "Hüseynzadə ",
                        Fathername = "Səyavuş",
                        Birthday = new DateTime(1996,12,13),
                        CurrentAddres = "Bakı şəhər, Yasamal rayon, Metbuat pr 56c",
                        DistrictRegistration = "Bakı şəhər, Yasamal rayon, Metbuat pr 56c",
                        PassportNumber = "AA 0218976",
                        PassportExpirationDate = new DateTime(2021,12,13),
                        Image = "employee/melihe.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 77 454 93 97",
                        Email = "malihahusenyzade@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Elnarə",
                        Lastname = "Quliyeva",
                        Fathername = "Elşən",
                        Birthday = new DateTime(1996,09,11),
                        CurrentAddres = "Bakı şəhər, Asif Məhərrəmov küçəsi 52, mənzil 27",
                        DistrictRegistration = "Azərbaycan, Agcabedi rayon, Xocavend kendi",
                        PassportNumber = "AZE № 13470386",
                        PassportExpirationDate = new DateTime(2021,09,11),
                        Image = "employee/elnare.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 51 738 67 95",
                        Email = "elnaraquliyeva141@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Mina",
                        Lastname = "Fərzəli",
                        Fathername = "Faiq",
                        Birthday = new DateTime(1999,08,26),
                        CurrentAddres = "Azərbaycan, Sumqayıt şəhər, 48-ci məhlə, ev 3, mənzil 68",
                        DistrictRegistration = "Azərbaycan, Sumqayıt şəhər, 48-ci məhlə, ev 3, mənzil 68",
                        PassportNumber = "AZE № 0091625",
                        PassportExpirationDate = new DateTime(2024,08,26),
                        Image = "employee/minaye.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 55 928 87 55",
                        Email = "minayaff@code.edu.az"
                    },
                    new Employee
                    {
                        Firstname = "Səbinə",
                        Lastname = "Əzimova",
                        Fathername = "Sabit",
                        Birthday = new DateTime(1997,01,01),
                        CurrentAddres = "Azərbaycan, Bakı şəhəri, Məzahir Rüstəmov küçəsi 14, mənzil 56",
                        DistrictRegistration = "Azərbaycan, Ağdam şəhəri, Ə.Bayramov küçəsi 69",
                        PassportNumber = "AZE № 13101928",
                        PassportExpirationDate = new DateTime(2022,01,01),
                        Image = "employee/sebine.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 51 333 17 26",
                        Email = "azimova.sabina7@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Durna",
                        Lastname = "Zeynallı",
                        Fathername = "Zeynalabdin",
                        Birthday = new DateTime(1997,08,22),
                        CurrentAddres = "Azərbaycan, Baki sheher, Abay Kunanbayev kucesi, bina 24, ev 10",
                        DistrictRegistration = "Naxçıvan MR, Şahbuz rayon, Mahmudoba kəndi",
                        PassportNumber = "AZE № 09423383",
                        PassportExpirationDate = new DateTime(2022,08,22),
                        Image = "employee/durna.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 50 867 40 70",
                        Email = "durnazeynalli@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Nərmin",
                        Lastname = "Osmanova",
                        Fathername = "Natiq",
                        Birthday = new DateTime(1996,12,20),
                        CurrentAddres = "Azərbaycan, Baki sheher, Abay Kunanbayev kucesi, bina 24, ev 10",
                        DistrictRegistration = "Azərbaycan, Zaqatala rayonu A.Tala kendi",
                        PassportNumber = "AA 049u874",
                        PassportExpirationDate = new DateTime(2029,08,20),
                        Image = "employee/nermin1.jpg",
                        MarialStatusType = Enum.MarialStatus.Evli,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Bakalavr,
                        Phone = "+994 55 952 01 85",
                        Email = "nermink141@gmail.com"
                    },
                    new Employee
                    {
                        Firstname = "Nərmin",
                        Lastname = "Məmmədli",
                        Fathername = "Zahid",
                        Birthday = new DateTime(1996,01,14),
                        CurrentAddres = "Azərbaycan, Siyezen, Nizami 28",
                        DistrictRegistration = "Azərbaycan, Siyezen, Nizami 28",
                        PassportNumber = "AA 0545542",
                        PassportExpirationDate = new DateTime(2029,08,20),
                        Image = "employee/nermin2.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 55 952 01 85",
                        Email = "nermin.memmedli.96@inbox.ru"
                    },
                    new Employee
                    {
                        Firstname = "Sara",
                        Lastname = "Əhmədova",
                        Fathername = "Nadir",
                        Birthday = new DateTime(1997,05,19),
                        CurrentAddres = "Azərbaycan, Siyezen, Nizami 28",
                        DistrictRegistration = "Azərbaycan, Siyezen, Nizami 28",
                        PassportNumber = "AA 0545542",
                        PassportExpirationDate = new DateTime(2029,08,20),
                        Image = "employee/nermin2.jpg",
                        MarialStatusType = Enum.MarialStatus.Subay,
                        GenderType = Enum.Gender.Qadin,
                        EducationType = Enum.Education.Magistr,
                        Phone = "+994 55 952 01 85",
                        Email = "nermin.memmedli.96@inbox.ru"
                    },
                };

                await Employees.AddRangeAsync(employees);

                await SaveChangesAsync();
            }

            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

            var roleAny = await Roles.AnyAsync();

            if (!roleAny)
            {
                string[] roles = { "Admin", "HR", "Payroll Specialist", "Department Head" };

                foreach (string role in roles)
                {
                    AppRole appRole = new AppRole()
                    {
                        Name = role
                    };

                    await roleManager.CreateAsync(appRole);
                }
            }

            var userAny = await Users.AnyAsync();

            if (!userAny)
            {
                Recruitment admin = await Recruitments.FirstOrDefaultAsync(e => e.Employee.Email == "turancha@code.edu.az");
                Recruitment hr = await Recruitments.FirstOrDefaultAsync(e => e.Employee.Email == "matinnn@code.edu.az");
                Recruitment departmentHead = await Recruitments.FirstOrDefaultAsync(e => e.Employee.Email == "fuadng@code.edu.az");
                Recruitment payrollSpecialist = await Recruitments.FirstOrDefaultAsync(e => e.Employee.Email == "fidanrz@code.edu.az");

                if (admin != null)
                {
                    AppUser dbadmin = new AppUser()
                    {
                        Email = "jchturan@gmail.com",
                        UserName = "admin",
                        PhoneNumber = "+994 50 350 42 90",
                        EmployeeId = admin.EmployeeId
                    };

                    IdentityResult result = await userManager.CreateAsync(dbadmin, "Turan123@");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(dbadmin, "Admin");
                    }
                }

                if (hr != null)
                {
                    AppUser dbhr = new AppUser()
                    {
                        Email = "matinnn@code.edu.az",
                        UserName = "hr",
                        PhoneNumber = "+994 70 200 92 60",
                        EmployeeId = hr.EmployeeId
                    };

                    IdentityResult result = await userManager.CreateAsync(dbhr, "Metin123@");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(dbhr, "HR");
                    }
                }

                if (departmentHead != null)
                {
                    AppUser dbdepartmentHead = new AppUser()
                    {
                        Email = "fuadng@code.edu.az",
                        UserName = "department head",
                        PhoneNumber = "+994 55 534 34 53",
                        EmployeeId = departmentHead.EmployeeId
                    };

                    IdentityResult result = await userManager.CreateAsync(dbdepartmentHead, "Fuad123@");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(dbdepartmentHead, "Department Head");
                    }
                }

                if (payrollSpecialist != null)
                {
                    AppUser dbpayrollSpecialist = new AppUser()
                    {
                        Email = "fidanrz@code.edu.az",
                        UserName = "payroll Specialist",
                        PhoneNumber = "+994 50 992 31 54",
                        EmployeeId = payrollSpecialist.EmployeeId
                    };

                    IdentityResult result = await userManager.CreateAsync(dbpayrollSpecialist, "Fidan123@");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(dbpayrollSpecialist, "Payroll Specialist");
                    }
                }

            }

            var poctIndex = await PoctIndex.AnyAsync();
            if (!poctIndex)
            {
                List<PoctIndex> poctIndices = new List<PoctIndex>
                {
                    new PoctIndex
                    {
                        Name = "2 saylı poçt filialının 134 saylı poçt şöbəsi"
                    },
                      new PoctIndex
                    {
                        Name = "2 saylı poçt filialının 136 saylı poçt şöbəsi"
                    },
                        new PoctIndex
                    {
                        Name = "2 saylı poçt filialının 138 saylı poçt şöbəsi"
                    },
                          new PoctIndex
                    {
                        Name = "138 saylı poçt şöbəsinin 1 saylı poçt agentliyi"
                    },
                            new PoctIndex
                    {
                        Name = "2 saylı poçt filialının 141 saylı poçt şöbəsi"
                    },
                              new PoctIndex
                    {
                        Name = "Şərur Poçt Filialının Kərimbəyli poçt bölməsı"
                    },
                    new PoctIndex
                    {
                        Name = "Şərur Poçt Filialının Danyeri kənd poçt bölməsı"
                    },
                    new PoctIndex
                    {
                        Name = "Şərur Poçt Filialının Danyeri kənd poçt bölməsı"
                    }
                };

                await PoctIndex.AddRangeAsync(poctIndices);
                await SaveChangesAsync();

            }

            var company = await Companies.AnyAsync();
            if (!company)
            {
                List<Company> companies = new List<Company>()
                    {
                        new Company
                        {
                            Name = "Apple",
                            PoctIndexId = 1,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Google",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Sumsung",
                            PoctIndexId = 3,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Microsoft",
                            PoctIndexId = 4,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Sinteks",
                            PoctIndexId = 5,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Adidas",
                            PoctIndexId = 6,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Nike",
                            PoctIndexId = 7,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Paşa Holding",
                            PoctIndexId = 8,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Google",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Wrizty",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Voatı",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Ziolty",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Korbax",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Donicy",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Konıry",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Foroly",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Gariox",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Yıgatı",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Heanco",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Hariox",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                        new Company
                        {
                            Name = "Mengary",
                            PoctIndexId = 2,
                            OpenCompany = new DateTime(2012, 11, 01),
                            Addres = "BAKI Ş.,NƏRİMANOV R.,Ə.ƏLİYEV K.,EV.148,M.907",
                            Email = "turancha@code.edu.az",
                            TelNumber = "0503504290"
                        },
                    };

                await Companies.AddRangeAsync(companies);

                await SaveChangesAsync();
            }

            var profit = await CompanyProfit.AnyAsync();
            if (!profit)
            {
                List<CompanyProfit> companyProfits = new List<CompanyProfit>
                {
                    new CompanyProfit
                    {
                        CompanyId = 14,
                        Profit = 123567,
                        Date = new DateTime(2019, 01, 31)
                    },
                    new CompanyProfit
                    {
                        CompanyId = 15,
                        Profit = 132198,
                        Date = new DateTime(2019, 01, 31)
                    },
                    new CompanyProfit
                    {
                        CompanyId = 16,
                        Profit = 289567,
                        Date = new DateTime(2019, 01, 31)
                    },
                    new CompanyProfit
                    {
                        CompanyId = 14,
                        Profit = 219804,
                        Date = new DateTime(2019, 02, 28)
                    },
                    new CompanyProfit
                    {
                        CompanyId = 15,
                        Profit = 198435,
                        Date = new DateTime(2019, 02, 28)
                    },
                    new CompanyProfit
                    {
                        CompanyId = 16,
                        Profit = 201921,
                        Date = new DateTime(2019, 02, 28)
                    },
                    new CompanyProfit
                    {
                        CompanyId = 14,
                        Profit = 220745,
                        Date = new DateTime(2019, 03, 31)
                    },
                    new CompanyProfit
                    {
                        CompanyId = 15,
                        Profit = 2108435,
                        Date = new DateTime(2019, 03, 31)
                    },
                    new CompanyProfit
                    {
                        CompanyId = 16,
                        Profit = 232312,
                        Date = new DateTime(2019, 03, 31)
                    }
                };

                await CompanyProfit.AddRangeAsync(companyProfits);
                await SaveChangesAsync();
            }

            var departmant = await Departmants.AnyAsync();
            if (!departmant)
            {
                List<Departmant> departmants = new List<Departmant>
                    {
                        new Departmant
                        {
                            Name = "Bütün departamentlər",
                        },
                        new Departmant
                        {
                            Name = "Satiş departamenti",
                        },
                        new Departmant
                        {
                            Name = "İt departamenti",
                        },
                        new Departmant
                        {
                            Name = "Biznes inkişaf departamenti",
                        },
                        new Departmant
                        {
                            Name = "Marketinq departamenti",
                        },
                        new Departmant
                        {
                            Name = "Administration departamenti",
                        },
                        new Departmant
                        {
                            Name = "Shipping departamenti",
                        },
                        new Departmant
                        {
                            Name = "Purchasing  departamenti",
                        },
                        new Departmant
                        {
                            Name = "Human Resources  departamenti",
                        },
                        new Departmant
                        {
                            Name = "Public Relations departamenti",
                        },
                        new Departmant
                        {
                            Name = "Sales departamenti",
                        },
                        new Departmant
                        {
                            Name = "Executive departamenti",
                        },
                        new Departmant
                        {
                            Name = "Finance departamenti",
                        },
                        new Departmant
                        {
                            Name = "Treasury departamenti",
                        },
                        new Departmant
                        {
                            Name = "Corporate Tax departamenti",
                        },
                        new Departmant
                        {
                            Name = "Control And Credit departamenti",
                        },
                        new Departmant
                        {
                            Name = "Shareholder Services departamenti",
                        },
                        new Departmant
                        {
                            Name = "Benefits departamenti",
                        },
                        new Departmant
                        {
                            Name = "Manufacturing departamenti",
                        },
                        new Departmant
                        {
                            Name = "Construction departamenti",
                        },
                        new Departmant
                        {
                            Name = "Contracting departamenti",
                        },
                        new Departmant
                        {
                            Name = "Operations departamenti",
                        },
                        new Departmant
                        {
                            Name = "IT Support departamenti",
                        },
                        new Departmant
                        {
                            Name ="NOC departamenti",
                        },
                        new Departmant
                        {
                            Name = "IT Helpdesk departamenti",
                        },
                        new Departmant
                        {
                            Name = "Government Sales departamenti",
                        },
                        new Departmant
                        {
                            Name = "Retail Sales departamenti",
                        },
                        new Departmant
                        {
                            Name = "Recruiting departamenti",
                        },
                        new Departmant
                        {
                            Name = "Payroll departamenti",
                        },
                        new Departmant
                        {
                            Name = "African American Studies Program departamenti",
                        },

                };

                await Departmants.AddRangeAsync(departmants);

                await SaveChangesAsync();
            }

            var companyDepartament = await CompanyDepartaments.AnyAsync();
            if (!companyDepartament)
            {
                List<CompanyDepartament> companyDepartaments = new List<CompanyDepartament>
                    {
                        new CompanyDepartament
                        {
                            CompanyId = 14,
                            DepartmantId = 25
                        },
                        new CompanyDepartament
                        {
                            CompanyId = 14,
                            DepartmantId = 26
                        },
                        new CompanyDepartament
                        {
                            CompanyId = 14,
                            DepartmantId = 27
                        },
                        new CompanyDepartament
                        {
                            CompanyId = 14,
                            DepartmantId = 28
                        },
                         new CompanyDepartament
                        {
                            CompanyId = 15,
                            DepartmantId = 25
                        },
                        new CompanyDepartament
                        {
                            CompanyId = 15,
                            DepartmantId = 26
                        },
                        new CompanyDepartament
                        {
                            CompanyId = 15,
                            DepartmantId = 27
                        },
                        new CompanyDepartament
                        {
                            CompanyId = 15,
                            DepartmantId = 28
                        },
                             new CompanyDepartament
                        {
                            CompanyId = 16,
                            DepartmantId = 25
                        },
                        new CompanyDepartament
                        {
                            CompanyId = 16,
                            DepartmantId = 26
                        },
                        new CompanyDepartament
                        {
                            CompanyId = 16,
                            DepartmantId = 27
                        },
                        new CompanyDepartament
                        {
                            CompanyId = 16,
                            DepartmantId = 28
                        }
                    };

                await CompanyDepartaments.AddRangeAsync(companyDepartaments);

                await SaveChangesAsync();
            }

            var shop = await Shops.AnyAsync();
            if (!shop)
            {
                List<Shop> shops = new List<Shop>
                    {
                        new Shop
                        {
                            Name = "Celio",
                            CompanyId = 16
                        },
                        new Shop
                        {
                            Name = "New Yorker",
                            CompanyId = 16
                        },
                        new Shop
                        {
                            Name = "Mango",
                            CompanyId = 16
                        },
                        new Shop
                        {
                            Name = "Emporium",
                            CompanyId = 16
                        },
                        new Shop
                        {
                            Name = "Zara",
                            CompanyId = 15
                        },
                        new Shop
                        {
                            Name = "Next",
                            CompanyId = 16
                        },
                        new Shop
                        {
                            Name = "Ideal",
                            CompanyId = 15
                        },
                        new Shop
                        {
                            Name = "Sabina",
                            CompanyId = 15
                        },
                        new Shop
                        {
                            Name = "Boutır",
                            CompanyId = 14
                        },
                        new Shop
                        {
                            Name = "Roselle",
                            CompanyId = 14
                        },
                        new Shop
                        {
                            Name = "Bambi",
                            CompanyId = 14
                        },
                        new Shop
                        {
                            Name = "Belvest",
                            CompanyId = 14
                        },
                        new Shop
                        {
                            Name = "Calvin Klein Jeans",
                            CompanyId = 15
                        },
                        new Shop
                        {
                            Name = "Corneliani",
                            CompanyId = 14
                        },
                        new Shop
                        {
                            Name = "Jaguar",
                            CompanyId = 15
                        },
                        new Shop
                        {
                            Name = "Liberi",
                            CompanyId = 14
                        },
                        new Shop
                        {
                            Name = "Poppy",
                            CompanyId = 15
                        },
                        new Shop
                        {
                            Name = "Salamander",
                            CompanyId = 5
                        },
                        new Shop
                        {
                            Name = "Salle De Mode",
                            CompanyId = 6
                        },
                        new Shop
                        {
                            Name = "Scabal",
                            CompanyId = 7
                        },
                        new Shop
                        {
                            Name = "Torras",
                            CompanyId = 8
                        },
                        new Shop
                        {
                            Name = "Waggon",
                            CompanyId = 1
                        }
                    };

                await Shops.AddRangeAsync(shops);

                await SaveChangesAsync();

            }

            var shopProfit = await ShopProfits.AnyAsync();
            if (!shopProfit)
            {
                List<ShopProfit> shopProfits = new List<ShopProfit>
                {
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019, 10, 03),
                         Profit = 5000
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019, 10, 01),
                         Profit = 6000
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019, 10, 02),
                         Profit = 6010
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019, 10, 04),
                         Profit = 5010
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019, 10, 05),
                         Profit = 5810
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019, 10, 06),
                         Profit = 5610
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019, 10, 07),
                         Profit = 5550
                    },
                    new ShopProfit
                    {
                         ShopId = 16,
                         Date = new DateTime(2019, 10, 07),
                         Profit = 6234
                    },
                    new ShopProfit
                    {
                         ShopId = 18,
                         Date = new DateTime(2019, 10, 07),
                         Profit = 5900
                    },
                    new ShopProfit
                    {
                         ShopId = 19,
                         Date =  new DateTime(2019, 10, 08),
                         Profit = 6213
                    },
                    new ShopProfit
                    {
                         ShopId = 20,
                         Date = new DateTime(2019,10,09),
                         Profit = 3090
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,11),
                         Profit = 4090
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,08),
                         Profit = 6090
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,09),
                         Profit = 6990
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,10),
                         Profit = 7990
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,11),
                         Profit = 7990
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,12),
                         Profit = 5090
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,13),
                         Profit = 3090
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,14),
                         Profit = 5090
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,15),
                         Profit = 5090
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,16),
                         Profit = 5090
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,17),
                         Profit = 5090
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,18),
                         Profit = 5090
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,19),
                         Profit = 676090
                    },
                    new ShopProfit
                    {
                         ShopId = 1,
                         Date = new DateTime(2019,10,20),
                         Profit = 75590
                    }
                };

                await ShopProfits.AddRangeAsync(shopProfits);
                await SaveChangesAsync();
            }

            var position = await Positions.AnyAsync();
            if (!position)
            {
                List<Position> positions = new List<Position>
                    {
                        new Position
                        {
                            Name = "Satiş təmsilçisi",
                            DepartmantId = 28
                        },
                        new Position
                        {
                            Name = "Satiş təmsilçisinin köməkçisi",
                            DepartmantId = 28
                        },
                        new Position
                        {
                            Name = "Baş satıcı",
                            DepartmantId = 28
                        },
                        new Position
                        {
                            Name = "Mağaza meneceri",
                            DepartmantId = 25
                        },
                        new Position
                        {
                            Name = "Brend meneceri",
                            DepartmantId = 25
                        },
                        new Position
                        {
                            Name = "Programçı",
                            DepartmantId = 27
                        },
                        new Position
                        {
                            Name = "Baş programçı",
                            DepartmantId = 27
                        },
                        new Position
                        {
                            Name = "Aparıcı programçı",
                            DepartmantId = 27
                        },
                        new Position
                        {
                            Name = "İT direktor",
                            DepartmantId = 27
                        },
                        new Position
                        {
                            Name = "System administratoru",
                            DepartmantId = 27
                        },
                        new Position
                        {
                            Name = "Satış departament müdiri",
                            DepartmantId = 28
                        },
                        new Position
                        {
                            Name = "İt departament müdiri",
                            DepartmantId = 27
                        },
                        new Position
                        {
                            Name = "Marketing departament müdiri",
                            DepartmantId = 25
                        }

                    };

                await Positions.AddRangeAsync(positions);
                await SaveChangesAsync();
            }

            var recruitment = await Recruitments.AnyAsync();
            if (!recruitment)
            {
                List<Recruitment> recruitments = new List<Recruitment>
                    {
                         new Recruitment
                         {
                             EmployeeId =1,
                             PositionId =1,
                             ShopId =1,
                             WhenStarted = new DateTime(2019,1,01),
                             Amount = 600
                         },
                         new Recruitment
                         {
                             EmployeeId =20,
                             PositionId =2,
                             ShopId =1,
                             WhenStarted = new DateTime(2018,11,01),
                             Amount = 500
                         },
                         new Recruitment
                         {
                             EmployeeId =18,
                             PositionId =3,
                             ShopId =1,
                             WhenStarted = new DateTime(2017,11,01),
                             Amount = 700
                         },
                         new Recruitment
                         {
                             EmployeeId =17,
                             PositionId =4,
                             ShopId =1,
                             WhenStarted = new DateTime(2016,09,01),
                             Amount = 750
                         },
                         new Recruitment
                         {
                             EmployeeId =16,
                             PositionId =5,
                             ShopId =1,
                             WhenStarted = new DateTime(2017,11,01),
                             Amount = 800
                         },
                         new Recruitment
                         {
                             EmployeeId =15,
                             PositionId =6,
                             ShopId =1,
                             WhenStarted = new DateTime(2016,09,01),
                             Amount = 1000
                         },
                         new Recruitment
                         {
                             EmployeeId =14,
                             PositionId =7,
                             ShopId =1,
                             WhenStarted = new DateTime(2017,11,01),
                             Amount = 1500
                         },
                         new Recruitment
                         {
                             EmployeeId =13,
                             PositionId =8,
                             ShopId =1,
                             WhenStarted = new DateTime(2017,09,01),
                             Amount = 1200
                         },
                         new Recruitment
                         {
                             EmployeeId =12,
                             PositionId =9,
                             ShopId =1,
                             WhenStarted = new DateTime(2016,09,01),
                             Amount = 2000
                         },
                         new Recruitment
                         {
                             EmployeeId =21,
                             PositionId =10,
                             ShopId =1,
                             WhenStarted = new DateTime(2018,09,01),
                             Amount = 2200
                         },
                         new Recruitment
                         {
                             EmployeeId =11,
                             PositionId =1,
                             ShopId =18,
                             WhenStarted = new DateTime(2017,09,01),
                             Amount = 600
                         },
                         new Recruitment
                         {
                             EmployeeId =9,
                             PositionId =2,
                             ShopId =18,
                             WhenStarted = new DateTime(2016,06,01),
                             Amount = 500
                         },
                         new Recruitment
                         {
                             EmployeeId =8,
                             PositionId =3,
                             ShopId =18,
                             WhenStarted = new DateTime(2016,01,01),
                             Amount = 700
                         },
                         new Recruitment
                         {
                             EmployeeId =7,
                             PositionId =4,
                             ShopId =18,
                             WhenStarted = new DateTime(2016,04,01),
                             Amount = 750
                         },
                         new Recruitment
                         {
                             EmployeeId =6,
                             PositionId =5,
                             ShopId =18,
                             WhenStarted = new DateTime(2016,07,01),
                             Amount = 800
                         },
                         new Recruitment
                         {
                             EmployeeId =5,
                             PositionId =6,
                             ShopId =18,
                             WhenStarted = new DateTime(2016,09,21),
                             Amount = 1000
                         },
                         new Recruitment
                         {
                             EmployeeId =4,
                             PositionId =7,
                             ShopId =18,
                             WhenStarted = new DateTime(2016,02,01),
                             Amount = 1500
                         },
                         new Recruitment
                         {
                             EmployeeId =3,
                             PositionId =8,
                             ShopId =18,
                             WhenStarted = new DateTime(2016,03,01),
                             Amount = 1200
                         },
                         new Recruitment
                         {
                             EmployeeId =2,
                             PositionId =9,
                             ShopId =18,
                             WhenStarted = new DateTime(2016,06,01),
                             Amount = 2000
                         },
                         new Recruitment
                         {
                             EmployeeId =10,
                             PositionId =10,
                             ShopId =18,
                             WhenStarted = new DateTime(2016,10,01),
                             Amount = 2200
                         },
                         new Recruitment
                         {
                             EmployeeId =22,
                             PositionId =1,
                             ShopId =19,
                             WhenStarted = new DateTime(2016,12,01),
                             Amount = 600
                         },
                         new Recruitment
                         {
                             EmployeeId =19,
                             PositionId =2,
                             ShopId =19,
                             WhenStarted = new DateTime(2016,06,01),
                             Amount = 500
                         },
                         new Recruitment
                         {
                             EmployeeId =23,
                             PositionId =3,
                             ShopId =19,
                             WhenStarted = new DateTime(2016,01,01),
                             Amount = 700
                         },
                         new Recruitment
                         {
                             EmployeeId =24,
                             PositionId =4,
                             ShopId =19,
                             WhenStarted = new DateTime(2016,04,01),
                             Amount = 750
                         },
                         new Recruitment
                         {
                             EmployeeId =25,
                             PositionId =5,
                             ShopId =19,
                             WhenStarted = new DateTime(2016,07,01),
                             Amount = 800
                         },
                         new Recruitment
                         {
                             EmployeeId =26,
                             PositionId =6,
                             ShopId =19,
                             WhenStarted = new DateTime(2016,09,21),
                             Amount = 1000
                         },
                         new Recruitment
                         {
                             EmployeeId =27,
                             PositionId =7,
                             ShopId =19,
                             WhenStarted = new DateTime(2016,02,01),
                             Amount = 1500
                         },
                         new Recruitment
                         {
                             EmployeeId =28,
                             PositionId =8,
                             ShopId =19,
                             WhenStarted = new DateTime(2016,03,01),
                             Amount = 1200
                         },
                         new Recruitment
                         {
                             EmployeeId =29,
                             PositionId =9,
                             ShopId =19,
                             WhenStarted = new DateTime(2016,06,01),
                             Amount = 2000
                         },
                         new Recruitment
                         {
                             EmployeeId =30,
                             PositionId =10,
                             ShopId =19,
                             WhenStarted = new DateTime(2016,10,01),
                             Amount = 2200
                         },
                         new Recruitment
                         {
                             EmployeeId =31,
                             PositionId =1,
                             ShopId =20,
                             WhenStarted = new DateTime(2016,12,01),
                             Amount = 600
                         },
                         new Recruitment
                         {
                             EmployeeId =32,
                             PositionId =2,
                             ShopId =20,
                             WhenStarted = new DateTime(2016,06,01),
                             Amount = 500
                         },
                         new Recruitment
                         {
                             EmployeeId =33,
                             PositionId =3,
                             ShopId =20,
                             WhenStarted = new DateTime(2016,01,01),
                             Amount = 700
                         },
                         new Recruitment
                         {
                             EmployeeId =34,
                             PositionId =4,
                             ShopId =20,
                             WhenStarted = new DateTime(2016,04,01),
                             Amount = 750
                         },
                         new Recruitment
                         {
                             EmployeeId =35,
                             PositionId =5,
                             ShopId =20,
                             WhenStarted = new DateTime(2016,07,01),
                             Amount = 800
                         },
                         new Recruitment
                         {
                             EmployeeId =36,
                             PositionId =6,
                             ShopId =20,
                             WhenStarted = new DateTime(2016,09,21),
                             Amount = 1000
                         },
                         new Recruitment
                         {
                             EmployeeId =41,
                             PositionId =7,
                             ShopId =20,
                             WhenStarted = new DateTime(2016,02,01),
                             Amount = 1500
                         },
                         new Recruitment
                         {
                             EmployeeId =38,
                             PositionId =8,
                             ShopId =20,
                             WhenStarted = new DateTime(2016,03,01),
                             Amount = 1200
                         },
                         new Recruitment
                         {
                             EmployeeId =39,
                             PositionId =9,
                             ShopId =20,
                             WhenStarted = new DateTime(2016,06,01),
                             Amount = 2000
                         },
                         new Recruitment
                         {
                             EmployeeId =40,
                             PositionId =10,
                             ShopId =20,
                             WhenStarted = new DateTime(2016,10,01),
                             Amount = 2200
                         },
                         new Recruitment
                         {
                             EmployeeId =42,
                             PositionId =1,
                             ShopId =16,
                             WhenStarted = new DateTime(2016,12,01),
                             Amount = 600
                         },
                         new Recruitment
                         {
                             EmployeeId =43,
                             PositionId =2,
                             ShopId =16,
                             WhenStarted = new DateTime(2016,06,01),
                             Amount = 500
                         },
                         new Recruitment
                         {
                             EmployeeId =44,
                             PositionId =3,
                             ShopId =16,
                             WhenStarted = new DateTime(2016,01,01),
                             Amount = 700
                         },
                         new Recruitment
                         {
                             EmployeeId =45,
                             PositionId =4,
                             ShopId =16,
                             WhenStarted = new DateTime(2016,04,01),
                             Amount = 750
                         },
                         new Recruitment
                         {
                             EmployeeId =46,
                             PositionId =5,
                             ShopId =16,
                             WhenStarted = new DateTime(2016,07,01),
                             Amount = 800
                         },
                         new Recruitment
                         {
                             EmployeeId =47,
                             PositionId =6,
                             ShopId =16,
                             WhenStarted = new DateTime(2016,09,21),
                             Amount = 1000
                         },
                         new Recruitment
                         {
                             EmployeeId =48,
                             PositionId =7,
                             ShopId =16,
                             WhenStarted = new DateTime(2016,02,01),
                             Amount = 1500
                         },
                         new Recruitment
                         {
                             EmployeeId =49,
                             PositionId =8,
                             ShopId =16,
                             WhenStarted = new DateTime(2016,03,01),
                             Amount = 1200
                         },
                         new Recruitment
                         {
                             EmployeeId =50,
                             PositionId =9,
                             ShopId =16,
                             WhenStarted = new DateTime(2016,06,01),
                             Amount = 2000
                         },
                    };

                await Recruitments.AddRangeAsync(recruitments);

                await SaveChangesAsync();
            }

            var salary = await Salaries.AnyAsync();
            if (!salary)
            {
                List<Salary> salaries = new List<Salary>
                {
                    new Salary
                    {
                         CompanyId = 16,
                         PositionId = 1,
                         SalaryAmount = 600
                    },
                     new Salary
                     {
                         CompanyId = 16,
                         PositionId = 2,
                         SalaryAmount = 500
                     },
                     new Salary
                     {
                         CompanyId = 16,
                         PositionId = 3,
                         SalaryAmount = 700
                     },
                     new Salary
                     {
                         CompanyId = 16,
                         PositionId = 4,
                         SalaryAmount = 750
                     },
                     new Salary
                     {
                         CompanyId = 16,
                         PositionId = 5,
                         SalaryAmount = 800
                     },
                     new Salary
                     {
                         CompanyId = 16,
                         PositionId = 6,
                         SalaryAmount = 1000
                     },
                     new Salary
                     {
                         CompanyId = 16,
                         PositionId = 7,
                         SalaryAmount = 1500
                     },
                      new Salary
                      {
                         CompanyId = 16,
                         PositionId = 8,
                         SalaryAmount = 1200
                      },
                      new Salary
                      {
                         CompanyId = 16,
                         PositionId = 9,
                         SalaryAmount = 2000
                      },
                      new Salary
                      {
                         CompanyId = 16,
                         PositionId = 10,
                         SalaryAmount = 2200
                      },
                      new Salary
                      {
                         CompanyId = 16,
                         PositionId = 11,
                         SalaryAmount = 3000
                      },
                       new Salary
                      {
                         CompanyId = 16,
                         PositionId = 12,
                         SalaryAmount = 3000
                      },
                       new Salary
                      {
                         CompanyId = 16,
                         PositionId = 13,
                         SalaryAmount = 3000
                      }
                };

                await Salaries.AddRangeAsync(salaries);

                await SaveChangesAsync();
            }

            var continutiy = await Continuity.AnyAsync();
            if (!continutiy)
            {
                List<Continuity> continuities = new List<Continuity>
                {
                    new Continuity
                    {
                         RecruitmentId = 1,
                         Date = DateTime.Now,
                         PermissionType = Enum.Permission.Üzürlü,
                         Reason = "Xeste olub"
                    },
                    new Continuity
                    {
                         RecruitmentId = 2,
                         Date = DateTime.Now,
                         PermissionType = Enum.Permission.Üzürlü,
                         Reason = "Xeste olub"
                    },
                    new Continuity
                    {
                         RecruitmentId = 3,
                         Date = DateTime.Now,
                         PermissionType = Enum.Permission.Üzürlü,
                         Reason = "Xeste olub"
                    },
                    new Continuity
                    {
                         RecruitmentId = 4,
                         Date = DateTime.Now,
                         PermissionType = Enum.Permission.Üzürlü,
                         Reason = "Xeste olub"
                    },
                    new Continuity
                    {
                         RecruitmentId = 6,
                         Date = new DateTime(2019,10,10),
                         PermissionType = Enum.Permission.Üzürlü,
                         Reason = "Xeste olub"
                    },
                    new Continuity
                    {
                         RecruitmentId = 8,
                         Date = new DateTime(2019,10,3),
                         PermissionType = Enum.Permission.Üzürlü,
                         Reason = "Xeste olub"
                    },
                    new Continuity
                    {
                         RecruitmentId = 10,
                         Date = new DateTime(2019,10,7),
                         PermissionType = Enum.Permission.Üzürlü,
                         Reason = "Xeste olub"
                    },
                     new Continuity
                    {
                         RecruitmentId = 1,
                         Date = new DateTime(2019,01,01),
                         PermissionType = Enum.Permission.Üzürlü,
                         Reason = "Xeste olub"
                    },
                      new Continuity
                    {
                         RecruitmentId = 4,
                         Date = new DateTime(2019,01,25),
                         PermissionType = Enum.Permission.Üzürlü,
                         Reason = "Xeste olub"
                    }

                };

                await Continuity.AddRangeAsync(continuities);
                await SaveChangesAsync();

            }

            var bonus = await Bonus.AnyAsync();
            if (!bonus)
            {
                List<Bonus> bonus1 = new List<Bonus>
                {
                    new Bonus
                    {
                         RecruitmentId = 1,
                         Amount = 100,
                         Reason = "Çox çalışdığı üçün...",
                         Date = new DateTime(2019,01,31)
                    },
                    new Bonus
                    {
                         RecruitmentId = 2,
                         Amount = 50,
                         Reason = "İşdən sonra qalıb, işlədiyi üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 3,
                         Amount = 120,
                         Reason = "Daha çox müştəri yola saldığı üçün...",
                         Date = new DateTime(2019,02,28)
                    },
                    new Bonus
                    {
                         RecruitmentId = 4,
                         Amount = 100,
                         Reason = "Çox çalışdığı üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 5,
                         Amount = 50,
                         Reason = "İşdən sonra qalıb, işlədiyi üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 6,
                         Amount = 120,
                         Reason = "Daha çox müştəri yola saldığı üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 7,
                         Amount = 100,
                         Reason = "Çox çalışdığı üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 8,
                         Amount = 50,
                         Reason = "İşdən sonra qalıb, işlədiyi üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 9,
                         Amount = 120,
                         Reason = "Daha çox müştəri yola saldığı üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 10,
                         Amount = 100,
                         Reason = "Çox çalışdığı üçün...",
                         Date = new DateTime(2019,01,30)
                    },
                    new Bonus
                    {
                         RecruitmentId = 11,
                         Amount = 50,
                         Reason = "İşdən sonra qalıb, işlədiyi üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 12,
                         Amount = 120,
                         Reason = "Daha çox müştəri yola saldığı üçün...",
                         Date = DateTime.Now
                    },
                           new Bonus
                    {
                         RecruitmentId = 13,
                         Amount = 100,
                         Reason = "Çox çalışdığı üçün...",
                         Date = new DateTime(2019,01,31)
                    },
                    new Bonus
                    {
                         RecruitmentId = 14,
                         Amount = 50,
                         Reason = "İşdən sonra qalıb, işlədiyi üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 15,
                         Amount = 120,
                         Reason = "Daha çox müştəri yola saldığı üçün...",
                         Date = new DateTime(2019,02,28)
                    },
                    new Bonus
                    {
                         RecruitmentId = 16,
                         Amount = 100,
                         Reason = "Çox çalışdığı üçün...",
                         Date =DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 21,
                         Amount = 120,
                         Reason = "Daha çox müştəri yola saldığı üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 22,
                         Amount = 100,
                         Reason = "Çox çalışdığı üçün...",
                         Date = new DateTime(2019,02,28)
                    },
                         new Bonus
                    {
                         RecruitmentId = 25,
                         Amount = 120,
                         Reason = "Daha çox müştəri yola saldığı üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 28,
                         Amount = 120,
                         Reason = "Daha çox müştəri yola saldığı üçün...",
                         Date = DateTime.Now
                    },
                    new Bonus
                    {
                         RecruitmentId = 29,
                         Amount = 100,
                         Reason = "Çox çalışdığı üçün...",
                         Date = new DateTime(2019,01,30)
                    }

                };

                await Bonus.AddRangeAsync(bonus1);
                await SaveChangesAsync();
            }

            var payroll = await Payrolls.AnyAsync();
            if (!payroll)
            {
                List<Payroll> payrolls = new List<Payroll>
                {
                    new Payroll
                    {
                        RecruitmentId =1,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 600
                    },
                    new Payroll
                    {
                        RecruitmentId =2,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1500
                    },
                    new Payroll
                    {
                        RecruitmentId =3,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1200
                    },
                    new Payroll
                    {
                        RecruitmentId =4,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 2000
                    },
                    new Payroll
                    {
                        RecruitmentId =5,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 2200
                    },
                    new Payroll
                    {
                        RecruitmentId =6,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 600
                    },
                    new Payroll
                    {
                        RecruitmentId =7,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 500
                    },
                    new Payroll
                    {
                        RecruitmentId =8,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 700
                    },
                    new Payroll
                    {
                        RecruitmentId =9,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 750
                    },
                    new Payroll
                    {
                        RecruitmentId =10,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 800
                    },
                    new Payroll
                    {
                        RecruitmentId =11,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1000
                    },
                    new Payroll
                    {
                        RecruitmentId =12,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1500
                    },
                    new Payroll
                    {
                        RecruitmentId =13,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1200
                    },
                    new Payroll
                    {
                        RecruitmentId =14,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 2000
                    },
                    new Payroll
                    {
                        RecruitmentId =15,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 2200
                    },
                    new Payroll
                    {
                        RecruitmentId =16,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 600
                    },
                    new Payroll
                    {
                        RecruitmentId =17,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 500
                    },
                    new Payroll
                    {
                        RecruitmentId =18,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 700
                    },
                    new Payroll
                    {
                        RecruitmentId =19,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 750
                    },
                    new Payroll
                    {
                        RecruitmentId =20,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 800
                    },
                    new Payroll
                    {
                        RecruitmentId =21,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1000
                    },
                    new Payroll
                    {
                        RecruitmentId =22,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1500
                    },
                    new Payroll
                    {
                        RecruitmentId =23,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1000
                    },
                    new Payroll
                    {
                        RecruitmentId =24,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1200
                    },
                    new Payroll
                    {
                        RecruitmentId =25,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 800
                    },
                    new Payroll
                    {
                        RecruitmentId =26,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 700
                    },
                    new Payroll
                    {
                        RecruitmentId =27,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 500
                    },
                    new Payroll
                    {
                        RecruitmentId =28,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 700
                    },
                    new Payroll
                    {
                        RecruitmentId =29,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 750
                    },
                    new Payroll
                    {
                        RecruitmentId =30,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 800
                    },
                    new Payroll
                    {
                        RecruitmentId =31,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1000
                    },
                    new Payroll
                    {
                        RecruitmentId =32,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1500
                    },
                    new Payroll
                    {
                        RecruitmentId =33,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1200
                    },
                    new Payroll
                    {
                        RecruitmentId =34,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 2000
                    },
                    new Payroll
                    {
                        RecruitmentId =35,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 2200
                    },
                    new Payroll
                    {
                        RecruitmentId =36,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 600
                    },
                    new Payroll
                    {
                        RecruitmentId =38,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 900
                    },
                    new Payroll
                    {
                        RecruitmentId =37,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 500
                    },
                    new Payroll
                    {
                        RecruitmentId =39,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 750
                    },
                    new Payroll
                    {
                        RecruitmentId =40,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 800
                    },
                    new Payroll
                    {
                        RecruitmentId =41,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1000
                    },
                    new Payroll
                    {
                        RecruitmentId =42,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1500
                    },
                    new Payroll
                    {
                        RecruitmentId =43,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 1200
                    },
                    new Payroll
                    {
                        RecruitmentId =44,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 2000
                    },
                    new Payroll
                    {
                        RecruitmentId =45,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 2200
                    },
                    new Payroll
                    {
                        RecruitmentId =46,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 600
                    },
                    new Payroll
                    {
                        RecruitmentId =47,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 500
                    },
                    new Payroll
                    {
                        RecruitmentId =48,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 750
                    },
                    new Payroll
                    {
                        RecruitmentId =49,
                        Date = new DateTime(2019,01,31),
                        TotalSalary = 2000
                    }
                };

                await Payrolls.AddRangeAsync(payrolls);
                await SaveChangesAsync();
            }

            var formerwork = await FormerWorks.AnyAsync();
            if (!formerwork)
            {
                List<FormerWork> formerWorks = new List<FormerWork>
                {
                    new FormerWork
                    {
                        WorkName = "CaspianPay",
                        EmployeeId = 1,
                        WhyLeftReason = "Mühiti sevmədiyi üçün",
                        WhenStarted = new DateTime(2018, 01, 07),
                        WhenLeft = new DateTime(2018, 01, 29)
                    },
                    new FormerWork
                    {
                        EmployeeId =2,
                        WorkName ="Nobel Oil",
                        WhyLeftReason = "Maaşı qanə etmədiyi üçün",
                        WhenStarted = new DateTime(2018, 03, 01),
                        WhenLeft = new DateTime(2018,08,01 )
                    },
                    new FormerWork
                    {
                        EmployeeId =4,
                        WorkName = "Akold",
                        WhenStarted = new DateTime(2011,01,01),
                        WhenLeft = new DateTime(2014, 01, 01),
                        WhyLeftReason = "Lahiyə bitdiyi üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId = 5,
                        WorkName = "B.A.R.S. web solutions",                        WhenStarted = new DateTime(2018,01,01),                        WhyLeftReason ="Maaş qane etmədiyi üçün",                        WhenLeft = new DateTime(2018,06,30)                    },
                    new FormerWork
                    {
                        EmployeeId = 5,
                        WorkName = "Caspian Group",
                        WhenStarted = new DateTime(2018,07,01),
                        WhenLeft = new DateTime(2018,12,30),
                        WhyLeftReason ="Maaş qane etmədiyi üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId =5,
                        WorkName = "Code Academy",
                        WhyLeftReason = "Başqa is tapdığım üçün çıxdım.",
                        WhenStarted = new DateTime(2017, 04,01),
                        WhenLeft = new DateTime(2017, 12, 30)
                    },
                    new FormerWork
                    {
                        EmployeeId = 8,
                        WorkName = "Yeni iz tədris mərkəzi:Riyaziyyat müəllimi",
                        WhenStarted = new DateTime(2018,03,01),
                        WhenLeft = new DateTime(2018,12,30),
                        WhyLeftReason = "Daha yaxşı iş tapdığım üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId = 8 ,
                        WorkName = "Yup technology:C# trainer(muellimi)",
                        WhenStarted = new DateTime(2018,01,01),
                        WhenLeft = new DateTime(2018,12,31),
                        WhyLeftReason = "Maaş az olduğu üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId = 20,
                        WorkName = "Code Academy",
                        WhenStarted = new DateTime(2018,01,01),
                        WhenLeft = new DateTime(2018,10,01),
                        WhyLeftReason = "Başqa iş tapdığım üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId = 20,
                        WorkName = "Media Balans",
                        WhenStarted = new DateTime(2018,01,01),
                        WhenLeft = new DateTime(2018,08,01),
                        WhyLeftReason = "Başqa iş tapdığım üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId = 4,
                        WorkName = "Avesta Konserin",
                        WhenStarted = new DateTime(2014,01,01),
                        WhenLeft = new DateTime(2015,01,01),
                        WhyLeftReason = "Şirkət bakurot olduğu üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId = 4,
                        WorkName = "Təhsil Nazirliyi",
                        WhenStarted = new DateTime(2015,01,01),
                        WhenLeft = new DateTime(2018,01,01),
                        WhyLeftReason = "Maaş az olduğu üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId = 23,
                        WorkName = "Amrah Bank",
                        WhenStarted = new DateTime(2018,03,25),
                        WhenLeft = new DateTime(2018,06,25),
                        WhyLeftReason = "Təcrübə bitdiyi üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId =23 ,
                        WorkName = "Code Academy",
                        WhenStarted = new DateTime(2018,05,25),
                        WhenLeft = new DateTime(2018,09,25),
                        WhyLeftReason = "Başqa iş tapdığım üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId = 22,
                        WorkName = "Safaroff",
                        WhenStarted = new DateTime(2018,06,01),
                        WhenLeft = new DateTime(2018,09,01),
                        WhyLeftReason = "Təcrübə bitdiyi üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId = 10,
                        WorkName = "Baku City Circuit'te Könüllü ",
                        WhenStarted = new DateTime(2017,04,07),
                        WhenLeft = new DateTime(2017,07,07),
                        WhyLeftReason = "Təcrübə bitdiyi üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId = 10,
                        WorkName = "AREA Könüllü",
                        WhenStarted = new DateTime(2018,07,01),
                        WhenLeft = new DateTime(2018,10,01),
                        WhyLeftReason = "Təcrübə bitdiyi üçün"
                    },
                    new FormerWork
                    {
                        EmployeeId = 10,
                        WorkName = "Sağlam həyat Könüllü",
                        WhenStarted = new DateTime(2016,01,01),
                        WhenLeft = new DateTime(2016,03,01),
                        WhyLeftReason = "Təcrübə bitdiyi üçün"
                    }
                };

                await FormerWorks.AddRangeAsync(formerWorks);
                await SaveChangesAsync();
            }

            var penalty = await Penalties.AnyAsync();
            if (!penalty)
            {
                List<Penalty> penalties = new List<Penalty>
                {
                     new Penalty
                    {
                         RecruitmentId = 33,
                         Amount = 50,
                         Reason = "Ay ərzində işdən 10 gün tez çıxdığı üçün...",
                         Date = DateTime.Now
                    },
                    new Penalty
                    {
                         RecruitmentId = 36,
                         Amount = 50,
                         Reason = "İşdən sonra qalıb, işləmədiyi üçün...",
                         Date = DateTime.Now
                    },
                    new Penalty
                    {
                         RecruitmentId = 41,
                         Amount = 120,
                         Reason = "Müştəri ilə düzgün davranmadığı üçün...",
                         Date = DateTime.Now
                    },
                    new Penalty
                    {
                         RecruitmentId = 38,
                         Amount = 100,
                         Reason = "Çox böyük məsuliyyətsizlik etdiyi üçün...",
                         Date =DateTime.Now
                    },
                    new Penalty
                    {
                         RecruitmentId = 1,
                         Amount = 20,
                         Reason = "İşə gecikdiyi üçün",
                         Date =DateTime.Now
                    },
                    new Penalty
                    {
                         RecruitmentId = 2,
                         Amount = 30,
                         Reason = "İcazəsiz iki gün qaldığı üçün",
                         Date = DateTime.Now
                    },
                    new Penalty
                    {
                         RecruitmentId = 3,
                         Amount = 35,
                         Reason = "Bir ayda işə 4 dəfə gecikdiyi üçün",
                         Date = DateTime.Now
                    },
                    new Penalty
                    {
                         RecruitmentId = 4,
                         Amount = 50,
                         Reason = "Taskı vaxtında çatdıra bilmədiyi üçün",
                         Date = DateTime.Now

                    },
                    new Penalty
                    {
                         RecruitmentId = 5,
                         Amount = 40,
                         Reason = "Məsuliyyətsizlik etdiyi üçün",
                         Date = DateTime.Now
                    }
                };

                await Penalties.AddRangeAsync(penalties);
                await SaveChangesAsync();
            }

            var shopBonus = await ShopBonus.AnyAsync();
            if (!shopBonus)
            {
                List<ShopBonus> datas = new List<ShopBonus>
                {
                     new ShopBonus
                     {
                        ShopId = 20,
                        PromotionAmount = 100,
                        MinAmount = 150000,
                        MaxAmount = 250000,
                        WhenLeft = new DateTime(2019, 10, 01),
                        WhenStarted = new DateTime(2019, 10, 31)
                     },
                     new ShopBonus
                     {
                        ShopId = 20,
                        PromotionAmount = 150,
                        MinAmount = 150000,
                        MaxAmount = 230000,
                        WhenLeft = new DateTime(2019, 09, 01),
                        WhenStarted = new DateTime(2019, 09, 30)
                     },
                     new ShopBonus
                     {
                        ShopId = 1,
                        PromotionAmount = 120,
                        MinAmount = 150000,
                        MaxAmount = 250000,
                        WhenLeft = new DateTime(2019, 10, 01),
                        WhenStarted = new DateTime(2019, 10, 31)
                     }
                };

                await ShopBonus.AddRangeAsync(datas);
                await SaveChangesAsync();

            }

            var vacation = await Vacations.AnyAsync();
            if (!vacation)
            {
                List<Vacation> vacations = new List<Vacation>
                {
                    new Vacation
                    {
                          RecruitmentId = 1,
                          WhenStarted = new DateTime(2019,06,01),
                          WhenLeft = new DateTime(2019, 06, 15)
                    },
                    new Vacation
                    {
                          RecruitmentId = 2,
                          WhenStarted = new DateTime(2019,07,01),
                          WhenLeft = new DateTime(2019, 07, 15)
                    },
                    new Vacation
                    {
                          RecruitmentId = 3,
                          WhenStarted = new DateTime(2019,08,01),
                          WhenLeft = new DateTime(2019, 08, 15)
                    },
                    new Vacation
                    {
                          RecruitmentId = 4,
                          WhenStarted = new DateTime(2019,09,01),
                          WhenLeft = new DateTime(2019, 09, 15)
                    },
                    new Vacation
                    {
                          RecruitmentId = 5,
                          WhenStarted = new DateTime(2019,10,01),
                          WhenLeft = new DateTime(2019, 10, 15)
                    },
                    new Vacation
                    {
                          RecruitmentId = 6,
                          WhenStarted = new DateTime(2019,09,15),
                          WhenLeft = new DateTime(2019, 09, 25)
                    },
                    new Vacation
                    {
                          RecruitmentId = 7,
                          WhenStarted = new DateTime(2019,08,15),
                          WhenLeft = new DateTime(2019, 08, 26)
                    }
                };

                await Vacations.AddRangeAsync(vacations);
                await SaveChangesAsync();
            }


        }
    }
}