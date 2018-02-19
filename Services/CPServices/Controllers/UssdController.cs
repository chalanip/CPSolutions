using CP.Constants;
using CP.Dto;
using CP.Utility;
using Nanosoft.IdeaMartAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace CPServices.Controllers
{
    /// <summary>
    /// Manage data related to USSD
    /// </summary>
    public class UssdController : ApiController
    {
        /// <summary>
        /// Send ussd based messages
        /// </summary>
        /// <param name="ideaMartResponse"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IHttpActionResult> Send(IdeaMartResponse ideaMartResponse)
        {
            //SIMULATOR Application Data: 
            //URL: http://localhost:2513/api/v1/Ussd/Send

            var appId = "App_000001";
            var pwd = "password";
            UssdAPI ussdApi = new UssdAPI(appId, pwd);

            ussdApi.IsInProduction = false;
           
            if (ideaMartResponse.ussdOperation == "mo-init")
            {
                IdeaMartStatusResponse statusResponse = new IdeaMartStatusResponse();
                statusResponse = await ussdApi.SendRequestAsync("My first app. \r\n 1.Press one to OK \r\n 2. Press two to cancel"
                    , ideaMartResponse.sessionId, UssdAPI.UssdOperation.mt_cont, ideaMartResponse.sourceAddress);
                return Ok(statusResponse);
            }
            else if (ideaMartResponse.ussdOperation == "mo-cont" && ideaMartResponse.message == "1")
            {
                IdeaMartStatusResponse statusResponse = new IdeaMartStatusResponse();
                statusResponse = await ussdApi.SendRequestAsync("You Pressed 1."
                    , ideaMartResponse.sessionId, UssdAPI.UssdOperation.mt_cont, ideaMartResponse.sourceAddress);
                return Ok(statusResponse);
            }
            else if (ideaMartResponse.ussdOperation == "mo-cont" && ideaMartResponse.message == "2")
            {
                IdeaMartStatusResponse statusResponse = new IdeaMartStatusResponse();
                statusResponse = await ussdApi.SendRequestAsync("You Pressed 2."
                    , ideaMartResponse.sessionId, UssdAPI.UssdOperation.mt_cont, ideaMartResponse.sourceAddress);
                return Ok(statusResponse);
            }
            return BadRequest();
        }

        
    }
}
