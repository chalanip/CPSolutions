using CP.Constants;
using CP.Dto;
using CP.Dto.Caas;
using CP.Enum;
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
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CPServices.Utility
{
    /// <summary>
    /// Charging as a service integration
    /// </summary>
    public class Caas
    {
        static readonly string _baseUrl = "http://localhost:7000/";// ConfigurationManager.AppSettings["baseUrl"];

        /// <summary>
        /// This service charges a specific amount from a subscriber’s account.
        /// </summary>
        /// <param name="applicationId">Used to identify the application. </param>
        /// <param name="password">Used to authenticate the application originated message against the service providers credentials.Encoded, single value</param>
        /// <param name="subscriberId">This can be the MSISDN the subscriber to be charged. This is a unique identifier.</param>
        /// <returns></returns>
        public async Task<DirectDebitResponseDto> Charge(string applicationId, string password, string subscriberId)
        {
            Log.TraceStart();
            DirectDebitResponseDto response = null;
            try
            {
                DirectDebitRequestDto ddRequestDto = new DirectDebitRequestDto();
                ddRequestDto.applicationId = applicationId;
                ddRequestDto.password = password;
                ddRequestDto.subscriberId = subscriberId;
                ddRequestDto.externalTrxId = Guid.NewGuid().ToString();
                //ddRequestDto.paymentInstrumentName = "MobileAccount";
                ddRequestDto.amount = "2";

                var ddResponse = await DirectDebit(ddRequestDto);

                if (ddResponse.IsSuccessStatusCode)
                {
                    var result = ddResponse.Content.ReadAsAsync<dynamic>().Result;

                    //response = new IdeaMartStatusResponse();
                    //response.requestId = result.requestId;
                    //response.statusCode = result.statusCode;
                    //response.statusDetail = result.statusDetail;
                    //response.timeStamp = DateTime.UtcNow.ToString();
                    //response.version = result.version;

                   // response = new DirectDebitResponseDto();
                    response = MapToDirectDebitResponseDto(result);

                    if (ddRequestDto.externalTrxId != response.externalTrxId)
                    {
                        Log.Error("External Transaction Id mimatch");

                    }
                    //    return WebApiHelper.GetErrorResponce<DirectDebitResponseDto>(StatusCode.CaasServiceFailed, Resources.Caas_ExternalTransactionIdMismatch);

                    //response = new Response<DirectDebitResponseDto>(StatusCode.Success, Resources.Success, ddResponseDto);
                }
                else
                {
                    Log.Error(Resources.Caas_CassServiceFailed);
                }
                Log.TraceEnd();
                return response;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw;
                //return WebApiHelper.GetErrorResponce<DirectDebitResponseDto>(StatusCode.Error, Resources.Error_ServiceAccessFailed);
            }


        }
        private async Task<HttpResponseMessage> DirectDebit(DirectDebitRequestDto ddRequestDto)
        {
            try
            {
                Log.TraceStart();
                var requestUri = string.Format("{0}{1}", _baseUrl, Consts.URL_CAAS_DIRECT_DEBIT);
                var contentType = InternetMediaType.ApplicationJson;
                var postBody = JsonConvert.SerializeObject(ddRequestDto);
                var content = new StringContent(postBody);

                using (var client = new HttpClient())
                {
                    ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(ValidateServerCertificate);

                    //Send HTTP requests
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

                    Log.TraceEnd();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw;
            }

        }

        private DirectDebitResponseDto MapToDirectDebitResponseDto(dynamic response)
        {
            Log.TraceStart();

            DirectDebitResponseDto ddResponseDto = new DirectDebitResponseDto();
            ddResponseDto.statusCode = response.statusCode;
            ddResponseDto.timeStamp = response.timeStamp;
            ddResponseDto.shortDescription = response.shortDescription;
            ddResponseDto.statusDetail = response.statusDetail;
            ddResponseDto.externalTrxId = response.externalTrxId;
            ddResponseDto.longDescription = response.longDescription;
            ddResponseDto.internalTrxId = response.internalTrxId;

            Log.TraceEnd();
            return ddResponseDto;
        }

        /// <summary>
        /// Validate server certificates
        /// </summary>
        private bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            Log.TraceStart();
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                Log.TraceEnd();
                return true;
            }

            Log.Error(string.Format("Certificate error: {0}", sslPolicyErrors));
            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

    }
}