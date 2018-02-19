using CP.Dto;
using CP.Utility;
using CPStatusCode = CP.Enum.StatusCode;
using CPServices.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CP.Data;
using CP.Enum;
using System.Data;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Nanosoft.IdeaMartAPI;

namespace CPServices.Controllers
{
    public class UserController : ApiController
    {
        /// <summary>
        /// Add user details into the system
        /// </summary>
        /// <param name="userDto">User details</param>
        /// <returns>User Id created in the system</returns>
        [HttpPost]
        public Response<int> AddUser(AddUserDto userDto)
        {
            Log.TraceStart();
            Response<int> response = null;
            try
            {
                //Validate data
                if (userDto == null)
                    return WebApiHelper.GetErrorResponce<int>(CPStatusCode.InvalidInputData, Resources.Error_Invalid_Inputs);
                if (string.IsNullOrEmpty(userDto.Name) || string.IsNullOrWhiteSpace(userDto.Name))
                    return WebApiHelper.GetErrorResponce<int>(CPStatusCode.InvalidInputData, string.Format(Resources.Error_Invalid_Parameter_Value, "Name"));
                if (string.IsNullOrEmpty(userDto.ContactNumber) || string.IsNullOrWhiteSpace(userDto.ContactNumber))
                    return WebApiHelper.GetErrorResponce<int>(CPStatusCode.InvalidInputData, string.Format(Resources.Error_Invalid_Parameter_Value, "ContactNumber"));
                if (string.IsNullOrEmpty(userDto.NIC) || string.IsNullOrWhiteSpace(userDto.NIC))
                    return WebApiHelper.GetErrorResponce<int>(CPStatusCode.InvalidInputData, string.Format(Resources.Error_Invalid_Parameter_Value, "NIC"));
                if (userDto.Category == Category.All)
                    return WebApiHelper.GetErrorResponce<int>(CPStatusCode.InvalidInputData, string.Format(Resources.Error_Invalid_Parameter_Value, "Category"));

                int result = 0;
                DataManager dm = new DataManager();
                result = 0;// dm.AddUser(userDto);

                response = new Response<int>(CPStatusCode.Success, Resources.Success, result);

                return response;
            }
            catch(Exception ex)
            {
                Log.Exception(ex);
                return WebApiHelper.GetErrorResponce<int>(CPStatusCode.AddUserFailed, string.Concat(Resources.Error_AddUser, " - ", ex.Message));

            }
            finally
            {
                Log.TraceEnd();
            }
        }

        /// <summary>
        /// Get active users in the system
        /// </summary>
        /// <param name="category">User category</param>
        /// <returns>User list</returns>
        [HttpGet]
        public Response<List<UserDto>> GetUsers(Category category)
        {
            Log.TraceStart();
            Response<List<UserDto>> response = null;
            try
            {
                //Validate data
                if (!Enum.IsDefined(typeof(Category), category))
                    return WebApiHelper.GetErrorResponce<List<UserDto>>(CPStatusCode.InvalidInputData, string.Format(Resources.Error_Invalid_Parameter_Value, "Category"));

                
                DataManager dm = new DataManager();
                DataTable dataTable = null;// dm.GetUsers(category);

                if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
                {
                    List<UserDto> userList = new List<UserDto>();
                    userList = DataMap.MapUserData(dataTable);
                    response = new Response<List<UserDto>>(CPStatusCode.Success, Resources.Success, userList);

                }
                else
                {
                   response = new Response<List<UserDto>>(CPStatusCode.NoUserData, Resources.Info_NoUserData, null);
                }

                return response;
            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return WebApiHelper.GetErrorResponce<List<UserDto>>(CPStatusCode.AddUserFailed, string.Concat(Resources.Error_AddUser, " - ", ex.Message));

            }
            finally
            {
                Log.TraceEnd();
            }
        }

        [HttpPost]
        public async Task<Response<HttpResponseMessage>> SendSMS(SmsRequestDto smsRequestDto)
        {
            Log.TraceStart();
            Response<HttpResponseMessage> response = null;
            try
            {
                HttpContent content;
                string data = JsonConvert.SerializeObject(smsRequestDto);
                StringContent queryString = new StringContent(data);

                var url = @"/sms/send";
                //HttpResponseMessage httpresponse;
                using (var client = new HttpClient())
                {
                    //Send HTTP requests
                    client.BaseAddress = new Uri(@"http://localhost:10001");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var result = await client.PostAsync(url, queryString);
                    string resultContent = await result.Content.ReadAsStringAsync();

                }

            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return WebApiHelper.GetErrorResponce<HttpResponseMessage>(CPStatusCode.AddUserFailed, string.Concat(Resources.Error_AddUser, " - ", ex.Message));

            }
            finally
            {
                Log.TraceEnd();
            }
            return response;
        }

        [HttpPost]
        public async Task<Response<HttpResponseMessage>> ReceiveSMS(SmsRequestDto smsRequestDto)
        {
            Log.TraceStart();
            Response<HttpResponseMessage> response = null;
            try
            {
                HttpContent content;
                string data = JsonConvert.SerializeObject(smsRequestDto);
                StringContent queryString = new StringContent(data);

                var url = @"/sms/send";
                //HttpResponseMessage httpresponse;
                using (var client = new HttpClient())
                {
                    //Send HTTP requests
                    client.BaseAddress = new Uri(@"http://localhost:10001");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    var result = await client.PostAsync(url, queryString);
                    string resultContent = await result.Content.ReadAsStringAsync();

                }

            }
            catch (Exception ex)
            {
                Log.Exception(ex);
                return WebApiHelper.GetErrorResponce<HttpResponseMessage>(CPStatusCode.AddUserFailed, string.Concat(Resources.Error_AddUser, " - ", ex.Message));

            }
            finally
            {
                Log.TraceEnd();
            }
            return response;
        }

        

    }
}
