using System.IO;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace COI.UI.MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.SetBasePath(Directory.GetCurrentDirectory());
                        config.AddJsonFile("coi_env.json", optional: false, reloadOnChange: true); // TODO implement IOptions
                        config.AddJsonFile("prod_env.json", optional: true, reloadOnChange: true); // TODO implement IOptions
                    })
                .UseStartup<Startup>();
    }
}
