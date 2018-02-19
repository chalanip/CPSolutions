namespace CP.Dto.Subscription
{
    public class SubscriptionQueryBaseResponseDto
    {
        /// <summary>
        /// API version shall be numbered as 1.0, 2.0 etc
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// The current subscribers base size. (optional)
        /// </summary>
        public string baseSize { get; set; }
        
        /// <summary>
        /// The status code for the entire request
        /// </summary>
        public string statusCode { get; set; }

        /// <summary>
        /// The status detail for the entire request
        /// </summary>
        public string statusDetail { get; set; }
    }
}
