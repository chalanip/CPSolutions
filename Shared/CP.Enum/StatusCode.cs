namespace CP.Enum
{
    public enum StatusCode
    {
        /// <summary>
        /// General success code
        /// </summary>
        Success = 1000,        
        AddUserFailed = 1001,
        InvalidInputData = 1002,
        NoUserData = 1003,

        /* Subscription error codes */
        SubscriptionServiceFailed = 2000,

        /* Caas error codes */
        CaasServiceFailed = 2100,

        
        #region Common Error Codes 3000 - 3099

        Error = 3000,
        InvalidParameter = 3001,
        NA = 3002,
        Unauthorized = 3003,

        #endregion Common Error Codes

    }
}
