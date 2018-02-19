using CP.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CP.Dto
{
    /// <summary>
    /// Contains the details of a response object.
    /// </summary>
    public class Response<T>
    {
        public Response() { }

        public Response(StatusCode StatusCode, string Message, T Data)
        {
            this.StatusCode = StatusCode;
            this.Message = Message;
            this.Data = Data;
        }

        /// <summary>
        /// Gets or Sets StatusCode of the Response.
        /// </summary>
        public StatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or Sets StatusMessage of the Response.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or Sets Result of the Response.
        /// </summary>
        public T Data { get; set; }

    }
}
