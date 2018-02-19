using ChatApp.Properties;
using CP.Constants;
using CP.Data;
using CP.Dto;
using CP.Dto.Caas;
using CP.Enum;
using CP.IdeaMart;
using CP.Utility;
using Nanosoft.IdeaMartAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ChatApp.Controllers
{
    /// <summary>
    /// Chat App based methods
    /// </summary>
    public class UserChatController : ApiController
    {
        static readonly string _appId = ConfigurationManager.AppSettings["appId"];
        static readonly string _password = ConfigurationManager.AppSettings["password"];
        static readonly int _codeLength = Int32.Parse(ConfigurationManager.AppSettings["codeLength"]);

        #region APIs

        /// <summary>
        /// Send sms messages
        /// </summary>
        /// <param name="ideaMartResponse"></param>
        [HttpPost]
        public async Task<IHttpActionResult> Send(IdeaMartResponse ideaMartResponse)
        {
            //SIMULATOR Application Data: //URL: http://localhost:41440/api/v1/userchat/send

            IdeaMartStatusResponseDto statusResponse = new IdeaMartStatusResponseDto();
            var receivedMsg = ideaMartResponse.message;
            var userAddress = ideaMartResponse.sourceAddress;

            try
            {
                var receivedMsgArray = receivedMsg.Split(' ');

                //Validate message
                if (receivedMsgArray == null || receivedMsgArray.Length < 2)
                {
                    Sms.Send(userAddress, Resources.Error_InvalidInputMessage);
                    BadRequest();
                }

                var key1 = receivedMsgArray[0].ToUpper();
                var key2 = receivedMsgArray[1].ToUpper();

                if (key1 == ShortKey.REG.ToString() && key2 == ShortKey.CC.ToString())// Register
                {
                    statusResponse = await RegisterUser(userAddress);
                }
                else if (key1 == ShortKey.UNREG.ToString() && key2 == ShortKey.CC.ToString())// Unregister
                {
                    statusResponse = await UnregisterUser(userAddress);
                }
                else if (key1 == ShortKey.CC.ToString() && key2.Length == _codeLength)// send chat sms
                {
                    Chat(userAddress, receivedMsg);
                    statusResponse.requestId = ideaMartResponse.requestId;
                    statusResponse.statusCode = Consts.SUCCESS;
                    statusResponse.statusDetail = Consts.SUCCESS;
                    statusResponse.timeStamp = DateTime.UtcNow.ToString();
                    statusResponse.version = ideaMartResponse.version;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest();
            }
            return Ok(statusResponse);
        }

        /// <summary>
        /// Charge from users for chat app
        /// </summary>
        //[HttpPost]
        //public async Task<IHttpActionResult> Charge()
        //{
        //    //URL: http://localhost:2513/41440/v1/chat/charge
        //    IdeaMartStatusResponse statusResponse = new IdeaMartStatusResponse();
        //    Log.TraceStart();
        //    try
        //    {
        //        ChatAppDM dm = new ChatAppDM();
        //        var userList = dm.GetAllUserAddressList();

        //        foreach (var userAddress in userList)
        //        {
        //            var chargingResponse = await ChargeUser(userAddress);
        //        }

        //        statusResponse.requestId = "User Chat App";
        //        statusResponse.statusCode = Consts.SUCCESS;
        //        statusResponse.statusDetail = "Charging service successful for User Chat App.";
        //        statusResponse.timeStamp = DateTime.UtcNow.ToString();
        //        statusResponse.version = "1.0";
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.ToString());
        //        return BadRequest();
        //    }
        //    Log.TraceEnd();

        //    return Ok(statusResponse);
        //}

        #endregion

        #region private methods

        private async Task<IdeaMartStatusResponseDto> RegisterUser(string userAddress)
        {
            //Log.TraceStart();
            try
            {
                //Subscribe User
                Subscription subscription = new Subscription();
                var response = await subscription.Add(_appId, _password, userAddress);

                if (response != null)
                {
                    ChatAppDM dm = new ChatAppDM();

                    if (response.statusCode == Consts.SUCCESS)//subscription successful
                    {
                       // var chargingResponse = await ChargeUser(userAddress);
                        //ChargeUser(userAddress);

                        //Generate code
                        string code = Common.GenerateCode(_codeLength);
                        while (dm.CodeExists(code))
                        {
                            code = Common.GenerateCode(_codeLength);
                        }

                        //Save in DB
                        var userId = dm.SubscribeUser(userAddress, code);

                        if (userId > 0)
                        {
                            var message = string.Format(Resources.Info_RegisterSuccess, code.ToUpper());
                            Sms.Send(userAddress, message);
                        }
                        else
                        {
                            Log.Error(string.Format(Resources.Error_UserSaveFailed, userAddress));
                        }

                    }
                    else if (response.statusCode == Consts.USER_ALREADY_REGISTERED)
                    {
                        string code = dm.GetUserCode(userAddress);

                        var message = string.Format(Resources.Info_UserAlreadyRegistered, code);
                        Sms.Send(userAddress, message);
                    }
                    else
                    {
                        var message = string.Format(response.statusDetail);
                        Sms.Send(userAddress, message);
                    }

                }
                //Log.TraceEnd();
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }

        }

        //private async Task<DirectDebitResponseDto> ChargeUser(string userAddress)
        //{
        //    Caas caas = new Caas();
        //    var chargingResponse = await caas.Charge(_appId, _password, userAddress);

        //    if (chargingResponse.statusCode != Consts.SUCCESS)
        //    {
        //        Log.Error(string.Format(Resources.Error_ChargeFailed, userAddress));
        //        //return false;
        //    }
        //    return chargingResponse;
        //}

        private async Task<IdeaMartStatusResponseDto> UnregisterUser(string userAddress)
        {
            //Log.TraceStart();
            try
            {
                //Unsubscribe User
                Subscription subscription = new Subscription();
                var response = await subscription.Remove(_appId, _password, userAddress);

                if (response != null)
                {
                    if (response.statusCode == Consts.SUCCESS)//subscription removed successfully
                    {
                        //Save in DB
                        ChatAppDM dm = new ChatAppDM();
                        var status = dm.UnsubscribeUser(userAddress);

                        if (status)
                        {
                            var message = Resources.Info_UnregisterSuccess;
                            Sms.Send(userAddress, message);
                        }
                    }
                    else if (response.statusCode == Consts.USER_NOT_REGISTERED)
                    {
                        var message = string.Format(Resources.Info_UserNotRegistered);
                        Sms.Send(userAddress, message);
                    }
                    else
                    {
                        var message = response.statusDetail;
                        Sms.Send(userAddress, message);
                    }
                }
                //Log.TraceEnd();
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
        }

        private void Chat(string userAddress, string receivedMsg)
        {
            try
            {
                string[] receivedMsgArray = receivedMsg.Split(' ');
                var key = receivedMsgArray[0].ToUpper();
                var code = receivedMsgArray[1].ToUpper();

                if (receivedMsgArray.Length < 3)
                {
                    Sms.Send(userAddress, Resources.Error_InvalidChatFormat);
                    throw new Exception();
                }

                var startIndex = string.Format("{0} {1} ", key, code).Length;
                var message = receivedMsg.Substring(startIndex);

                ChatAppDM dm = new ChatAppDM();
                var receiverAddress = dm.GetUserAddress(code);

                if (!string.IsNullOrEmpty(receiverAddress))
                {
                    Sms.Send(receiverAddress, message);
                }
                else
                {
                    Sms.Send(userAddress, Resources.Error_InvalidCode);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
        }

       
        #endregion

    }
}
