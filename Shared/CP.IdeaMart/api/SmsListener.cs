using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace CP.IdeaMart.api
{
    /// <summary>
    /// Summary description for SmsListener
    /// </summary>
    public abstract class SmsListener : IHttpHandler
    {
        private readonly String APPLICATION_RUNING_MESSAGE = "SMS Application Runing";
        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            MoSmsResp moSmsResp = null;
            string jsonString = "";
            context.Response.ContentType = "application/json";
            try
            {
                byte[] PostData = context.Request.BinaryRead(context.Request.ContentLength);
                jsonString = Encoding.UTF8.GetString(PostData);
                JavaScriptSerializer json_serializer = new JavaScriptSerializer();
                MoSmsReq moSmsReq = json_serializer.Deserialize<MoSmsReq>(jsonString);
                moSmsResp = GenerateStatus(true);
                onMessage(moSmsReq);
            }
            catch (Exception)
            {
                moSmsResp = GenerateStatus(false);
            }
            finally
            {
                if (jsonString.Equals(""))
                    context.Response.Write(APPLICATION_RUNING_MESSAGE);
                else
                    context.Response.Write(moSmsResp.ToString());
            }
        }

        public MoSmsResp GenerateStatus(bool Sucess)
        {
            if (Sucess)
                return new MoSmsResp(EnumStatusCode.S1000);
            return new MoSmsResp(EnumStatusCode.E1312);
        }

        protected abstract void onMessage(MoSmsReq moSmsReq);

    }
}