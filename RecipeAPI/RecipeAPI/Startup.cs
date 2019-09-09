using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PantryTracker.ExternalServices;
using RecipeAPI.Helpers;

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