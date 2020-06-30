// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PantryTrackers.STS;
using PantryTrackers.STS.Models;
using PantryTrackers.STS.Data;
using PantryTrackers.STS.Integrations;
using PantryTrackers.STS.Quickstart.Account;
using System;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using IdentityServer4.Services;

namespace PantryTrackers
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllRequests", builder =>
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowAnyOrigin();
                });
            });

            services.Configure<AppSettings>(Configuration.GetSection("SingleSignOn"));

            string connectionString = Environment.GetEnvironmentVariable("ConnectionString");
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            //services.AddSingleton<ICorsPolicyService, CORSService>();
            services.AddTransient<EmailClient>();
            services.AddHttpClient("SendGridClient", client =>
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("SendGridClientId"));
                client.BaseAddress = new Uri(EmailClient.SendGridMailEndpoint);
            });

            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;
                options.Authentication.CookieLifetime = TimeSpan.FromMinutes(30);
            })
                .AddInMemoryApiResources(Config.ApiResources)
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddClientStore<ClientStore>()
                .AddCorsPolicyService<CORSService>()
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<CustomProfileService>();

            var certificatePassword = Environment.GetEnvironmentVariable("CertificatePassword");
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();
            var certificateLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"certificate.{environment}.pfx");
            var bytes = File.ReadAllBytes(certificateLocation);

            var certificate = new X509Certificate2(bytes, certificatePassword, X509KeyStorageFlags.MachineKeySet);
            builder.AddSigningCredential(certificate);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                //app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseCors("AllRequests");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseIdentityServer();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}