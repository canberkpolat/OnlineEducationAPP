using System;
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
using Microsoft.Extensions.FileProviders;
using System.IO;

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
            services.AddTransient<IUserRepository, EfUserRepository>();
            services.AddTransient<IUnitOfWork, EfUnitOfWork>();



            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                //Password Settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
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


            services.AddAuthentication()
                .AddGoogle(option =>
                {
                    option.ClientId = "259830213470-k8vvqq16tjjugutvpalpjb3dr8k01udg.apps.googleusercontent.com";
                    option.ClientSecret = "nzwCmQevQd-ixi0I7b28fNEK";
                })
                .AddFacebook(option =>
                {
                    option.AppId = "2953476641407757";
                    option.AppSecret = "ea698f157ad33ac614870dc2a7cc6378";
                });


            //Cookie Settings
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Expiration = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.LoginPath = "/Account/Login";
                options.LogoutPath = "/Account/Logout";
                options.SlidingExpiration = true;
            });


            services.AddMvc();
            services.AddSignalR();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors("_myAllowSpecificOrigins");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseStatusCodePagesWithRedirects("/Error/{0}");
            }
            
            //wwwroot
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseSignalR(routes =>
            {
                routes.MapHub<ChatHub>("/chatHub");
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


    }
}
