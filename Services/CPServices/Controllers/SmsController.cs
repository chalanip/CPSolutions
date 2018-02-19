using CP.Constants;
using CP.Data;
using CP.Enum;
using CP.Utility;
using CPServices.api;
using CPServices.Properties;
using CPServices.Utility;
using Nanosoft.IdeaMartAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;

namespace CPServices.Controllers
{
    /// <summary>
    /// Sms based methods
    /// </summary>
    public class SmsController : ApiController
    {
        readonly string _appId = "App_000001";
        readonly string _password = "password";
        static readonly string _baseUrl = "http://localhost:7000/";// ConfigurationManager.AppSettings["baseUrl"];

        /// <summary>
        /// Send sms messages
        /// </summary>
        /// <param name="ideaMartResponse"></param>
        [HttpPost]
        //public async Task<IHttpActionResult> Send(IdeaMartResponse ideaMartResponse)
        public async Task<IHttpActionResult> Send(IdeaMartResponse ideaMartResponse)//(MoSmsReq moSmsReq)//
        {
            //SIMULATOR Application Data: //URL: http://localhost:2513/api/v1/sms/send

            IdeaMartStatusResponse statusResponse = new IdeaMartStatusResponse();
            var receivedMsg = ideaMartResponse.message;
            var userAddress = ideaMartResponse.sourceAddress;

            try
            {   
                var receivedMsgArray = receivedMsg.Split(' ');
                
                //Validate message
                if (receivedMsgArray ==  null || receivedMsgArray.Length < 2)
                {
                    SendSmsToUser(userAddress, "Invalid input message. Please type correctly.");
                    BadRequest();                    
                }
                
                var key1 = receivedMsgArray[0].ToUpper();
                var key2 = receivedMsgArray[1].ToUpper();
                
                if (key1 == ShortKey.REG.ToString() && key2 == ShortKey.CC.ToString())// Register
                {
                    RegisterUser(userAddress);                   
                }
                if (key1 == ShortKey.UNREG.ToString() && key2 == ShortKey.CC.ToString())// Unregister
                {
                    UnregisterUser(userAddress);
                }
                else if (key1 == ShortKey.CC.ToString() && key2 == ShortKey.ADD.ToString())// Add another user (couple user)
                {
                    CoupleUser(userAddress, receivedMsgArray);                                     
                }
                else if (key1 == ShortKey.CC.ToString() && key2 == ShortKey.OK.ToString())// Accept request from couple user
                {
                    AcceptUser(userAddress, receivedMsgArray);                    
                }
                else if (key1 == ShortKey.CC.ToString() && key2 == ShortKey.END.ToString())// Reject/End request from couple user
                {
                    CancelUser(userAddress, receivedMsgArray);                    
                }
                else if (key1 == ShortKey.CC.ToString() && key2 == ShortKey.SEND.ToString())// send chat sms
                {
                    Chat(userAddress, receivedMsgArray, receivedMsg);                   
                }
                return Ok();
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                
                return BadRequest();
            }           

        }

        //Working
        private bool SendSmsToUser(string address, string message)
        {
            bool status = false;

            MtSmsReq mtSmsReq = new MtSmsReq();
            mtSmsReq.applicationId = _appId;
            mtSmsReq.password = _password;
            mtSmsReq.destinationAddress = address;
            mtSmsReq.message = message;// String.Join(", ", address.ToArray());

            SmsSender smsSender = new SmsSender(string.Concat(_baseUrl, Consts.URL_SMS_SEND));
            MtSmsResp mtSmsResp = smsSender.SendSMSReq(mtSmsReq);

            if (mtSmsResp.statusCode == "SUCCESS")
                status = true;
            
            return status;
        }
        
