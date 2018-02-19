using CP.Constants;
using CP.Data;
using CP.Enum;
using CP.Utility;
using CPServices.api;
using CPServices.Properties;
using CPServices.Utility;
using Nanosoft.IdeaMartAPI;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace CPServices.Controllers
{
    /// <summary>
    /// Chat App based methods
    /// </summary>
    public class ChatController : ApiController
    {
        static readonly string _appId = ConfigurationManager.AppSettings["appId"];
        static readonly string _password = ConfigurationManager.AppSettings["password"];
        static readonly int _codeLength = Int32.Parse(ConfigurationManager.AppSettings["codeLength"]);
       
        
        /// <summary>
        /// Send sms messages
        /// </summary>
        /// <param name="ideaMartResponse"></param>
        [HttpPost]
        public async Task<IHttpActionResult> Send(IdeaMartResponse ideaMartResponse)
        {
            //SIMULATOR Application Data: //URL: http://localhost:2513/api/v1/chat/send

            IdeaMartStatusResponse statusResponse = new IdeaMartStatusResponse();
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
                if (key1 == ShortKey.UNREG.ToString() && key2 == ShortKey.CC.ToString())// Unregister
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
        [HttpPost]
        public async Task<IHttpActionResult> Charge()
        {
            //URL: http://localhost:2513/api/v1/chat/charge
            IdeaMartStatusResponse statusResponse = new IdeaMartStatusResponse();
            Log.TraceStart();
            try
            {                
                ChatAppDM dm = new ChatAppDM();
                var userList = dm.GetAllUserAddressList();

                foreach (var userAddress in userList)
                {
                    Caas caas = new Caas();
                    var response = await caas.Charge(_appId, _password, userAddress);

                    if (response.statusCode == Consts.SUCCESS)
                    {
                        //var status = dm.ChargeUser(userAddress);
                    }
                    else
                    {
                        Log.Error(string.Format(Resources.Error_ChargeFailed, userAddress));
                    }
                }

                statusResponse.requestId = "";
                statusResponse.statusCode = Consts.SUCCESS;
                statusResponse.statusDetail = Consts.SUCCESS;
                statusResponse.timeStamp = DateTime.UtcNow.ToString();
                statusResponse.version = "1.0";
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest();
            }
            Log.TraceEnd();

            return Ok(statusResponse);
        }



        #region private methods
        //private bool SendSmsToUser(string userAddress, string message)
        //{
        //    bool status = false;

        //    MtSmsReq mtSmsReq = new MtSmsReq();
        //    mtSmsReq.applicationId = _appId;
        //    mtSmsReq.password = _password;
        //    mtSmsReq.destinationAddress = userAddress;
        //    mtSmsReq.message = message;

        //    SmsSender smsSender = new SmsSender(string.Concat(Consts.URL_BASE_ADDRESS, Consts.URL_SMS_SEND));
        //    MtSmsResp mtSmsResp = smsSender.SendSMSReq(mtSmsReq);

        //    if (mtSmsResp.statusCode == "SUCCESS")
        //        status = true;
        //    else
        //        Log.Error(string.Format(Resources.Error_SendSmsFailed, userAddress));

        //    return status;
        //}

        private async Task<IdeaMartStatusResponse> RegisterUser(string userAddress)
        {
            Log.TraceStart();
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
                        Caas caas = new Caas();
                        var chargingResponse = await caas.Charge(_appId, _password, userAddress);

                        if (response.statusCode != Consts.SUCCESS)
                        {
                            Log.Error(string.Format(Resources.Error_ChargeFailed, userAddress));
                        }

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
                            var result = Sms.Send(userAddress, message);
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
                        var result = Sms.Send(userAddress, message);
                    }
                    else
                    {
                        var message = string.Format(response.statusDetail);
                        var result = Sms.Send(userAddress, message);
                    }
                    
                }
                Log.TraceEnd();
                return response;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }          
            
        }

        private async Task<IdeaMartStatusResponse> UnregisterUser(string userAddress)
        {
            Log.TraceStart();
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
                            var result = Sms.Send(userAddress, message);
                        }
                    }
                    else if (response.statusCode == Consts.USER_NOT_REGISTERED)
                    {
                        var message = string.Format(Resources.Info_UserNotRegistered);
                        var result = Sms.Send(userAddress, message);
                    }
                    else
                    {
                        var message = response.statusDetail;
                        var result = Sms.Send(userAddress, message);
                    }
                }
                Log.TraceEnd();
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
            Log.TraceStart();
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
            Log.TraceEnd();
        }

        //private async void ChargeUser()
        //{
        //    Log.TraceStart();
        //    DataManager dm = new DataManager();
        //    var userList = dm.GetAllUserAddressList();

        //    foreach (var userAddress in userList)
        //    { 
        //        Caas caas = new Caas();
        //        var response = await caas.Charge(_appId, _password, userAddress);

        //        if (response.StatusCode == CP.Enum.StatusCode.Success)
        //        {
        //            //var status = dm.ChargeUser(userAddress);
        //        }
        //        else
        //        {
        //            Log.Error(string.Format(Resources.Error_ChargeFailed, userAddress));
        //        }
        //   }
        //    Log.TraceEnd();

        //}

        #endregion

    }
}
