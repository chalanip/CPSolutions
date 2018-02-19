using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPServices.api
{
    /// <summary>
    /// Summary description for MoSmsReq
    /// </summary>
    public class MoSmsReq : Message
    {
        public String version { get; set; }
        public String applicationID { get; set; }
        public String sourceAddress { get; set; }
        public String message { get; set; }
        public String requestID { get; set; }
        public String encoding { get; set; }
        public String deliveryStatusRequest { get; set; }
    }
}