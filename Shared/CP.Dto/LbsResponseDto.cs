namespace CP.Dto
{
    public class LbsResponseDto
    {
        public string StatusCode { get; set; }
        public string TimeStamp { get; set; }
        public string SubscriberState { get; set; }
        public string StatusDetail { get; set; }
        public string HorizontalAccuracy { get; set; }
        public string Longitude { get; set; }
        public string Freshness { get; set; }
        public string Latitude { get; set; }
        public string MessageId { get; set; }
        public string Version { get; set; }
    }
}
