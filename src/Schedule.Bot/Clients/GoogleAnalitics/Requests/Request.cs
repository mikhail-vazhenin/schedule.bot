using Refit;

namespace Schedule.Bot.Clients.GoogleAnalitics.Requests
{
    public abstract class Request
    {
        private const string V = "1";

        [AliasAs("v")]
        public string Version => V;
        [AliasAs("cid")]
        public string ClientId { get; set; }

        [AliasAs("tid")]
        public string TrackingId { get; set; }
        [AliasAs("t")]
        public abstract string HitType { get; }
    }
}
