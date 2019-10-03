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
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography.X509Certificates;
using System.IO;

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
            services.Configure<AppSettings>(Configuration.GetSection("Authentication"));

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

            /*var certificatePassword = Environment.GetEnvironmentVariable("CertificatePassword");
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();
            var cert = new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"certificate.{environment}.pfx"), certificatePassword, X509KeyStorageFlags.MachineKeySet);
            X509SecurityKey key = new X509SecurityKey(cert);*/

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddJwtBearer(options =>
                     {
                         options.Authority = Configuration.GetSection("Authentication:STSAuthority").Value;
                         options.RequireHttpsMetadata = false;
                         options.Audience = "pantrytrackers-ui";
                         /*options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = key
                         };*/
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
                app.UseDeveloperExceptionPage();
                //app.UseExceptionHandler("/Error");
            }

            app.UseCors("AllRequests");
            app.UseStaticFiles();
            app.UseSwashbuckle(env);
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}