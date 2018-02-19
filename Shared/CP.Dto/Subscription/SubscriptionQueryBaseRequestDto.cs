namespace CP.Dto.Subscription
{
    public class SubscriptionQueryBaseRequestDto
    {
        /// <summary>
        /// Application ID as given when provisioned.
        /// </summary>
        public string applicationId { get; set; }

        /// <summary>
        /// Password as given when provisioned.
        /// </summary>
        public string password { get; set; }

    }
}
