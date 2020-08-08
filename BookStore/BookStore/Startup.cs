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

            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>();

            services.AddMvc(options => options.EnableEndpointRouting = false);
            services.AddScoped<IPersonRepository, SqlPersonRepository>();
            services.AddScoped<IAuthorRepository, SqlAuthorRepository>();
            services.AddScoped<ICategoryRepository, SqlCategoryRepository>();
            services.AddScoped<IBookRepository, SqlBookRepository>();
            services.AddScoped<IAuthorBookRepository, SqlAuthorBookRepository>();
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
