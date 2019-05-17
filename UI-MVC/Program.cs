using System;
using System.IO;
using COI.UI.MVC.Services;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace COI.UI.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            // adapted to make sure roles are created first
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var serviceProvider = services.GetRequiredService<IServiceProvider>();
                    var conf = services.GetRequiredService<IConfiguration>();

                    RoleSeed.CreateRoles(serviceProvider, conf).Wait();
                }
                catch (Exception e)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(e, "An error occurred while creating roles");
                }
            }
            
            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        // configure environment files
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("coi_env.json", optional: false, reloadOnChange: true); // TODO implement IOptions
                        config.AddJsonFile("prod_env.json", optional: true, reloadOnChange: true); // TODO implement IOptions
                    })
                .UseStartup<Startup>();
    }
}
