using Refit;

namespace Schedule.Bot.Clients.GoogleAnalitics.Requests
{
    public class EventTracking : Request
    {
        private const string Event = "event";

        public EventTracking(string action, string clientId, string label, string category, string trackingId)
        {
            Category = category;
            Action = action;
            Label = label;
            ClientId = clientId;
            TrackingId = trackingId;
        }

        public override string HitType => Event;

        /// <summary>
        /// Event Category. Required.
        /// </summary>
        [AliasAs("ec")]
        public string Category { get; set; }
        /// <summary>
        /// Event Action. Required.
        /// </summary>
        [AliasAs("ea")]
        public string Action { get; set; }
        /// <summary>
        /// Event label.
        /// </summary>
        [AliasAs("el")]
        public string Label { get; set; }
        /// <summary>
        /// Event value.
        /// </summary>
        [AliasAs("ev")]
        public int Value { get; set; }
    }
}
