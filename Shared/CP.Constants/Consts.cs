namespace CP.Constants
{
    /// <summary>
    /// constant names
    /// </summary>
    public class Consts
    {
        //URLs
        //public const string URL_BASE_ADDRESS = "http://localhost:7000/";//https://api.dialog.lk/

        public const string URL_LBS_GET_LOCATION = "lbs/locate";

        public const string URL_SMS_SEND = "sms/send";
        /* Status Codes */
        /// <summary>
        /// Process completed successfully for all the available destination numbers.
        /// </summary>
        public const string SUCCESS = "S1000";
        public const string USER_ALREADY_REGISTERED = "E1351";
        public const string USER_NOT_REGISTERED = "E1356";
        

        /* Subscription */
        public const string URL_SUBSCRIPTION_ADD = "subscription/send";
        public const string URL_SUBSCRIPTION_GET_STATUS = "subscription/getStatus";
        public const string URL_SUBSCRIPTION_QUERY_BASE = "subscription/query-base";        
        //The subscription notification is received to the URL which is configured when provisioning.
        public const string URL_SUBSCRIPTION_NOTIFICATION = "subscription/xxxx";

        /* Caas */
        public const string URL_CAAS_QUERY_BALANCE = "caas/get/balance";// prod = caas/balance/query
        public const string URL_CAAS_DIRECT_DEBIT = "caas/direct/debit";
        public const string URL_CAAS_PAYMENT_INSTRUMENT_LIST = "caas/list/pi";

        //User Table columns
        public const string COL_User_ID = "UserId";
        public const string COL_User_Name = "UserName";
        public const string COL_User_NIC = "NIC";
        public const string COL_User_ContactNo = "ContactNo";
        public const string COL_User_Town = "Town";

        //Category Table columns
        public const string COL_Category_Name = "CategoryName";



    }
}
