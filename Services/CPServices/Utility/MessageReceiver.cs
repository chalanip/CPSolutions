using CPServices.api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPServices.Utility
{
    /// <summary>
    /// Summary description for MessageReceiver
    /// </summary>
    public class MessageReceiver : SmsListener
    {
        protected override void onMessage(MoSmsReq moSmsReq)
        {
           
            MtSmsResp mtSmsResp = null;
            try
            {
                //SmsSender smsSender = new SmsSender("http://localhost:5556/mo-receiver");// ("http://127.0.0.1:7007/service");

                //List<String> address = new List<String>();
                //address.Add(moSmsReq.sourceAddress);

                //MtSmsReq mtSmsReq = new MtSmsReq();
                //mtSmsReq.applicationId = moSmsReq.applicationID;
                //mtSmsReq.password = "password";
                //mtSmsReq.destinationAddress = address;
                //mtSmsReq.message = "Message Received. Thanks you.";

                //mtSmsResp = smsSender.SendSMSReq(mtSmsReq);

            }
            catch (SdpException ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

        }
    }
}