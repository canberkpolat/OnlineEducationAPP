﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineEducationAPP.MvcWebUI.Identity;
using OnlineEducationAPP.MvcWebUI.Repository.Abstract;
using OnlineEducationAPP.MvcWebUI.Repository.Concrete.EntityFrameworkCore;
using OnlineEducationAPP.MvcWebUI.Hubs;

namespace OnlineEducationAPP.MvcWebUI
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940

        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("_myAllowSpecificOrigins",
                builder =>
                {
                    builder
                        .WithOrigins("https://onlineeducationapp.canberkpolat.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddDbContext<OnlineEducationDbContext>(
                options => options.UseLazyLoadingProxies().UseSqlServer(Configuration.GetConnectionString("OnlineEducationAppConnection")));
            services.AddTransient<ICourseRepository, EfCourseRepository>();
            services.AddTransient<ICategoryRepository, EfCategoryRepository>();
            services.AddTransient<IStreamRepository, EfStreamRepository>();
            services.AddTransient<IUnitOfWork, EfUnitOfWork>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();





            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //Password Settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 10;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;

                //Lockout Settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
                options.Lockout.MaxFailedAccessAttempts = 5;

                //Signin Settings
                options.SignIn.RequireConfirmedEmail = false;

                //User Settings
                options.User.RequireUniqueEmail = true;

                //Token Settings
                options.Tokens.AuthenticatorTokenProvider = "Name of AuthenticatorTokenProvider";

            })  
                .AddEntityFrameworkStores<OnlineEducationDbContext>()
                .AddDefaultTokenProviders();


            //Cookie Settings
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Expiration = TimeSpan.FromDays   (1);
                options.Cookie.HttpOnly = true;
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.SlidingExpiration = true;
            });


            services.AddMvc();
            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, RoleManager<IdentityRole> roleManager)
        {
            app.UseCors("_myAllowSpecificOrigins");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub/24");
                routes.MapHub<ChatHub>("/chatHub/22");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            //InitializeHelper.Initial(roleManager).GetAwaiter().GetResult();  // Migration yapmaya engel oluyor,
        }

    }
}
