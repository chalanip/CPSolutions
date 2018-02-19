using CP.Constants;
using CP.Utility;
using CPServices.api;
using CPServices.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace CPServices.Utility
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
            bool status = false;

            MtSmsReq mtSmsReq = new MtSmsReq();
            mtSmsReq.applicationId = _appId;
            mtSmsReq.password = _password;
            mtSmsReq.destinationAddress = userAddress;
            mtSmsReq.message = message;

            SmsSender smsSender = new SmsSender(string.Concat(_baseUrl, Consts.URL_SMS_SEND));
            MtSmsResp mtSmsResp = smsSender.SendSMSReq(mtSmsReq);

            if (mtSmsResp.statusCode == "SUCCESS")
                status = true;
            else
                Log.Error(string.Format(Resources.Error_SendSmsFailed, userAddress));

            return status;
        }
    }
}