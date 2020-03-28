using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Schedule.Bot.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Schedule.Bot.Services
{
    public class YandexMapService : IMapService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
        const string WorkPoint = "37.658979,55.789592";
        const string MetroPoint = "37.676294,55.789472";

        const string InitUrl = @"https://api-maps.yandex.ru/2.1/?lang=ru_RU&load=route,router.Route";
        const string DurationUrl = @"https://api-maps.yandex.ru/services/route/2.0/?lang=ru_RU&token={0}&rll={1}~{2}&rtm=dtr";

        const string RegexTokenRule = "\"token\":\"(?<token>\\w*)\"";

        public YandexMapService(IMemoryCache memoryCache, IConfiguration configuration)
        {
            this._memoryCache = memoryCache;
            this._configuration = configuration;
        }

        public async Task<string> GetDurationToMetro() => await GetCachedDuration(WorkPoint, MetroPoint).ConfigureAwait(false);
        public async Task<string> GetDurationToWork() => await GetCachedDuration(MetroPoint, WorkPoint).ConfigureAwait(false);

        private async Task<string> GetCachedDuration(string from, string to)
        {
            string duration = null;
            if (!_memoryCache.TryGetValue(to, out duration))
            {
                duration = await GetYandexDuration(from, to);
                _memoryCache.Set(to, duration, TimeSpan.FromMinutes(1));

            }
            return duration;
        }

        protected virtual async Task<string> GetYandexDuration(string from, string to)
        {
            string duration = null;

            try
            {
                using (var client = new HttpClient())
                {

                    var initResponse = await client.GetAsync(InitUrl).ConfigureAwait(false);

                    if (initResponse.IsSuccessStatusCode)
                    {
                        var jsInit = await initResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                        var token = TokenParser(jsInit);
                        if (!string.IsNullOrWhiteSpace(token))
                        {
                            var url = string.Format(DurationUrl, token, from, to);
                            var durationResponse = await client.GetAsync(url).ConfigureAwait(false);
                            if (durationResponse.IsSuccessStatusCode)
                            {
                                var json = await durationResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
                                var jObject = JObject.Parse(json);

                                duration = jObject.SelectToken("data.features[0].properties.RouteMetaData.DurationInTraffic.text").Value<string>();
                                //Если ерунду возвращает, то обнулим
                                if (duration.StartsWith("0")) duration = null;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
            }

            return duration;
        }

        private static string TokenParser(string initInfo)
        {
            var regex = new Regex(RegexTokenRule);

            var match = regex.Match(initInfo);

            return match.Success ? match.Groups["token"].Value : string.Empty;
        }



    }
}
