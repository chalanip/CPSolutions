namespace CP.Dto.Subscription
{
    public class SubscriptionStatusRequestDto
    {
        /// <summary>
        /// Application ID as given when provisioned.
        /// </summary>
        public string applicationId { get; set; }

        /// <summary>
        /// Password as given when provisioned.
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// Subcribed users ID
        /// subscriberID: tel:94771234567
        /// If the application is masked then the number will be masked.
        /// </summary>
        public string subscriberId { get; set; }
    }
}
