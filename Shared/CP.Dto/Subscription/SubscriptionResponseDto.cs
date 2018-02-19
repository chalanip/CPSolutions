namespace CP.Dto.Subscription
{
    public class SubscriptionResponseDto
    {
        /// <summary>
        /// API version shall be numbered as 1.0, 2.0 etc
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// Request ID to uniquely identify the request within the SDP
        /// </summary>
        public string requestId { get; set; }

        /// <summary>
        /// The status code for the entire request
        /// </summary>
        public string statusCode { get; set; }

        /// <summary>
        /// The status detail for the entire request
        /// </summary>
        public string statusDetail { get; set; }

        /// <summary>
        /// Enum:  REGISTERED / UNREGISTERED / PENDING CHARGE – Subscriber is not charged yet. (Optional)
        /// </summary>
        public string subscriptionStatus { get; set; }
    }
}
