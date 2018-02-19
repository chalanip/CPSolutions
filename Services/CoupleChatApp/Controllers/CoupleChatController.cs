using CoupleChatApp.Properties;
using CP.Constants;
using CP.Data;
using CP.Dto;
using CP.Dto.Subscription;
using CP.Enum;
using CP.IdeaMart;
using CP.Utility;
using System;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;

namespace CoupleChatApp.Controllers
{
    public class CoupleChatController : ApiController
    {
        static readonly string _appId = ConfigurationManager.AppSettings["appId"];
        static readonly string _password = ConfigurationManager.AppSettings["password"];
        static readonly int _codeLength = Int32.Parse(ConfigurationManager.AppSettings["codeLength"]);
        static readonly string _environment = ConfigurationManager.AppSettings["environment"];

        /// <summary>
        /// Send sms messages to Couple Chat App
        /// </summary>
        /// <param name="ideaMartResponse"></param>
        [HttpPost]
        public async Task<IHttpActionResult> Send(IdeaMartResponseDto ideaMartResponse)
        {
            //Log.TraceStart();
            //Log.Data("IdeaMartResponseDto: ", ideaMartResponse );
            IdeaMartStatusResponseDto statusResponse = new IdeaMartStatusResponseDto();

            try
            {
                var receivedMsg = ideaMartResponse.message.Trim();
                var userAddress = ideaMartResponse.sourceAddress;           
                var receivedMsgArray = receivedMsg.Split(' ');

                //Validate message
                if (receivedMsgArray == null || receivedMsgArray.Length < 2 || receivedMsgArray[0].ToUpper() != ShortKey.CC.ToString())
                {
                    Sms.Send(userAddress, Resources.Error_InvalidInputMessage);
                    BadRequest();
                }

                var key1 = receivedMsgArray[0].ToUpper();
                var message = receivedMsg.Substring(key1.Length + 1);

                if (message.Substring(0, 1) == "#")
                {
                    var key2 = receivedMsgArray[1].ToUpper();
                    
                    if (key2 == @"#HELP" && receivedMsgArray.Length == 2)// Help
                    {
                        message = Resources.Info_Help;
                        Sms.Send(userAddress, message);                        
                    }
                    else if (key2 == @"#DEL" && receivedMsgArray.Length == 2)// Delete linked users
                    {
                        RemoveUser(userAddress);                       
                    }
                    else if (key2 == @"#CODE" && receivedMsgArray.Length == 2)// Get Code
                    {
                        GetCode(userAddress);
                    }
                    else if (key2 == @"#ADD" && receivedMsgArray.Length > 2)// Link user
                    {
                        var code = receivedMsgArray[2].ToUpper();//for couple user
                        LinkUser(userAddress, code);
                    }
                    else if (key2 == @"#PATS" && receivedMsgArray.Length == 2)// Get user count
                    {
                        GetUserCount(userAddress);
                    }
                    else if (key2.Substring(0, 1) == @"#" && receivedMsgArray.Length > 2)//Send to specific user
                    {
                        var code = key2.Substring(1);
                        message = message.Substring(key2.Length + 1);
                        SendSmsByCode(userAddress, code, message);
                    }
                    else
                    {
                        message = Resources.Error_InvalidSmsFormat;
                        Sms.Send(userAddress, message);
                    }
                }                
                else // send sms to linked user
                {                    
                    SendSmsToLinkedUser(userAddress, message);
                }

                statusResponse.requestId = ideaMartResponse.requestId;
                statusResponse.statusCode = Consts.SUCCESS;
                statusResponse.statusDetail = Consts.SUCCESS;
                statusResponse.timeStamp = DateTime.UtcNow.ToString();
                statusResponse.version = ideaMartResponse.version;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest();
            }
            return Ok(statusResponse);
        }

        /// <summary>
        ///  Registration/ un-registration of the user to the Couple Chat App
        /// </summary>
        /// <param name="SubscriptionNotificationRequestDto">Subscription notification request data</param>
        [HttpPost]
        public async Task<IHttpActionResult> Subscribe(SubscriptionNotificationRequestDto ideaMartResponse)
        {
            //Log.TraceStart();
            //Log.Data("IdeaMartResponseDto: ", ideaMartResponse);

            IdeaMartStatusResponseDto statusResponse = new IdeaMartStatusResponseDto();
            try
            {
                var subscriberId = (_environment == "prod") ? "tel:" + ideaMartResponse.subscriberId : ideaMartResponse.subscriberId;
                var status = ideaMartResponse.status;  

                if (status == "REGISTERED")// Register
                {
                    RegisterUser(subscriberId);
                }
                else if (status == "UNREGISTERED")// Unregister
                {
                    UnregisterUser(subscriberId);
                }

                statusResponse.requestId = "";
                statusResponse.statusCode = Consts.SUCCESS;
                statusResponse.statusDetail = Consts.SUCCESS;
                statusResponse.timeStamp = DateTime.UtcNow.ToString();
                statusResponse.version = ideaMartResponse.version;
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                return BadRequest();
            }
            return Ok(statusResponse);
        }
                
