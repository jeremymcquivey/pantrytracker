using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RecipeAPI.Data;
using PantryTracker.ExternalServices;
using RecipeAPI.Helpers;
using System;

#pragma warning disable 1591
namespace RecipeAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private AppSettings Settings => Configuration.Get<AppSettings>();

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSwashbuckle();
            services.Configure<AppSettings>(Configuration);

            var connStr = Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.Process);
            services.AddDbContext<RecipeContext>(options => options.UseSqlServer(connStr));
            services.AddTransient<IOCRService, OCR>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();
            app.UseMvc();
            app.UseSwashbuckle(env);
        }
    }
}