        private async void RegisterUser(string userAddress)
        {
            Log.TraceStart();
            //try
            //{
            //    //include transaction scope
            //    //using (TransactionScope trx = new TransactionScope(TransactionScopeOption.RequiresNew))
            //    //{
            //        //Subscribe User
            //        Subscription subscription = new Subscription();
            //        var response = await subscription.Add(_appId, _password, userAddress);

            //        if (response.StatusCode == CP.Enum.StatusCode.Success)
            //        {
            //            if (response.Data.statusCode == "S1000")//subscription successful
            //            {
            //                //Save in DB
            //                DataManager dm = new DataManager();
            //                var userId = dm.SubscribeUser(userAddress);

            //                if (userId > 0)
            //                {
            //                    var message = string.Format("Thank you for registering our service.");
            //                    var result = SendSmsToUser(userAddress, message);

            //                }
            //                //trx.Complete();
            //            }
            //            else
            //            {
            //                var message = string.Concat(response.Data.statusDetail, userAddress);
            //                var result = SendSmsToUser(userAddress, message);
            //            }
            //            // Charge per day

            //        }
            //        else
            //        {
            //            var message = string.Concat(response.Data.statusDetail, userAddress);
            //            var result = SendSmsToUser(userAddress, message);
            //        }
                    
            //    //}
            //}
            //catch (Exception ex)
            //{
            //    Log.Error(ex.ToString());
            //    throw;
            //}
            Log.TraceEnd();
        }

