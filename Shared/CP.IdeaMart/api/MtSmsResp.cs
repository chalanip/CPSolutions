using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP.IdeaMart.api
{
    /// <summary>
    /// Summary description for MtSmsResp
    /// </summary>
    public class MtSmsResp : Message
    {
        public string version { get; set; }
        public string requestId { get; set; }
        public string statusCode { get; set; }
        public string statusDetail { get; set; }
        public List<DestinationResponse> destinationResponses { get; set; }

        public class DestinationResponse
        {
            public string address { get; set; }
            public string timeStamp { get; set; }
            public string requestId { get; set; }
            public string statusCode { get; set; }
            public string statusDetail { get; set; }
        }

    }
}