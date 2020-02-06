using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.Swagger;
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
                    new OpenApiInfo
                    {
                        Title = "Recipe API",
                        Version = "v1",
                        Description = "Manages the PantryTracker recipes"
                    });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT from PantryTracker's SSO API",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement 
                {{  
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }});

                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "RecipeAPI.xml"));
            });

            return services;
        }

        /// <summary>
        /// Configures UI elements for Swagger.
        /// </summary>
        public static IApplicationBuilder UseSwashbuckle(this IApplicationBuilder app,
                                                              IHostEnvironment env)
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