        private async void UnregisterUser(string userAddress)
        {
            Log.TraceStart();
            try
            {
               //Unsubscribe User
                //Subscription subscription = new Subscription();
                //var response = await subscription.Remove(_appId, _password, userAddress);

                //if (response.StatusCode == CP.Enum.StatusCode.Success)
                //{
                //    if (response.Data.statusCode == "S1000")//subscription removed successfully
                //    {
                //        //Save in DB
                //        DataManager dm = new DataManager();
                //        var status = dm.UnsubscribeUser(userAddress);

                //        if (status)
                //        {
                //            var message = string.Concat("You have unsubscribed from the App successfully. Thank you for using our service. ", userAddress);
                //            var result = SendSmsToUser(userAddress, message);
                //        }
                //    }
                //    else
                //    {
                //        var message = string.Concat(response.Data.statusDetail, userAddress);
                //        var result = SendSmsToUser(userAddress, message);
                //    }
                //}
                //else
                //{
                //    var message = string.Concat(response.Data.statusDetail, userAddress);
                //    var result = SendSmsToUser(userAddress, message);
                //}
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            Log.TraceEnd();
        }

        private void CoupleUser(string userAddress, string[] receivedMsgArray)
        {
            Log.TraceStart();
            try
            {
                //if (receivedMsgArray.Length < 3)
                //{
                //    SendSmsToUser(userAddress, "Invalid input. Please type \"CC ADD <mobile no>\" and send SMS again.");
                //    throw new Exception();
                //}

                //var newUserAddress = receivedMsgArray[2];
                //var message = string.Empty;
                //var coupleUserAddress = string.Empty;

                //if (ValidateMobileNumber(newUserAddress, out coupleUserAddress))
                //{
                //    DataManager dm = new DataManager();
                //    var status = dm.CoupleUser(userAddress, coupleUserAddress);
                //    if (status)
                //    {
                //        //Send sms to couple user
                //        message = string.Format("You have a request from {0}. If you accept the request please type \"CC OK {0} and send to 77000 to continue.", userAddress.Remove(0,4));
                //        SendSmsToUser(coupleUserAddress, message);

                //        //Send sms to first user
                //        message = string.Format("Please wait till you get the acceptance from {0}", coupleUserAddress.Remove(0, 4) );
                //        SendSmsToUser(userAddress, message);
                //    }
                //    else
                //    {
                //        var errmessage = string.Concat("Error in coupling user {0} with user {1}.", userAddress, coupleUserAddress);
                //        Log.Error(errmessage);
                //    }
                //}
                //else
                //{
                //    SendSmsToUser(userAddress, Resources.Error_InvalidMobileNumber);
                //}
                              
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            Log.TraceEnd();


        }

        private void AcceptUser(string userAddress, string[] receivedMsgArray)
        {
            Log.TraceStart();
            try
            {
                //if (receivedMsgArray.Length < 3)
                //{
                //    SendSmsToUser(userAddress, "Invalid input. Please type \"CC OK <mobile no>\" and SMS again.");
                //    throw new Exception();
                //}

                //var newUserAddress = receivedMsgArray[2];
                //var message = string.Empty;
                //var coupleUserAddress = string.Empty;

                //if (ValidateMobileNumber(newUserAddress, out coupleUserAddress))
                //{
                //    //Register new user
                //    RegisterUser(userAddress);

                //    DataManager dm = new DataManager();
                //    string code = dm.GetCoupleCode(userAddress, coupleUserAddress);

                //    if (string.IsNullOrEmpty(code))
                //    {
                //        Log.Error(string.Format("Chat code is empty for user {0} and {1}", coupleUserAddress, userAddress));
                //        throw new Exception();
                //    }
                //    //Send sms to couple user
                //    message = string.Format("You have accepted the request from user {0}. Type \"CC SEND {1} <message> and send to 77000 to chat with user.", coupleUserAddress.Remove(0, 4), code);
                //    SendSmsToUser(userAddress, message);

                //    //Send sms to first user
                //    message = string.Format("User {0} has accepted the request. Type \"CC SEND {1} <message> and send to 77000 to chat with user.", userAddress.Remove(0, 4), code);
                //    SendSmsToUser(coupleUserAddress, message);
                //}
                //else
                //{
                //   SendSmsToUser(userAddress, Resources.Error_InvalidMobileNumber);
                //}

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            Log.TraceEnd();


        }

        private void CancelUser(string userAddress, string[] receivedMsgArray)
        {
            Log.TraceStart();
            try
            {
                //if (receivedMsgArray.Length < 3)
                //{
                //    SendSmsToUser(userAddress, "Invalid input. Please type \"CC END <mobile no>\" and SMS again.");
                //    throw new Exception();
                //}

                //var newUserAddress = receivedMsgArray[2];
                //var message = string.Empty;
                //var coupleUserAddress = string.Empty;

                //if (ValidateMobileNumber(newUserAddress, out coupleUserAddress))
                //{
                //    //Reject/end chat with user
                //    DataManager dm = new DataManager();
                //    var result = dm.EndChat(userAddress, coupleUserAddress);

                //    if (result)
                //    {
                //        //Send sms to couple user
                //        message = string.Format("You have rejected the request from {0}.", coupleUserAddress.Remove(0, 4));
                //        SendSmsToUser(userAddress, message);

                //        //Send sms to first user ????? do we need to send
                //        message = string.Format("User {0} has rejected the chat request.", userAddress.Remove(0, 4));
                //        SendSmsToUser(coupleUserAddress, message);
                //    }
                //}
                //else
                //{
                //    SendSmsToUser(userAddress, Resources.Error_InvalidMobileNumber);
                //}
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            Log.TraceEnd();


        }

        private void Chat(string userAddress, string[] receivedMsgArray, string receivedMsg)
        {
            Log.TraceStart();
            try
            {
                //if (receivedMsgArray.Length < 4)
                //{
                //    SendSmsToUser(userAddress, "Invalid input. Please type \"CC SEND <code> <message>\" and send SMS again.");
                //    throw new Exception();
                //}

                //var code = receivedMsgArray[2];
                //var startIndex = string.Format("CC SEND {0} ", code).Length - 1;
                //var message = receivedMsg.Substring(startIndex);

                //DataManager dm = new DataManager();
                //var receiverAddress = dm.GetCoupleUserAddress(userAddress,code);

                //if (!string.IsNullOrEmpty(receiverAddress))
                //{
                //    ChargeUser(userAddress);
                //    SendSmsToUser(receiverAddress, message);                    
                //}
                //else
                //{
                //    SendSmsToUser(userAddress, Resources.Error_InvalidCode);
                //}  
            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
                throw;
            }
            Log.TraceEnd();
        }

        private async void ChargeUser(string userAddress)
        {
            Log.TraceStart();
            //DataManager dm = new DataManager();
            //var isCharged = dm.IsChargedToday(userAddress);

            //if (!isCharged)
            //{
            //    Caas caas = new Caas();
            //    var response = await caas.Charge(_appId, _password, userAddress);

            //    if (response.StatusCode == CP.Enum.StatusCode.Success)
            //    {
            //        var status = dm.ChargeUser(userAddress);
            //    }
            //    else
            //    {
            //        Log.Error(string.Format("Charging failed for user {0}.", userAddress));
            //    }
            //}
            Log.TraceEnd();

        }

        private bool ValidateMobileNumber(string number, out string updatedNumber)
        {
            updatedNumber = number;
            int n;
            if (string.IsNullOrEmpty(number) || int.TryParse(number, out n) || number.Length != 10)
                return false;

            updatedNumber = "tel:"  + number;
            return true;
        }

    }
}

