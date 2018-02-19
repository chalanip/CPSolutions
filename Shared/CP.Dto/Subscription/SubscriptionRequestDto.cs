
namespace CP.Dto.Subscription
{
    public class SubscriptionRequestDto
    {
        /// <summary>
        /// Application ID as given when provisioned. (mandatory)
        /// </summary>
        public string applicationId { get; set; }

        /// <summary>
        /// Password as given when provisioned. (mandatory)
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// API version shall be numbered as 1.0, 2.0 etc (optional)
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// Whether the subscriber is to opt in or opt out. 
        /// Action = 1 for registering, Action = 0 for un-registering 
        /// </summary>
        public string action { get; set; }

        /// <summary>
        /// Telephone number of the user to be subscribed.tel – for MSISDN  (mandatory)
        /// eg: tel:94771234567 
        /// Note: tel might be a masked number depending on the application
        /// </summary>
        public string subscriberId { get; set; }
        
    }
}
