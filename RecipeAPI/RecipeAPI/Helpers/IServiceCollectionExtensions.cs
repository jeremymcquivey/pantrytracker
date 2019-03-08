using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RecipeAPI.Helpers
{
    /// <summary>
    /// Defines extension helper methods to clean up the startup class.
    /// </summary>
    public static class IServiceCollectionExtensions
    {
        /// <summary>
        /// Configures the Swashbuckle documentation for the API
        /// </summary>
        public static IServiceCollection AddSwashbuckle(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(
                    "v1",
                    new Info
                    {
                        Title = "Recipe API",
                        Version = "v1",
                        Description = "Manages the PantryTracker recipes"
                    });

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    In = "header",
                    Description = "JWT from PantryTracker's SSO API",
                    Name = "Authorization",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>()
                {
                    { "Bearer", Enumerable.Empty<string>() }
                });

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "RecipeAPI.xml"));
            });

            return services;
        }

        /// <summary>
        /// Configures UI elements for Swagger.
        /// </summary>
        public static IApplicationBuilder UseSwashbuckle(this IApplicationBuilder app,
                                                              IHostingEnvironment env)
        {
            var appName = "Recipe API";
            if (!env.IsProduction())
            {
                appName += $" ({env.EnvironmentName})";
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", appName);
                c.RoutePrefix = "swagger";
            });

            return app;
        }
    }
}
