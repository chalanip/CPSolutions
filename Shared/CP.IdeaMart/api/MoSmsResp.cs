using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP.IdeaMart.api
{
    /// <summary>
    /// Status code
    /// </summary>
    public enum EnumStatusCode { S1000, E1312 };

    /// <summary>
    /// Summary description for MoSmsResp
    /// </summary>
    public class MoSmsResp : Message
    {

        public String statusCode { get; set; }
        public String statusDetail { get; set; }

        private static readonly String S1000_STATUS = "Process completed successfully",
                                        E1312_STATUS = "Request is Invalid. " +
                                                        "Refer the Idea Mart NBL API Developer Guide for " +
                                                        "the mandatory fields and correct format of the request.";

        public MoSmsResp(EnumStatusCode enumStatusCode)
        {
            statusCode = (enumStatusCode.Equals(EnumStatusCode.S1000)) ? "S1000" : "E1312";
            statusDetail = (enumStatusCode.Equals(EnumStatusCode.S1000)) ? S1000_STATUS : E1312_STATUS;
        }

    }
}