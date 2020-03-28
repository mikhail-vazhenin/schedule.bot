using System.Threading.Tasks;
using Refit;
using Schedule.Bot.Clients.GoogleAnalitics.Requests;

namespace Schedule.Bot.Clients.GoogleAnalitics
{
    public interface IGoogleAnalyticsClient
    {
        [Post("/collect")]
        Task SendEventTrack([Body(BodySerializationMethod.UrlEncoded)]EventTracking eventTracking);
    }
}