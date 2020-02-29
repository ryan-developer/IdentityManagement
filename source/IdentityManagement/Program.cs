using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace IdentityManagement
{
    public class Program
    {
        public static void Main(string[] args) => CreateHostBuilder(args).Build().Run();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseKestrel(k => { k.Limits.KeepAliveTimeout = TimeSpan.FromMinutes(1); });
                    webBuilder.UseStartup<Startup>();
                    webBuilder.ConfigureAppConfiguration((httpContext, configuration) =>
                    {
                        string baseConfigPath = Path.Combine(httpContext.HostingEnvironment.ContentRootPath, "Configuration", "Settings");
                        configuration.SetBasePath(baseConfigPath);
                    });
                    webBuilder.ConfigureLogging(builder =>
                    {
                        builder.AddApplicationInsights();
                        builder.AddConsole();
                    });
                });
    }
}
