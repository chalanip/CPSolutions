using CP.Constants;
using CP.Dto;
using CP.Dto.Subscription;
using CP.Utility;
using CPServices.Properties;
using Nanosoft.IdeaMartAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Threading.Tasks;
using System.Web.Http;

namespace CPServices.Controllers
{
    /// <summary>
    /// Subscription related APIs
    /// </summary>
    public class SubscriptionController : ApiController
    {
        [HttpPost]
        //public async Task<Response<SubscriptionResponseDto>> Add(string applicationId, string password, string subscriberId, bool action)
        public async Task<Response<SubscriptionResponseDto>> Add(SubscriptionRequestDto subscriptionRequestDto)
        {
            Log.TraceStart();
            Response<SubscriptionResponseDto> response = null;
            try
            {
                //SubscriptionRequestDto subscriptionRequestDto = new SubscriptionRequestDto();
                //subscriptionRequestDto.applicationId = applicationId;
                //subscriptionRequestDto.password = password;
                //subscriptionRequestDto.subscriberId = subscriberId;
                //subscriptionRequestDto.action = (action) ? "1" : "0";
                //subscriptionRequestDto.version = "1.0";

                var subscriptionResponse = await SubscribeUser(subscriptionRequestDto);

                if (subscriptionResponse.IsSuccessStatusCode)
                {
                    var result = subscriptionResponse.Content.ReadAsAsync<dynamic>().Result;
                    SubscriptionResponseDto subscriptionResponseDto = new SubscriptionResponseDto();
                    //subscriptionResponseDto = MapToSubscriptionResponseDto(result);

                    response = new Response<SubscriptionResponseDto>(CP.Enum.StatusCode.Success, Resources.Success, subscriptionResponseDto);
                }
                else
                {
                    return WebApiHelper.GetErrorResponce<SubscriptionResponseDto>(CP.Enum.StatusCode.SubscriptionServiceFailed, Resources.Subscription_SubscriptionServiceFailed);
                }
                Log.TraceEnd();
                return response;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return WebApiHelper.GetErrorResponce<SubscriptionResponseDto>(CP.Enum.StatusCode.Error, Resources.Error_ServiceAccessFailed);
            }


        }

        private Task<HttpResponseMessage> SubscribeUser(SubscriptionRequestDto subscriptionRequestDto)
        {
            Log.TraceStart();
            using (var client = new HttpClient())
            {
                //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);
                string requestId = Guid.NewGuid().ToString();

                //Send HTTP requests
                client.BaseAddress = new Uri(Consts.URL_BASE_ADDRESS);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var request = new HttpRequestMessage(HttpMethod.Get, new Uri(Consts.URL_BASE_ADDRESS + Consts.URL_SUBSCRIPTION_ADD));

                var postBody = JsonConvert.SerializeObject(subscriptionRequestDto);
                request.Content = new StringContent(postBody);
                var response = client.SendAsync(request);

                Log.TraceEnd();
                return response;
            }

        }

        /// <summary>
        /// Get location
        /// </summary>
        [HttpPost]
        //public Response<SubscriptionResponseDto> AddSubscription(string Id)
       public void AddSubscription(string Id)
        {

            ////Response<LbsResponseDto> response = null;
            //var appId = "App_000001";
            //var pwd = "password";
            ////UssdAPI ussdApi = new UssdAPI(appId, pwd);
            ////CaasAPI ca = new CaasAPI(appId, pwd);

            //try
            //{
            //    SubscriptionRequestDto subscriptionRequestDto = new SubscriptionRequestDto();
            //    subscriptionRequestDto.applicationId = appId;//mandatory 
            //    subscriptionRequestDto.password = pwd;//mandatory 
            //    subscriptionRequestDto.subscriberId = Id;//mandatory 
            //    subscriptionRequestDto.action = "1";//mandatory 
            //    //lbsRequestDto.ResponseTime = "NO_DELAY";
            //    //lbsRequestDto.Freshness = "HIGH";
            //    //lbsRequestDto.HorizontalAccuracy = "1500";
            //    subscriptionRequestDto.version = "2.0";


            //    var response = WebApiHelper.PostAsync<SubscriptionResponseDto>
            //        (Consts.URL_BASE_ADDRESS, Consts.URL_SUBSCRIPTION_ADD, subscriptionRequestDto);

            //    return response.Result;
            //}
            //catch (Exception ex)
            //{
            //    Log.Exception(ex);
            //    throw;
            //    //result = GetErrorResponce<T>(StatusCode.Error, "Error occurred while accessing service.");
            //}


        }
    }
}
