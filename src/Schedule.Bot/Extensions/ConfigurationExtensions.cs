using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Bot.Extensions
{
    public static class ConfigurationExtensions
    {
        public static string GetTitle(this IConfiguration configuration, string command)
        {
            var values = configuration.GetSection("titles").GetChildren().ToDictionary(k => k.Key, v => v.Value);
            return values[command];
        }

        public static string ToMetroTitle(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("titles:toMetro");
        }

        public static string ToWorkTitle(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("titles:toWork");
        }

        public static string[] ToMetroAliases(this IConfiguration configuration)
        {
            return configuration.GetValue<string[]>("aliases:toMetro");
        }

        public static IDictionary<string, string[]> Aliases(this IConfiguration configuration)
        {
            var aliases = configuration.GetSection("aliases").GetChildren().ToDictionary(k => k.Key, v => v.GetChildren().Select(c => c.Value).ToArray());
            return aliases;
        }

        public static string[] ToWorkAliases(this IConfiguration configuration)
        {
            return configuration.GetValue<string[]>("aliases:toWork");
        }

        public static string GoogleApiKey(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("googleAnalitics:key");
        }

        public static string GoogleAnalyticsUrl(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("googleAnalitics:url");
        }

        public static string TelegramKey(this IConfiguration configuration)
        {
            return configuration.GetValue<string>("telegram:key");
        }
    }
}
