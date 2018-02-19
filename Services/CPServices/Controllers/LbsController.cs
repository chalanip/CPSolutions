using CP.Constants;
using CP.Dto;
using CP.Utility;
using Nanosoft.IdeaMartAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace CPServices.Controllers
{
    /// <summary>
    /// Lbs (Location Based Service) related APIs
    /// </summary>
    public class LbsController : ApiController
    {
        /// <summary>
        /// Get location
        /// </summary>
        [HttpPost]
        public async Task<Response<LbsResponseDto>> GetLocation(LbsRequestDto lbsRequestDto)
        {

            //Response<LbsResponseDto> response = null;
            //var appId = "App_000001";
            //var pwd = "password";
            
            try
            {
                //LbsRequestDto lbsRequestDto = new LbsRequestDto();
                //lbsRequestDto.applicationId = appId;//mandatory 
                //lbsRequestDto.password = pwd;//mandatory 
                //lbsRequestDto.subscriberId = "tel:94771122336";//mandatory 
                //lbsRequestDto.serviceType = "IMMEDIATE";//mandatory 
                //lbsRequestDto.ResponseTime = "NO_DELAY";
                //lbsRequestDto.Freshness = "HIGH";
                //lbsRequestDto.HorizontalAccuracy = "1500";
                //lbsRequestDto.Vesrion = "2.0";
                string acceptHeaderMediaType = InternetMediaType.ApplicationJson;
                var client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:7000/lbs/locate");

                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
              

                var postBody = JsonConvert.SerializeObject(lbsRequestDto);
                    var response =
                        await
                            client.PostAsync(client.BaseAddress, new StringContent(postBody, Encoding.UTF8, "application/json"));
                var result = await response.Content.ReadAsAsync<Response<LbsResponseDto>>();//await AuthorizeResponse<T>(response);


                //var response = WebApiHelper.PostAsync<LbsResponseDto>
                //    (Consts.URL_BASE_ADDRESS, Consts.URL_LBS_GET_LOCATION, lbsRequestDto);
                return result;


            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw;
                //result = GetErrorResponce<T>(StatusCode.Error, "Error occurred while accessing service.");
            }

           
        }
    }
}
