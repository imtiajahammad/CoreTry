using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreTry.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreTry
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration config, IHostingEnvironment env)
        {
            _config = config;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<AppDbContext>(options => options.UseSqlServer(_config.GetConnectionString("EmployeeDBConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>(options => {
                options.Password.RequiredLength = 4;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                        .AddEntityFrameworkStores<AppDbContext>();

            /*services.Configure<IdentityOptions>(options=> {
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
                options.Password.RequireNonAlphanumeric = false;
            })*/

            //services.AddMvc();
            services.AddMvc(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                .RequireAuthenticatedUser()
                                .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            services.ConfigureApplicationCookie(options=>
            {
                options.AccessDeniedPath = new PathString("/Administration/AccessDenied");
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("DeleteRolePolicy", 
                    policy => policy.RequireClaim("Delete Role")
                                    .RequireClaim("Create Role"));
                options.AddPolicy("EditRolePolicy",
                   policy => policy.RequireClaim("Edit Role", "true"));/*claim.Value is case sensative*/
                options.AddPolicy("AdminRolePolicy",
                    policy => policy.RequireRole("ADMIN,NGB-ADMIN"));
            });
            //services.AddMvc().AddXmlSerializerFormatters();//to get xml format
            //services.AddMvcCore();
            //services.AddSingleton<IEmployeeRepository, MockEmployeeRepository>();
            services.AddScoped<IEmployeeRepository, SQLEmployeeRepository>();
        }

        // This method gets called by the ru    ntime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //app.UseDeveloperExceptionPage();
                /*app.UseStatusCodePages();
                 app.UseStatusCodePagesWithRedirects("/Error/{0}");
                 
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                */
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                app.UseExceptionHandler("/Error");
            }
            /*else if( env.IsStaging() || env.IsProduction() || env.IsEnvironment("UAT") )
            {
                app.UseExceptionHandler("/Error");
            }*/

            app.UseStaticFiles();//to get the images and files form wwwroot
            //app.UseDefaultFiles();
            //app.UseFileServer();//works for UseStaticFiles and UseDefaultFiles both


            //app.UseMvcWithDefaultRoute();//adds mvc with default route


            app.UseAuthentication();


            app.UseMvc(routes =>
            {
                routes.MapRoute("default", "{controller=home}/{action=index}/{id?}");
            });//adds mvc only without default route
            //app.UseMvc();

            //app.UseMvc();//for using attribute routing 
            /*app.Run(async (context) =>
            {
                //await context.Response.WriteAsync("Hello World!");
                //await context.Response.WriteAsync(System.Diagnostics.Process.GetCurrentProcess().ProcessName);
                await context.Response.WriteAsync("Hello from startup.app.run ->" +env.EnvironmentName);
            });*/
        }
    }
}
