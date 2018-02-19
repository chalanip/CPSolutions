using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CP.IdeaMart.api
{
    /// <summary>
    /// Summary description for MtSmsReq
    /// </summary>
    public class MtSmsReq : Message
    {
        public string applicationId { get; set; }
        public string password { get; set; }
        public string version { get; set; }
        //public List<String> destinationAddresses { get; set; }
        public string[] destinationAddresses { get; set; }
        public string message { get; set; }
        public string sourceAddress { get; set; }
        public string deliveryStatusRequest { get; set; }
        public string encoding { get; set; }
        public string chargingAmount { get; set; }
        public string binaryHeader { get; set; }
    }
}