using CP.Constants;
using CP.IdeaMart.api;
using CP.IdeaMart.Properties;
using CP.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.IdeaMart
{
    /// <summary>
    /// Summary description for MessageSender
    /// </summary>
    public static class Sms
    {
        static readonly string _appId = ConfigurationManager.AppSettings["appId"];
        static readonly string _password = ConfigurationManager.AppSettings["password"];
        static readonly string _baseUrl = ConfigurationManager.AppSettings["baseUrl"];

        /// <summary>
        /// Send sms to user
        /// </summary>
        /// <param name="userAddress">Address of the message recevier</param>
        /// <param name="message">Text message</param>
        /// <returns>True if message send successful otherwise false.</returns>
        public static bool Send(string userAddress, string message)
        {
            try
            {                
                MtSmsReq mtSmsReq = new MtSmsReq();
                mtSmsReq.applicationId = _appId;
                mtSmsReq.password = _password;
                mtSmsReq.destinationAddresses = new string[] { userAddress };
                mtSmsReq.message = message;
                mtSmsReq.version = "1.0";

                //Log.Data("MtSmsReq ", mtSmsReq);

                SmsSender smsSender = new SmsSender(string.Concat(_baseUrl, Consts.URL_SMS_SEND));
                MtSmsResp mtSmsResp = smsSender.SendSMSReq(mtSmsReq);
                
                return true;
            }
            catch (SdpException ex)
            {
                Log.Error(string.Format(Resources.Error_SendSmsFailed, userAddress));
                Log.Exception(ex);
                return false;
            }
        }
    }
}
