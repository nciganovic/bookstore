using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using BookStore.Models;
using Microsoft.EntityFrameworkCore;
using BookStore.Models.InterfaceRepo;
using BookStore.Models.SqlRepository;
using Microsoft.AspNetCore.Identity;
using BookStore.Models.Tables;
using BookStore.Security;
using Microsoft.AspNetCore.Authorization;

namespace BookStore
{
    public class Startup
    {
        private IConfiguration conf;

        public Startup(IConfiguration conf) {
            this.conf = conf;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(
                options => options.UseSqlServer(conf.GetConnectionString("BookStoreDbConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc(options => options.EnableEndpointRouting = false);

            services.AddAuthentication()
            .AddGoogle(options => {
                options.ClientId = Environment.GetEnvironmentVariable("bookstore-GoogleClientId");
                options.ClientSecret = Environment.GetEnvironmentVariable("bookstore-GoogleClientSecret");
            })
            .AddFacebook(options => {
                options.AppSecret = Environment.GetEnvironmentVariable("bookstore-FacebookAppSecret"); 
                options.AppId = Environment.GetEnvironmentVariable("bookstore-FacebookAppId"); 


            });

            services.AddScoped<IPersonRepository, SqlPersonRepository>();
            services.AddScoped<IAuthorRepository, SqlAuthorRepository>();
            services.AddScoped<ICategoryRepository, SqlCategoryRepository>();
            services.AddScoped<IBookRepository, SqlBookRepository>();
            services.AddScoped<IAuthorBookRepository, SqlAuthorBookRepository>();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", policy => policy.RequireClaim("Delete role", "true"));
                options.AddPolicy("EditRolePolicy", policy => policy.AddRequirements(new ManageAdminRolesAndClaimsRequirement()));
                options.AddPolicy("CreateRolePolicy", policy => policy.RequireClaim("Create role", "true"));

            });

            services.AddSingleton<IAuthorizationHandler, EditOnlyOtherRolesAndClaims>();
            services.AddSingleton<IAuthorizationHandler, SuperAdminHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Error");
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Hello");
                });
            });

        }
    }
}
