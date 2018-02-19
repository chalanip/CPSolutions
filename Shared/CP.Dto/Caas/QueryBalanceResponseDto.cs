namespace CP.Dto.Caas
{
    public class QueryBalanceResponseDto
    {
        /// <summary>
        /// Account type of the subscriber id.
        /// E.g. Prepaid/Postpaid for GSM domain.Only a single set of elements can be sent per request. (Mandatory)
        /// </summary>
        public string accountType { get; set; }

        /// <summary>
        ///Account status of the subscriber ID. Only a single set of elements can be sent perrequest. (Mandatory)
        /// </summary>
        public string accountStatus { get; set; }

        /// <summary>
        /// Status of the operation.Only a single set of elements can be sent perrequest. (Mandatory)
        /// </summary>
        public string statusCode { get; set; }

        /// <summary>
        /// The textual description of the operation’s status.Only a single set of elements can be sent per request. (Mandatory)
        /// </summary>
        public string statusDetail { get; set; }

        /// <summary>
        /// Available chargeable balance of the subscriber.Refers to either remaining account balance(prepaid user) or the difference between credit limit and outstanding bill (postpaid user). 
        /// Only a single value can be sent per request. String(rounded up to two decimal points)(Mandatory)
        /// </summary>
        public string chargeableBalance { get; set; }



        /* Comprehensive sample request:    
          {  
            “chargeableBalance”: “300.0”,

            “statusCode”: “S1000”,

            “statusDetail”: “Success”,

            “accountStatus”: “Active”,

            “accountType”: “Pre Paid”
        } */

    }
}
