using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FinalProject.Areas.PayrollAdmin.DAL;
using FinalProject.Areas.PayrollAdmin.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FinalProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddAntiforgery(options => options.HeaderName = "X-XSRF-Token");


            services.AddDbContext<PayrollDbContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]);
            });

            services.AddIdentity<AppUser, AppRole>()
                    .AddEntityFrameworkStores<PayrollDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication();

            services.AddSession();

            services.Configure<IdentityOptions>(identityoptions =>
            {
                identityoptions.Password.RequireDigit = true;
                identityoptions.Password.RequireLowercase = true;
                identityoptions.Password.RequireUppercase = true;
                identityoptions.Password.RequiredLength = 8;
                identityoptions.Password.RequireNonAlphanumeric = true;

                identityoptions.User.RequireUniqueEmail = true;

                identityoptions.Lockout.MaxFailedAccessAttempts = 3;
                identityoptions.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                identityoptions.Lockout.AllowedForNewUsers = false;
            });

            services.AddMvc();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/payrolladmin/account/login";
                options.LogoutPath = "/payrolladmin/account/logout";
                options.AccessDeniedPath = "/payrolladmin/account/accessdenied";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseSession();
            app.UseAuthentication();

            app.UseMvc(route =>
            {
                route.MapRoute(
                    name: "area",
                    template: "{area:exists}/{controller}/{action}/{id?}");

                route.MapAreaRoute(
                    name: "default",
                    areaName: "PayrollAdmin",
                    template: "{controller=Company}/{action=List}/{id?}");
            });
        }
    }
}
