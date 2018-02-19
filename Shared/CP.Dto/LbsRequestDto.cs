namespace CP.Dto
{
    public class LbsRequestDto
    {
        public string applicationId { get; set; }
        public string password { get; set; }
        public string subscriberId { get; set; }
        public string serviceType { get; set; }
        public string responseTime { get; set; }
        public string freshness { get; set; }
        public string horizontalAccuracy { get; set; }
        public string version { get; set; }
    }
}
