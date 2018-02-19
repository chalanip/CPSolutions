namespace CP.Dto.Subscription
{
    public class SubscriptionNotificationRequestDto
    {
        /// <summary>
        /// Application ID as given when provisioned
        /// </summary>
        public string applicationId { get; set; }

        /// <summary>
        /// Number of days the subscription is made for, till it is billed again.
        /// Value specified in number of days – monthly subs will be specified as 30 days.
        /// </summary>
        public string frequency { get; set; }

        /// <summary>
        /// Status of the subscription (Enum: REGISTERED/UNREGISTERED/PENDING CHARGE)
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// Subcribed users ID
        /// subscriberID: tel:94771234567
        /// If the application is masked then the number will be masked.
        /// </summary>
        public string subscriberId { get; set; }

        /// <summary>
        /// API version shall be numbered as 1.0, 2.0 etc
        /// </summary>
        public string version { get; set; }

        /// <summary>
        /// Time of subscription – yyyyMMddhhmmss
        /// </summary>
        public string timeStamp { get; set; }

    }
}
