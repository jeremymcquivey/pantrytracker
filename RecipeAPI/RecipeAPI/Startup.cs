using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PantryTracker.ExternalServices;
using RecipeAPI.Helpers;
using System;
using RecipeAPI.Models;

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
            services.Configure<AppSettings>(Configuration);

            var connStr = Environment.GetEnvironmentVariable("ConnectionString", EnvironmentVariableTarget.Process);
            services.AddDbContext<RecipeContext>(options => options.UseSqlServer(connStr));
            services.AddTransient<IOCRService, OCR>();

            services.AddCors(options =>
            {
                options.AddPolicy("AllRequests", builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                    .AllowCredentials();
                });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddJwtBearer(options =>
                     {
                         // base-address of your identityserver
                         options.Authority = "http://localhost:4242/";
                         options.RequireHttpsMetadata = false;
                         // name of the API resource
                         options.Audience = "projects-api";
                     });

            services.AddSwashbuckle();
            services.AddMvc();
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
            app.UseSwashbuckle(env);
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}