        #region private methods

        /// <summary>
        /// Register user
        /// </summary>
        /// <param name="subscriberId">User address</param>
        private void RegisterUser(string subscriberId)
        {
            //Log.TraceStart();
            try
            {
                CoupleChatDM dm = new CoupleChatDM();

                //Generate code and send               
                string code = Common.GenerateCode(_codeLength);
                
                while (dm.CodeExists(code))
                {
                    code = Common.GenerateCode(_codeLength);
                }
                //Save in DB
                var userId = dm.AddUser(subscriberId, code);

                if (userId > 0)
                {
                    var message = string.Format(Resources.Info_RegisterSuccess, code.ToUpper());
                    Sms.Send(subscriberId, message);
                }
                else
                {
                    Log.Error(string.Format(Resources.Error_UserSaveFailed, subscriberId));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }

        }

        /// <summary>
        /// Link user with another user by secret code
        /// </summary>
        /// <param name="sourceAddress">User address</param>
        /// <param name="coupleCode">Secret code of another user to be linked</param>
        private void LinkUser(string sourceAddress, string coupleCode)
        {
            //Log.TraceStart();
            try
            {
                CoupleChatDM dm = new CoupleChatDM();
                //When code is given then user should be linked
                if (!string.IsNullOrEmpty(coupleCode))
                {                    
                    var coupleUserAddress = dm.GetUserNumberByCode(coupleCode);

                    if (string.IsNullOrEmpty(coupleUserAddress))
                    {
                        var message = string.Format(Resources.Error_InvalidCode);
                        Sms.Send(sourceAddress, message);
                    }
                    if (coupleUserAddress == sourceAddress)
                    {
                        var message = string.Format(Resources.Error_InvalidCode);
                        Sms.Send(sourceAddress, message);
                    }
                    else if (dm.IsUserLinked(sourceAddress))
                    {
                        var message = string.Format(Resources.Error_UserAlreadyLinked);
                        Sms.Send(sourceAddress, message);
                    }
                    else if (dm.IsUserLinked(coupleUserAddress))
                    {
                        var message = string.Format(Resources.Error_CodeAlreadyLinked);
                        Sms.Send(sourceAddress, message);
                    }
                    else
                    {
                        //Save in DB
                        var status = dm.LinkUser(sourceAddress, coupleUserAddress);
                        if (status)
                        {
                            var message = string.Format(Resources.Info_RegisterSuccessCoupleUser);
                            Sms.Send(sourceAddress, message);
                            //Send OK message to first user
                            Sms.Send(coupleUserAddress, message);
                        }
                        else
                        {
                            Log.Error(string.Format(Resources.Error_AddAndLinkUserFailed, sourceAddress));
                            var message = string.Format(Resources.Error_CannotCoupleUser);
                            Sms.Send(sourceAddress, message);
                        }
                    }
                                
                }                
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }

        }

        /// <summary>
        /// Remove linked users
        /// </summary>
        /// <param name="userAddress">User address</param>
        private void RemoveUser(string userAddress)
        {
            //Log.TraceStart();
            try
            {
                CoupleChatDM dm = new CoupleChatDM();

                //Save in DB
                var coupledUserAddress = dm.GetCoupleUserNumberByUserNumber(userAddress);
                var status = dm.RemoveLinkedUser(userAddress);

                if (status)
                {
                    var message = Resources.Info_RemoveLinkedUser;
                    Sms.Send(userAddress, message);

                    if (!string.IsNullOrEmpty(coupledUserAddress))
                        Sms.Send(coupledUserAddress, message);
                }
                else
                {
                    Log.Error(string.Format(Resources.Error_RemoveLinkedUserFailed));                    
                }
               
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }

        }

        /// <summary>
        /// Unregister user
        /// </summary>
        /// <param name="subscriberId">User address</param>
        private void UnregisterUser(string subscriberId)
        {
            try
            {
                //Save in DB
                CoupleChatDM dm = new CoupleChatDM();
                var status = dm.RemoveUser(subscriberId);               
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
        }       

        /// <summary>
        /// Send sms to user who is linked by secret code
        /// </summary>
        /// <param name="userAddress">Sender's address</param>
        /// <param name="message">Sms message</param>
        private void SendSmsToLinkedUser(string userAddress, string message)
        {
            try
            {
                CoupleChatDM dm = new CoupleChatDM();
                var receiverAddress = dm.GetCoupleUserNumberByUserNumber(userAddress);

                if (!string.IsNullOrEmpty(receiverAddress))
                {
                    Sms.Send(receiverAddress, message);
                }
                else
                {
                    Sms.Send(userAddress, Resources.Error_NotLinkedUser);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Send sms to user given by a secret code
        /// </summary>
        /// <param name="userAddress">Sender's address</param>
        /// <param name="code">Receiver's secret code</param>
        /// <param name="receivedMsg">Sms message</param>
        private void SendSmsByCode(string userAddress, string code, string receivedMsg)
        {
            try
            {
                CoupleChatDM dm = new CoupleChatDM();
                var receiverAddress = dm.GetUserNumberByCode(code);

                if (!string.IsNullOrEmpty(receiverAddress))
                {
                    Sms.Send(receiverAddress, receivedMsg);
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

        /// <summary>
        /// Get user code
        /// </summary>
        /// <param name="userAddress">Sender's address</param>
        private void GetCode(string userAddress)
        {
            //Log.TraceStart();
            try
            {
                CoupleChatDM dm = new CoupleChatDM();

                var code = dm.GetUserCode(userAddress);
               
                if (!string.IsNullOrEmpty(code))
                {
                    var message = string.Format(Resources.Info_GetUserCode, code);
                    Sms.Send(userAddress, message);

                }
                else
                {
                    Log.Error(string.Format(Resources.Error_GetUserCodeFailed));
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }

        }

        /// <summary>
        /// Get active user count
        /// </summary>
        /// <param name="userAddress">Sender's address</param>
        private void GetUserCount(string userAddress)
        {
            //Log.TraceStart();
            try
            {
                CoupleChatDM dm = new CoupleChatDM();

                int count = dm.GetActiveUserCount();
                int cplCount = dm.GetCoupleUserCount();
                Sms.Send(userAddress, string.Format(Resources.Info_UserCount,count.ToString(), cplCount.ToString()));

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }

        }

        #endregion

        #region not using
        //SIMULATOR Application Data: //URL: http://localhost:1762/api/v1/couplechat/send

        /// <summary>
        /// Charge from users for chat app
        /// </summary>
        //[HttpPost]
        //public async Task<IHttpActionResult> Charge()
        //{
        //    //URL: http://localhost:2513/api/v1/chat/charge
        //    IdeaMartStatusResponse statusResponse = new IdeaMartStatusResponse();
        //    Log.TraceStart();
        //    try
        //    {
        //        CoupleChatDM dm = new CoupleChatDM();
        //        var userList = dm.GetAllUserAddressList();

        //        foreach (var userAddress in userList)
        //        {
        //            var chargingResponse = await ChargeUser(userAddress);
        //        }

        //        statusResponse.requestId = "CoupleChat App";
        //        statusResponse.statusCode = Consts.SUCCESS;
        //        statusResponse.statusDetail = "Charging service successful for CoupleChat App.";
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


        //private void Chat(string userAddress, string receivedMsg)
        //{
        //    try
        //    {
        //        //string[] receivedMsgArray = receivedMsg.Split(' ');
        //        //var key = receivedMsgArray[0].ToUpper();

        //        //if (receivedMsgArray.Length < 2)
        //        //{
        //        //    Sms.Send(userAddress, Resources.Error_InvalidChatFormat);
        //        //    throw new Exception();
        //        //}

        //        //var startIndex = string.Format("{0} ", key).Length;
        //        //Starting from 'CC '
        //        var message = receivedMsg.Substring(3);

        //        CoupleChatDM dm = new CoupleChatDM();
        //        var receiverAddress = dm.GetCoupleUserNumberByUserNumber(userAddress);

        //        if (!string.IsNullOrEmpty(receiverAddress))
        //        {
        //            Sms.Send(receiverAddress, message);
        //        }
        //        else
        //        {
        //            Sms.Send(userAddress, Resources.Error_InvalidCode);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex.ToString());
        //        throw;
        //    }
        //}


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

        #endregion

    }
}
