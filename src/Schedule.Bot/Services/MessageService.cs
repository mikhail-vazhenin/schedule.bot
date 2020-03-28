using Microsoft.Extensions.Configuration;
using Schedule.Bot.Clients.GoogleAnalitics;
using Schedule.Bot.Clients.GoogleAnalitics.Requests;
using Schedule.Bot.Extensions;
using Schedule.Bot.Services.Builder;
using Schedule.Bot.Services.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Schedule.Bot.Services
{
    public class MessageService : IMessageService
    {
        private readonly IHolidayStorage _holidayStorage;
        private readonly ITimeStorage _timeStorage;
        private readonly IMapService _mapService;
        private readonly IGoogleAnalyticsClient _googleAnalyticsClient;
        private readonly IConfiguration _configuration;

        private const string ToMetroCommand = "toMetro";
        private const string ToWorkCommand = "toWork";

        public MessageService(ITimeStorage timeStorage,
            IHolidayStorage holidayStorage,
            IMapService mapService,
            IGoogleAnalyticsClient googleAnalyticsClient,
            IConfiguration configuration)
        {
            this._googleAnalyticsClient = googleAnalyticsClient;
            this._mapService = mapService;
            this._timeStorage = timeStorage;
            this._holidayStorage = holidayStorage;
            this._configuration = configuration;
        }


        public async Task<string> GetNearestTimeMessage(string clientId, string message, DateTime? date = null)
        {
            var now = date ?? GetDateTimeNow();
            var command = ParseCommand(message);
            var title = _configuration.GetTitle(command);

            var analyticTask = _googleAnalyticsClient.SendEventTrack(new EventTracking(title, clientId, message, "NearestTime", _configuration.GoogleApiKey()));

            if (_holidayStorage.IsHoliday(now)) return MessageBuilder.TodayIsHoliday;

            var nearestTimes = Choose(command,
                () => _timeStorage.GetNearestToMetro(now),
                () => _timeStorage.GetNearestToWork(now));

            if (nearestTimes == null) return null;
            if (!nearestTimes.Any()) return MessageBuilder.TooLate;

            //YandexApi changed. Not working
            //var duration = await Choose(command,
            //    _mapService.GetDurationToMetro,
            //    _mapService.GetDurationToWork);

            var responseMessage = new MessageBuilder()
                .AddRoute(title)
                .AddTimes(nearestTimes)
                .AddShortDay(_holidayStorage.IsShortDay(now))
                .ToString();

            await analyticTask;
            return responseMessage;
        }

        private TResult Choose<TResult>(string command,
            Func<TResult> toMetroFunc,
            Func<TResult> toWorkFunc)
        {
            switch (command)
            {
                case ToMetroCommand: return toMetroFunc();
                case ToWorkCommand: return toWorkFunc();
            }
            return default(TResult);
        }

        private string ParseCommand(string message)
        {
            var lowerCaseMessage = message.ToLower();
            var aliases = _configuration.Aliases();

            var command = aliases.FirstOrDefault(k => k.Value.Any(w => lowerCaseMessage.Contains(w.ToLower()))).Key;
            return command;
        }

        protected virtual DateTime GetDateTimeNow()
        {
            return DateTime.UtcNow.AddHours(3);
        }
    }
}
