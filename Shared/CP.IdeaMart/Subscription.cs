using CP.Constants;
using CP.Dto;
using CP.Dto.Subscription;
using CP.IdeaMart.Properties;
using CP.Utility;
using Nanosoft.IdeaMartAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CP.IdeaMart
{
    /// <summary>
    /// Subscription service integration
    /// </summary>
    public class Subscription
    {
        static readonly string _baseUrl = ConfigurationManager.AppSettings["baseUrl"];

        #region Subscription APIs
        /// <summary>
        /// Subscribe user from an application.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="password"></param>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public async Task<IdeaMartStatusResponseDto> Add(string applicationId, string password, string subscriberId)
        {
            //Log.TraceStart();
            IdeaMartStatusResponseDto response = null;

            try
            {
                SubscriptionRequestDto subscriptionRequestDto = new SubscriptionRequestDto();
                subscriptionRequestDto.applicationId = applicationId;
                subscriptionRequestDto.password = password;
                subscriptionRequestDto.subscriberId = subscriberId;
                subscriptionRequestDto.action = "1";
                subscriptionRequestDto.version = "1.0";

                var subscriptionResponse = await SubscribeUser(subscriptionRequestDto);

                if (subscriptionResponse.IsSuccessStatusCode)
                {
                    var result = subscriptionResponse.Content.ReadAsAsync<dynamic>().Result;

                    response = new IdeaMartStatusResponseDto();
                    response.requestId = result.requestId;
                    response.statusCode = result.statusCode;
                    response.statusDetail = result.statusDetail;
                    response.timeStamp = DateTime.UtcNow.ToString();
                    response.version = result.version;

                }
                else
                {
                    Log.Error(Resources.Subscription_SubscriptionServiceFailed);
                }

            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }

            //Log.TraceEnd();
            return response;
        }

        /// <summary>
        /// Unsubscribe user from an application.
        /// </summary>
        /// <param name="applicationId"></param>
        /// <param name="password"></param>
        /// <param name="subscriberId"></param>
        /// <returns></returns>
        public async Task<IdeaMartStatusResponseDto> Remove(string applicationId, string password, string subscriberId)
        {
            //Log.TraceStart();
            IdeaMartStatusResponseDto response = null;

            try
            {
                SubscriptionRequestDto subscriptionRequestDto = new SubscriptionRequestDto();
                subscriptionRequestDto.applicationId = applicationId;
                subscriptionRequestDto.password = password;
                subscriptionRequestDto.subscriberId = subscriberId;
                subscriptionRequestDto.action = "0";
                subscriptionRequestDto.version = "1.0";

                var subscriptionResponse = await SubscribeUser(subscriptionRequestDto);

                if (subscriptionResponse.IsSuccessStatusCode)
                {
                    var result = subscriptionResponse.Content.ReadAsAsync<dynamic>().Result;
                    response = new IdeaMartStatusResponseDto();
                    response.requestId = result.requestId;
                    response.statusCode = result.statusCode;
                    response.statusDetail = result.statusDetail;
                    response.timeStamp = DateTime.UtcNow.ToString();
                    response.version = result.version;
                }
                else
                {
                    Log.Error(Resources.Subscription_SubscriptionServiceFailed);
                }

            }
            catch (Exception ex)
            {
                Log.Exception(ex);
            }

            //Log.TraceEnd();
            return response;
        }

        #endregion

        #region private methods

        /// <summary>
        /// This service comprises of registration/ un-registration request and response. 
        /// </summary>
        /// <param name="subscriptionRequestDto">Subscription Data</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> SubscribeUser(SubscriptionRequestDto subscriptionRequestDto)
        {
            try
            {
                //Log.TraceStart();
                var requestUri = string.Format("{0}{1}", _baseUrl, Consts.URL_SUBSCRIPTION_ADD);
                var contentType = InternetMediaType.ApplicationJson;
                var postBody = JsonConvert.SerializeObject(subscriptionRequestDto);
                var content = new StringContent(postBody);

                using (var client = new HttpClient())
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                    //Send HTTP requests
                    //client.BaseAddress = new Uri(Consts.URL_BASE_ADDRESS);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestUri);
                    content.Headers.ContentType = MediaTypeHeaderValue.Parse(contentType);

                    var response = await client.PostAsync(requestUri, content);

                    if (response.IsSuccessStatusCode) return response;

                    var ex = new HttpResponseException(new HttpResponseMessage()
                    {
                        StatusCode = response.StatusCode,
                        Content = response.Content,
                        ReasonPhrase = response.ReasonPhrase,
                        RequestMessage = response.RequestMessage,
                        Version = response.Version,
                    });

                    var fieldInfo = ex.GetType().GetField("_message", BindingFlags.Instance | BindingFlags.NonPublic);
                    if (fieldInfo != null)
                        fieldInfo.SetValue(ex, string.Format("{0}{1}HTTP Response:{1}{2}", ex.Message, Environment.NewLine, response.ToString()));

                    //Log.TraceEnd();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw;
            }

        }

        /// <summary>
        /// Validate server certificates
        /// </summary>
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            //Log.TraceStart();
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            Log.Error(string.Format("Certificate error: {0}", sslPolicyErrors));
            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        #endregion
    }
}
