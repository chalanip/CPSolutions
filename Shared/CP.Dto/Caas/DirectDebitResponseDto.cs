﻿namespace CP.Dto.Caas
{
    public class DirectDebitResponseDto
    {
        /// <summary>
        /// Status of the operation.Only a single set of elements can be sent perrequest. (Mandatory)
        /// </summary>
        public string statusCode { get; set; }

        /// <summary>
        /// Date Time Stamp
        /// </summary>
        public string timeStamp { get; set; }

        /// <summary>
        /// Short Description
        /// </summary>
        public string shortDescription { get; set; }

       
        /// <summary>
        /// The textual description of the operation’s status.Only a single set of elements can be sent per request. (Mandatory)
        /// </summary>
        public string statusDetail { get; set; }

        /// <summary>
        /// This is the transaction ID generated by the application to map the request with the response.
        /// This is needed to avoid any conflicts when SP inquires about a transaction.
        /// Only a single value can be sent per request. (String (32) - Mandatory)
        /// </summary>
        public string externalTrxId { get; set; }

        /// <summary>
        /// Long Description
        /// </summary>
        public string longDescription { get; set; }

        /// <summary>
        /// Internal TrxId
        /// </summary>
        public string internalTrxId { get; set; }


        /* Comprehensive sample request:    
          {  
             “statusCode”: “S1000”,

            “timeStamp”: “2012-07-30T12:48:10-0400”,

            “shortDescription”: “short Description”,

            “statusDetail”: “Success”,

            “externalTrxId”: “12345678901234567890123456789012”,

            “longDescription”: “Long Description”,

            “internalTrxId”: “321”
        } */
    }
}
