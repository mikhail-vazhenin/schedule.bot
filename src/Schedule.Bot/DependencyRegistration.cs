using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using Schedule.Bot.Clients.GoogleAnalitics;
using Schedule.Bot.Extensions;
using Schedule.Bot.Services;
using Schedule.Bot.Services.Interfaces;
using System;
using Telegram.Bot;

namespace Schedule.Bot
{
    public static class DependencyRegistration
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<ITelegramService, TelegramService>();

            services.AddSingleton<ITimeStorage>(c => new JsonTimeStorage("Schedule.json"));
            services.AddSingleton<IHolidayStorage>(c => new HolidayStorage("Holidays.txt"));

            services.AddTransient<ITelegramBotClient>(c =>
                new TelegramBotClient(c.GetRequiredService<IConfiguration>().TelegramKey()));
            services.AddSingleton<IMapService, YandexMapService>();
            services.AddMemoryCache();

            services.AddRefitClient<IGoogleAnalyticsClient>()
                .ConfigureHttpClient((s, c) =>
                    c.BaseAddress = new Uri(s.GetRequiredService<IConfiguration>().GoogleAnalyticsUrl()));
        }
    }
}
