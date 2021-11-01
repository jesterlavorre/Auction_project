using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IepProjekat.Factories;
using IepProjekat.Models.Database;
using IepProjekat.Models.Initializers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IepProjekat
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
            services.AddDbContext<IepProjekatContext> ( 
                options => options.UseSqlServer ( this.Configuration.GetConnectionString ( "IepDB" ) )
            );
            services.AddIdentity<User, IdentityRole> ( 
                options => {
                    options.User.RequireUniqueEmail = true;

                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = 2;
                }
            ).AddEntityFrameworkStores<IepProjekatContext> ( );
            services.AddRazorPages ( ).AddRazorRuntimeCompilation ( );

            services.AddAutoMapper ( typeof ( Startup ) );

            services.ConfigureApplicationCookie (
                options => {
                    options.LoginPath = "/User/LogIn";
                    options.AccessDeniedPath = "/Error";
                }
            );

            services.AddScoped<IUserClaimsPrincipalFactory<User>, ClaimFactory> ( );

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, UserManager<User> userMenager)
        {
            if (env.IsDevelopment())
            {
                UserInitializer.initialize(userMenager);
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication ( );
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
