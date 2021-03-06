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
using PantryTracker.Model.Products;
using PantryTrackers.Integrations.Kroger;
using PantryTracker.Model;
using RecipeAPI.ExternalServices;
using Microsoft.Extensions.Hosting;
using PantryTrackers.Integrations.Walmart;
using RecipeAPI.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;

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
            services.AddSingleton<ICacheManager, MemoryCache>();
            services.AddScoped<KrogerService>();
            services.AddScoped<WalmartService>();
            services.AddScoped<UPCLookup>();
            services.AddScoped<ProductService>();
            services.AddHttpClient();

            services.AddCors(options =>
            {
                options.AddPolicy("AllRequests", builder =>
                {
                    builder.SetIsOriginAllowed(x => _ = true)
                           .AllowAnyHeader()
                           .AllowAnyMethod()
                           .WithOrigins(Settings.AllowedOrigins)
                           .AllowCredentials();
                });
            });

            var certificatePassword = Environment.GetEnvironmentVariable("CertificatePassword");
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").ToLower();
            var cert = new X509Certificate2(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"certificate.{environment}.pfx"), certificatePassword, X509KeyStorageFlags.MachineKeySet);
            X509SecurityKey key = new X509SecurityKey(cert);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddJwtBearer(options =>
                     {
                         options.Authority = Configuration.GetSection("Authentication:STSAuthority").Value;
                         options.RequireHttpsMetadata = false;
                         options.Audience = "pantrytrackers-api";
                         options.TokenValidationParameters = new TokenValidationParameters
                         {
                             ValidateIssuerSigningKey = true,
                             IssuerSigningKey = key
                         };
                     });

            services.AddApplicationInsightsTelemetry();
            services.AddSwashbuckle();
            services.AddMvc(options =>
                {
                    options.EnableEndpointRouting = false;
                })
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            // Auto Mapper Configurations
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ShimmingProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.Secure = CookieSecurePolicy.Always;
            });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment() || true)
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseCors("AllRequests");
            app.UseStaticFiles();
            app.UseSwashbuckle(env);
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}