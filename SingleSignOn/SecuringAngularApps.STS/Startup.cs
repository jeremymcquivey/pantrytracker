using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PantryTracker.SingleSignOn.STS;
using PantryTracker.SingleSignOn.STS.Models;
using SecuringAngularApps.STS.Data;
using SecuringAngularApps.STS.Models;
using SecuringAngularApps.STS.Quickstart.Account;

namespace SecuringAngularApps.STS
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("SingleSignOn"));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(System.Environment.GetEnvironmentVariable("ConnectionString")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", corsBuilder =>
                {
                    corsBuilder.AllowAnyHeader()
                               .AllowAnyMethod()
                               .AllowAnyOrigin()
                               .AllowCredentials();
                });
            });

            services.AddMvc();
            services.AddTransient<IProfileService, CustomProfileService>();

            var builder = services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;
                    options.Authentication.CookieLifetime = TimeSpan.FromMinutes(15);
                })
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddClientStore<ClientStore>()
                .AddCorsPolicyService<CORSService>()
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<CustomProfileService>();

            var certificatePassword = System.Environment.GetEnvironmentVariable("CertificatePassword");
            var environment = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();
            var certificateLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"certificate.{environment}.pfx");
            var bytes = File.ReadAllBytes(certificateLocation);

            var certificate = new X509Certificate2(bytes, certificatePassword, X509KeyStorageFlags.MachineKeySet);
            builder.AddSigningCredential(certificate);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }
    }
}
