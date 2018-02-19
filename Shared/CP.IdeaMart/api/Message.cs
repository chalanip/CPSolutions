using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace CP.IdeaMart.api
{
    /// <summary>
    /// Summary description for Message
    /// </summary>
    public class Message
    {
        public override String ToString()
        {
            String jsonString = null;
            try
            {
                jsonString = new JavaScriptSerializer().Serialize(this);
            }
            catch (Exception)
            {
                jsonString = null;
            }
            return jsonString;
        }
    }
}