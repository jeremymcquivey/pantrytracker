using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Prism.Ioc;
using System;
using System.ComponentModel;
using System.Net.Http;
using System.Reflection;
using Unity;
using Unity.Microsoft.DependencyInjection;
using Xamarin.Essentials;

namespace PantryTrackers
{
    public static class Startup
    {
        const string environment = "Development";

        public static IServiceProvider ServiceProvider { get; set; }
        public static void Init(IUnityContainer unityContainer)
        {
            var a = Assembly.GetExecutingAssembly();
            using var stream = a.GetManifestResourceStream($"PantryTrackers.appsettings.{environment}.json");

            var host = new HostBuilder()
                        .ConfigureHostConfiguration(c =>
                        {
                // Tell the host configuration where to file the file (this is required for Xamarin apps)
                c.AddCommandLine(new string[] { $"ContentRoot={FileSystem.AppDataDirectory}" });

                //read in the configuration file!
                c.AddJsonStream(stream);
                        })
                        .ConfigureServices((c, x) =>
                        {
                // Configure our local services and access the host configuration
                ConfigureServices(c, x, unityContainer);
                        })
                        .ConfigureLogging(l => l.AddConsole(o =>
                        {
                //setup a console logger and disable colors since they don't have any colors in VS
                o.DisableColors = true;
                        }))
                        .Build();

            //Save our service provider so we can use it later.
            ServiceProvider = host.Services;
            
        }

        static void ConfigureServices(HostBuilderContext ctx, IServiceCollection services, IUnityContainer unity)
        {
            services.AddHttpClient();
            services.BuildServiceProvider(unity);
        }
    }
}
