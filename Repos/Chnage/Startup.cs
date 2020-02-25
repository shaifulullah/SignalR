using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Chnage.Repository;
using Chnage.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Chnage
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

            services.AddDbContext<MyECODBContext>(item => item.UseSqlServer(Configuration.GetConnectionString("myconn")));
            services.AddTransient<MyECODBContext>();
            //My ECO Manager
            services.AddTransient<IProduct, ProductRepository>();
            services.AddTransient<IRequestType, RequestTypeRepository>();
            services.AddTransient<IUser, UserRepository>();
            services.AddTransient<IUserRole, UserRoleRepository>();
            services.AddTransient<IECR, ECRRepository>();
            services.AddTransient<IECO, ECORepository>();
            services.AddTransient<IECN, ECNRepository>();
            services.AddTransient<IMiddleTables, MiddleTableRepository>();
            services.AddTransient<IAuditLog, AuditLogRepository>();
            services.AddTransient<INotificationSender, NotificationSenderRepository>();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
                //make the cookie essential
                options.Cookie.IsEssential = true;
            });


            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

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
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSession();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseMvc(routes =>
                    {
                        routes.MapRoute(
                             name: "default",
                             template: "{controller=Home}/{action=Main}");
                        routes.MapRoute(
                            name: "signin",
                            template: "{controller=Authentication}/{action=SignIn}");
                        routes.MapRoute(
                            name: "details",
                            template: "{controller=ECRs}/{action=Main}/{id?}");
                    });
            app.UseCookiePolicy();
        }
    }
    //public class Startup
    //{
    //    public Startup(IConfiguration configuration)
    //    {
    //        Configuration = configuration;
    //    }
    //    public IConfiguration Configuration { get; }
    //    // This method gets called by the runtime. Use this method to add services to the container.
    //    public void ConfigureServices(IServiceCollection services)
    //    {
    //        //string dbConnectionStringMyECO = Configuration.GetSection("Data")
    //        //.GetSection("MyECO")["ConnectionString"];
    //        services.Configure<CookiePolicyOptions>(options =>
    //        {
    //            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    //            options.CheckConsentNeeded = context => true;
    //            options.MinimumSameSitePolicy = SameSiteMode.None;
    //        });
    //        services.AddDbContext<MyECODBContext>(options =>
    //        {
    //            //MyECODB
    //            options.UseSqlServer(Configuration.GetConnectionString("ConnectionString"));
    //            //options.UseSqlServer(dbConnectionStringMyECO);
    //        });
    //        services.AddTransient<MyECODBContext>();
    //        services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
    //        //My ECO Manager
    //        services.AddTransient<IProduct, ProductRepository>();
    //        services.AddTransient<IRequestType, RequestTypeRepository>();
    //        services.AddTransient<IUser, UserRepository>();
    //        services.AddTransient<IUserRole, UserRoleRepository>();
    //        services.AddTransient<IECR, ECRRepository>();
    //        services.AddTransient<IECO, ECORepository>();
    //        services.AddTransient<IECN, ECNRepository>();
    //        services.AddTransient<IMiddleTables, MiddleTableRepository>();
    //        services.AddTransient<IAuditLog, AuditLogRepository>();
    //        services.AddTransient<INotificationSender, NotificationSenderRepository>();
    //    }

    //    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    //    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    //    {
    //        if (env.IsDevelopment())
    //        {
    //            app.UseDeveloperExceptionPage();
    //        }
    //        else
    //        {
    //            app.UseExceptionHandler("/Home/Error");
    //        }

    //        app.UseStaticFiles();
    //        app.UseCookiePolicy();

    //        app.UseMvc(routes =>
    //        {
    //            routes.MapRoute(
    //                 name: "default",
    //                 template: "{controller=Home}/{action=Main}");
    //            routes.MapRoute(
    //                name: "signin",
    //                template: "{controller=Authentication}/{action=SignIn}");
    //            routes.MapRoute(
    //                name: "details",
    //                template: "{controller=ECRs}/{action=Main}/{id?}");
    //        });
    //    }
    //}
}
