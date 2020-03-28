using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Schedule.Bot.Extensions;
using Toolkie.Configuration.Settings;

namespace Schedule.Bot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseToolkieConfiguration(args)
                .UseSerilogConfiguration()
                .UseStartup<Startup>();
    }
}
