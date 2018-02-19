using CP.Constants;
using CP.Dto;
using CP.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CP.Utility
{
    public static class WebApiHelper
    {
        #region HTTP GET methods

        /// <summary>
        /// Sends a HTTP GET request to the specified Uri as an asynchronous operation 
        /// and returns the result as a <see cref="Response{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="Response{T}.Data"/>.</typeparam>
        /// <param name="host">The base address for request.</param>
        /// <param name="api">The request Uri</param>
        /// <param name="acceptHeaderMediaType">The accept header type for request.</param>
        /// <param name="httpMessageHandler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="queryStringParams">The query string params.</param>
        /// <param name="customHeaders">Custom headers for the http request.</param>
        /// <param name="authHeader">The authorization header for the request.</param>
        /// <returns>The task object representing  the asynchronous operation.</returns>
        public static async Task<Response<T>> GetAsync<T>(string host, string api,
            string acceptHeaderMediaType = InternetMediaType.ApplicationJson, HttpMessageHandler httpMessageHandler = null,
            KeyValuePair<string, dynamic>[] queryStringParams = null,
            KeyValuePair<string, string>[] customHeaders = null, AuthenticationHeaderValue authHeader = null)
        {
            Response<T> result;
            try
            {
                using (var client = httpMessageHandler == null ? new HttpClient() : new HttpClient(httpMessageHandler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeaderMediaType));
                    client.DefaultRequestHeaders.Authorization = authHeader;
                    AddCustomHeaders(client, customHeaders);

                    var inputParams = string.Empty;
                    if (queryStringParams != null && queryStringParams.Length > 0)
                        inputParams = string.Format("?{0}",
                            string.Join("&", queryStringParams.Select(m => m.Key + "=" + m.Value)));

                    var response = await client.GetAsync(string.Format("{0}{1}{2}", host, api, inputParams));
                    result = await AuthorizeResponse<T>(response);
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                result = GetErrorResponce<T>(StatusCode.Error, "Error occurred while accessing service.");
            }

            return result;
        }


        /// <summary>
        /// Sends a HTTP GET request to the specified Uri as an asynchronous operation 
        /// and returns the result as a <see cref="Response{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="Response{T}.Data"/>.</typeparam>
        /// <param name="host">The base address for request.</param>
        /// <param name="api">The request Uri</param>
        /// <param name="acceptHeaderMediaType">The accept header type for request.</param>
        /// <param name="httpMessageHandler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="queryStringParams">The query string params.</param>
        /// <param name="customHeaders">Custom headers for the http request.</param>
        /// <param name="authHeader">The authorization header for the request.</param>
        /// <returns>The task object representing  the asynchronous operation.</returns>
        public static Response<T> Get<T>(string host, string api,
            string acceptHeaderMediaType = InternetMediaType.ApplicationJson, HttpMessageHandler httpMessageHandler = null,
            KeyValuePair<string, dynamic>[] queryStringParams = null,
            KeyValuePair<string, string>[] customHeaders = null, AuthenticationHeaderValue authHeader = null)
        {
            Response<T> result;
            try
            {
                using (var client = httpMessageHandler == null ? new HttpClient() : new HttpClient(httpMessageHandler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeaderMediaType));
                    client.DefaultRequestHeaders.Authorization = authHeader;
                    AddCustomHeaders(client, customHeaders);

                    var inputParams = string.Empty;
                    if (queryStringParams != null && queryStringParams.Length > 0)
                        inputParams = string.Format("?{0}",
                            string.Join("&", queryStringParams.Select(m => m.Key + "=" + m.Value)));

                    var response = client.GetAsync(string.Format("{0}{1}{2}", host, api, inputParams)).Result;
                    result = AuthorizeResponse<T>(response).Result;
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                result = GetErrorResponce<T>(StatusCode.Error, "Error occurred while accessing service.");
            }

            return result;
        }


        /// <summary>
        /// Sends a HTTP GET request to the specified Uri as an asynchronous operation to load a file. 
        /// </summary>
        /// <param name="baseAddress">The base address for request.</param>
        /// <param name="requestUri">The request Uri</param>
        /// <param name="queryStringParams">The query string params.</param>
        /// <param name="customHeaders">Custom headers which needs to be applied for the http request.</param>
        /// <param name="authHeader">The authorization header for the request.</param>
        /// <returns>The task object representing  the asynchronous operation.</returns>
        public static async Task<Stream> GetFileAsync(string baseAddress, string requestUri,
            KeyValuePair<string, dynamic>[] queryStringParams = null,
            KeyValuePair<string, string>[] customHeaders = null, AuthenticationHeaderValue authHeader = null)
        {
            try
            {
                Stream resultStream;
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = authHeader;
                    AddCustomHeaders(client, customHeaders);
                    var inputParams = String.Empty;
                    if (queryStringParams != null && queryStringParams.Length > 0)
                        inputParams = String.Format("?{0}",
                            String.Join("&", queryStringParams.Select(m => m.Key + "=" + m.Value)));

                    var response =
                        await client.GetAsync(String.Format("{0}{1}{2}", baseAddress, requestUri, inputParams));

                    if (response.IsSuccessStatusCode)
                    {
                        resultStream = await response.Content.ReadAsStreamAsync();
                    }
                    else
                    {
                        var error = await response.Content.ReadAsStringAsync();
                        throw new Exception(error);
                    }
                }

                return resultStream;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                throw;
            }

        }

        /// <summary>
        /// Sends a HTTP Get request to the specified Uri as an asynchronous operation 
        /// and returns the result as a <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="host">The base address for request.</param>
        /// <param name="api">The request Uri</param>
        /// <param name="acceptHeaderMediaType">The accept header type for request.</param>
        /// <param name="httpMessageHandler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="queryStringParams">The query string params.</param>
        /// <param name="customHeaders">Custom headers for the http request.</param>
        /// <param name="authHeader">The authorization header for the request.</param>
        /// <returns>The task object representing  the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> GetAsync(string host, string api,
            string acceptHeaderMediaType = InternetMediaType.ApplicationJson,
            HttpMessageHandler httpMessageHandler = null,
            KeyValuePair<string, dynamic>[] queryStringParams = null,
            KeyValuePair<string, string>[] customHeaders = null,
            AuthenticationHeaderValue authHeader = null)
        {
            using (var client = httpMessageHandler == null ? new HttpClient() : new HttpClient(httpMessageHandler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeaderMediaType));
                client.DefaultRequestHeaders.Authorization = authHeader;
                AddCustomHeaders(client, customHeaders);

                var queryString = string.Empty;
                if (queryStringParams != null && queryStringParams.Length > 0)
                    queryString = string.Format("?{0}",
                        string.Join("&", queryStringParams.Select(m => m.Key + "=" + m.Value)));

                var response = await client.GetAsync(string.Format("{0}{1}{2}", host, api, queryString));

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

                throw ex;
            }
        }

        #endregion

        #region HTTP POST methods

        /// <summary>
        /// Sends a HTTP POST request to the specified Uri as an asynchronous operation 
        /// and returns the result as a <see cref="Response{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="Response{T}.Data"/>.</typeparam>
        /// <param name="host">The base address for request.</param>
        /// <param name="api">The request Uri</param>
        /// <param name="requestContent">The request content which should be sent to the server.</param>
        /// <param name="acceptHeaderMediaType">The accept header type for request.</param>
        /// <param name="httpMessageHandler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="customHeaders">Custom headers for the http request.</param>
        /// <param name="authHeader">The authorization header for the request.</param>
        /// <returns>The task object representing  the asynchronous operation.</returns>
        public static async Task<Response<T>> PostAsync<T>(string host, string api, dynamic requestContent,
            string acceptHeaderMediaType = InternetMediaType.ApplicationJson, HttpMessageHandler httpMessageHandler = null,
            KeyValuePair<string, string>[] customHeaders = null, AuthenticationHeaderValue authHeader = null)
        {
            Response<T> result;
            try
            {
                using (var client = httpMessageHandler == null ? new HttpClient() : new HttpClient(httpMessageHandler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeaderMediaType));
                    client.DefaultRequestHeaders.Authorization = authHeader;
                    AddCustomHeaders(client, customHeaders);

                    var postBody = JsonConvert.SerializeObject(requestContent);
                    var response =
                        await
                            client.PostAsync(string.Format("{0}{1}", host, api),
                                new StringContent(postBody, Encoding.Default, acceptHeaderMediaType));
                                //new StringContent(postBody));
                    result = await AuthorizeResponse<T>(response);
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                result = GetErrorResponce<T>(StatusCode.Error, "Error occurred while accessing service.");
            }
            return result;
        }

        /// <summary>
        /// Sends a HTTP POST request to the specified Uri with multipart/form-data as an asynchronous operation 
        /// and returns the result as a <see cref="Response{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="Response{T}.Data"/>.</typeparam>
        /// <param name="baseAddress">The base address for request.</param>
        /// <param name="requestUri">The request Uri</param>
        /// <param name="formDataContent">Form data content (i.e. multipart/form-data) which should be send to the server as the payload.</param>
        /// <param name="internetMediaType">The accept header type for request.</param>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="customHeaders">Custom headers for the http request.</param>
        /// <param name="authHeader">The authorization header for the request.</param>
        /// <returns>The task object representing  the asynchronous operation.</returns>
        /// <remarks>This method is implemented to call some specific services which required  multipart/form-data.</remarks>
        public static async Task<Response<T>> PostMultiPartFormDataAsync<T>(string baseAddress, string requestUri, MultipartFormDataContent formDataContent,
            string internetMediaType = InternetMediaType.ApplicationJson, HttpMessageHandler handler = null,
            KeyValuePair<string, string>[] customHeaders = null, AuthenticationHeaderValue authHeader = null)
        {
            Response<T> result;
            try
            {
                using (var client = handler == null ? new HttpClient() : new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(internetMediaType));
                    client.DefaultRequestHeaders.Authorization = authHeader;
                    AddCustomHeaders(client, customHeaders);
                    var response =
                        await client.PostAsync(string.Format("{0}{1}", baseAddress, requestUri), formDataContent);
                    result = await AuthorizeResponse<T>(response);

                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                result = GetErrorResponce<T>(StatusCode.Error, "Error occurred while accessing service.");
            }
            return result;
        }


        public static Response<T> PostMultiPartFormData<T>(string baseAddress, string requestUri, MultipartFormDataContent formDataContent,
          string internetMediaType = InternetMediaType.ApplicationJson, HttpMessageHandler handler = null,
          KeyValuePair<string, string>[] customHeaders = null, AuthenticationHeaderValue authHeader = null)
        {
            Response<T> result;
            try
            {
                using (var client = handler == null ? new HttpClient() : new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(internetMediaType));
                    client.DefaultRequestHeaders.Authorization = authHeader;
                    AddCustomHeaders(client, customHeaders);
                    var response =
                         client.PostAsync(string.Format("{0}{1}", baseAddress, requestUri), formDataContent).Result;
                    result = AuthorizeResponse<T>(response).Result;

                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                result = GetErrorResponce<T>(StatusCode.Error, "Error occurred while accessing service.");
            }
            return result;
        }


        /// <summary>
        /// Sends a HTTP POST request to the specified Uri as an asynchronous operation 
        /// and returns the result as a <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="host">The base address for request.</param>
        /// <param name="api">The request Uri</param>
        /// <param name="requestContent">The request content which should be sent to the server.</param>
        /// <param name="acceptHeaderMediaType">The accept header type for request.</param>
        /// <param name="httpMessageHandler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="customHeaders">Custom headers for the http request.</param>
        /// <param name="authHeader">The authorization header for the request.</param>
        /// <param name="queryStringParams">The query string params.</param>
        /// <returns>The task object representing  the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> PostAsync(string host, string api,
            dynamic requestContent = null,
            string acceptHeaderMediaType = InternetMediaType.ApplicationJson,
            HttpMessageHandler httpMessageHandler = null,
            KeyValuePair<string, dynamic>[] queryStringParams = null,
            KeyValuePair<string, string>[] customHeaders = null,
            AuthenticationHeaderValue authHeader = null)
        {
            using (var client = httpMessageHandler == null ? new HttpClient() : new HttpClient(httpMessageHandler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(acceptHeaderMediaType));
                client.DefaultRequestHeaders.Authorization = authHeader;
                AddCustomHeaders(client, customHeaders);

                var queryString = string.Empty;
                if (queryStringParams != null && queryStringParams.Length > 0)
                    queryString = string.Format("?{0}", string.Join("&", queryStringParams.Select(m => m.Key + "=" + m.Value)));

                var response = await client.PostAsync(string.Format("{0}{1}{2}", host, api, queryString),
                    new StringContent(JsonConvert.SerializeObject(requestContent), Encoding.UTF8, InternetMediaType.ApplicationJson));

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

                throw ex;
            }
        }

        #endregion

        #region HTTP PUT methods

        /// <summary>
        /// Sends a HTTP PUT request to the specified Uri as an asynchronous operation 
        /// and returns the result as a <see cref="Response{T}"/>.
        /// </summary>
        /// <typeparam name="T">Type of the <see cref="Response{T}.Data"/>.</typeparam>
        /// <param name="baseAddress">The base address for request.</param>
        /// <param name="requestUri">The request Uri</param>
        /// <param name="requestContent">The request content which should be sent to the server.</param>
        /// <param name="internetMediaType">The accept header type for request.</param>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="customHeaders">Custom headers for the http request.</param>
        /// <param name="authHeader">The authorization header for the request.</param>
        /// <returns>The task object representing  the asynchronous operation.</returns>
        public static async Task<Response<T>> PutAsync<T>(string baseAddress, string requestUri, dynamic requestContent,
            string internetMediaType = InternetMediaType.ApplicationJson, HttpMessageHandler handler = null,
            KeyValuePair<string, string>[] customHeaders = null, AuthenticationHeaderValue authHeader = null)
        {
            Response<T> result;
            try
            {
                using (var client = handler == null ? new HttpClient() : new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(internetMediaType));
                    client.DefaultRequestHeaders.Authorization = authHeader;
                    AddCustomHeaders(client, customHeaders);

                    var postBody = JsonConvert.SerializeObject(requestContent);
                    var response =
                        await
                            client.PutAsync(string.Format("{0}{1}", baseAddress, requestUri),
                                new StringContent(postBody, Encoding.UTF8, internetMediaType));
                    result = await AuthorizeResponse<T>(response);
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                result = GetErrorResponce<T>(StatusCode.Error, "Error occurred while accessing service.");
            }
            return result;
        }



        /// <summary>
        /// Sends a HTTP PUT request to the specified Uri as an asynchronous operation 
        /// and returns the result as a <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="baseAddress">The base address for request.</param>
        /// <param name="requestUri">The request Uri</param>
        /// <param name="requestContent">The request content which should be sent to the server.</param>
        /// <param name="internetMediaType">The accept header type for request.</param>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="customHeaders">Custom headers for the http request.</param>
        /// <param name="authHeader">The authorization header for the request.</param>
        /// <returns>The task object representing  the asynchronous operation.</returns>
        public static async Task<HttpResponseMessage> PutAsync(string baseAddress, string requestUri,
            dynamic requestContent, string internetMediaType = InternetMediaType.ApplicationJson,
            HttpMessageHandler handler = null,
            KeyValuePair<string, string>[] customHeaders = null, AuthenticationHeaderValue authHeader = null)
        {
            using (var client = handler == null ? new HttpClient() : new HttpClient(handler))
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(internetMediaType));
                client.DefaultRequestHeaders.Authorization = authHeader;
                AddCustomHeaders(client, customHeaders);
                var postBody = JsonConvert.SerializeObject(requestContent);

                return
                    await
                        client.PostAsync(string.Format("{0}{1}", baseAddress, requestUri),
                            new StringContent(postBody, Encoding.UTF8, InternetMediaType.ApplicationJson));
            }
        }

        /// <summary>
        /// Sends a HTTP DELETE request to the specified Uri as an asynchronous operation 
        /// and returns the result as a <see cref="HttpResponseMessage"/>.
        /// </summary>
        /// <param name="baseAddress">The base address for request.</param>
        /// <param name="requestUri">The request Uri</param>
        /// <param name="internetMediaType">The accept header type for request.</param>
        /// <param name="queryStringParams">The query string params.</param>
        /// <param name="handler">The HTTP handler stack to use for sending requests.</param>
        /// <param name="customHeaders">Custom headers for the http request.</param>
        /// <param name="authHeader">The authorization header for the request.</param>
        /// <returns>The task object representing  the asynchronous operation.</returns>
        public static async Task<Response<T>> DeleteAsync<T>(string baseAddress, string requestUri,
            string internetMediaType = InternetMediaType.ApplicationJson, KeyValuePair<string, dynamic>[] queryStringParams = null,
            HttpMessageHandler handler = null,
            KeyValuePair<string, string>[] customHeaders = null, AuthenticationHeaderValue authHeader = null)
        {
            Response<T> result;
            try
            {
                using (var client = handler == null ? new HttpClient() : new HttpClient(handler))
                {
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(internetMediaType));
                    client.DefaultRequestHeaders.Authorization = authHeader;
                    AddCustomHeaders(client, customHeaders);

                    var inputParams = string.Empty;
                    if (queryStringParams != null && queryStringParams.Length > 0)
                        inputParams = string.Format("?{0}",
                            string.Join("&", queryStringParams.Select(m => m.Key + "=" + m.Value)));

                    var response =
                        await client.DeleteAsync(string.Format("{0}{1}{2}", baseAddress, requestUri, inputParams));
                    result = await response.Content.ReadAsAsync<Response<T>>();
                }
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                result = GetErrorResponce<T>(StatusCode.Error, "Error occurred while accessing service.");
            }

            return result;
        }

        #endregion

        #region private methods
        private static void AddCustomHeaders(HttpClient client, KeyValuePair<string, string>[] customHaders)
        {
            if (client != null && customHaders != null)
            {
                foreach (var customHadersList in customHaders)
                {
                    client.DefaultRequestHeaders.Add(customHadersList.Key, customHadersList.Value);
                }
            }
        }

        /// <summary>
        /// Check whether the response is authorized and prepare the return accordingly.
        /// </summary>
        /// <typeparam name="T">Dto type</typeparam>
        /// <param name="response">API Response</param>
        /// <returns>The task object representing  the asynchronous operation.</returns>
        private static async Task<Response<T>> AuthorizeResponse<T>(HttpResponseMessage response)
        {
            bool isAuthorizedResponse = true;
            StatusCode authorizeStatusCode = StatusCode.Success;

            if (response.Headers.Contains("AuthenticationStatus"))
            {
                string responseAuthHeader = Convert.ToString(response.Headers.GetValues("AuthenticationStatus").FirstOrDefault());
                isAuthorizedResponse = !string.IsNullOrEmpty(responseAuthHeader) && responseAuthHeader == "Authorized";

                if (isAuthorizedResponse == false)
                    authorizeStatusCode = StatusCode.Unauthorized;
            }
            //else if (response.Headers.Contains("RegisterStatus"))
            //{
            //    isAuthorizedResponse = false;
            //    authorizeStatusCode = StatusCode.ProfileStatusIncomplete;
            //}

            return isAuthorizedResponse ? await response.Content.ReadAsAsync<Response<T>>() : GetErrorResponce<T>(authorizeStatusCode, "Unauthorized");
        }

        public static Response<T> GetErrorResponce<T>(StatusCode status, string message)
        {
            Log.TraceExecute(String.Format("Error response. Status Code: {0}, Response message: {1}", status, message));
            return new Response<T>
            {
                Message = message,
                StatusCode = status
            };
        }


        #endregion
    }
}
