using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Script.Serialization;

namespace CPServices.api
{
    /// <summary>
    /// Summary description for SmsSender
    /// </summary>
    public class SmsSender
    {
        public String url { get; set; }

        public SmsSender(String url)
        {
            this.url = url;
        }

        public MtSmsResp SendSMSReq(MtSmsReq mtSmsReq)
        {
            StreamWriter requestWriter;
            MtSmsResp mtSmsResp = null;

            try
            {
                var webRequest = System.Net.WebRequest.Create(url) as HttpWebRequest;

                if (webRequest != null)
                {
                    webRequest.Method = "POST";
                    webRequest.ContentType = "application/json";
                    using (requestWriter = new StreamWriter(webRequest.GetRequestStream()))
                    {
                        requestWriter.Write(mtSmsReq.ToString());
                    }
                }

                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                Stream responseStream = response.GetResponseStream();
                StreamReader responseReader = new StreamReader(responseStream);
                String jsonResponseString = responseReader.ReadToEnd();

                JavaScriptSerializer javascriptserializer = new JavaScriptSerializer();
                mtSmsResp = javascriptserializer.Deserialize<MtSmsResp>(jsonResponseString);
            }
            catch (Exception ex)
            {
                throw new SdpException(ex);
            }
            return mtSmsResp;
        }

    }
}