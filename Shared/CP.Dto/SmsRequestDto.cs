using System.Collections.Generic;

namespace CP.Dto
{
    public class SmsRequestDto
    {
        public string Message { get; set; }
        public List<string> DestinationAddresses { get; set; }
        public string Password { get; set; }
        public string ApplicationId { get; set; }
        public string SourceAddress { get; set; }
        public string DeliveryStatusRequest { get; set; }
        public string ChargingAmount { get; set; }
        public string Encoding { get; set; }
        public string Version { get; set; }
        public string BinaryHeader { get; set; }


    }
}
