using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CPServices.api
{
    /// <summary>
    /// Summary description for SdpException
    /// </summary>
    public class SdpException : Exception
    {
        public SdpException(Exception ex) : base(ex.Message) { }
    }
}