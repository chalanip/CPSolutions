namespace CP.Dto.Caas
{
    public class QueryBalanceRequestDto
    {
        /// <summary>
        /// Used to identify the application. This is a unique identifier generated while provisioning 
        /// an application.Only a single value can be sent per request. (String (32) - Mandatory)
        /// </summary>
        public string applicationId { get; set; }

        /// <summary>
        /// Used to authenticate the application originated message against the service providers credentials.
        /// Encoded, single value. (String (32) - Mandatory)
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// This can be the MSISDN or the Username of the subscriber whose account balance is being queried.
        /// Note: tel might be a masked number depending on the type of the application.
        /// Only a single value can be sent per request. (Mandatory)
        /// </summary>
        public string subscriberId { get; set; }

        /// <summary>
        /// The name of the payment instrument.Only a single value can be sent per request. (Enum - Mandatory)
        /// </summary>
        public string paymentInstrumentName { get; set; }

        /// <summary>
        /// The account of the payment instrument.Only a single value can be sent per request. (Optional)
        /// </summary>
        public string accountId { get; set; }

        /// <summary>
        /// The currency of the amount.Only a single value can be sent per request.Only ‘LKR’ is allowed. (Optional)
        /// </summary>
        public string currency { get; set; }

        /* Comprehensive sample request:    
         {  
            “applicationId”: “APP_000018”,

            “password”: “95904999aa8edb0c038b3295fdd271de”,

            “subscriberId”: “5C74B588F97”,

            “paymentInstrumentName”: “MobileAccount”,

            “accountId”: “12345”,

            “currency”: “LKR”
        } */

    }
}
