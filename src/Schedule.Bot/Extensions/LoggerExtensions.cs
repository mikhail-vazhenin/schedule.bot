using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Schedule.Bot.Extensions
{
    public static class LoggerExtensions
    {
        public static IWebHostBuilder UseSerilogConfiguration(this IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureLogging(b => b.ClearProviders());
            webHostBuilder.UseSerilog((hostingContext, loggerConfiguration) =>
            {
                loggerConfiguration.ReadFrom.Configuration(hostingContext.Configuration);
                loggerConfiguration
                    .Enrich.WithProperty("environment", hostingContext.HostingEnvironment.EnvironmentName);
            });

            return webHostBuilder;
        }
    }
}
