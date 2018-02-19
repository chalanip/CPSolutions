using CP.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CP.Scheduler
{
    public class ConsumeData
    {
        /// <summary>
        /// Gets the is enabled key from the app config.
        /// </summary>
        private static readonly int IsEnabled = Convert.ToInt32(ConfigurationManager.AppSettings["IsEnabled"]);

        /// <summary>
        /// Gets the chat app api url from the app config.
        /// </summary>
        private static readonly string Chat_App_Url = ConfigurationManager.AppSettings["Chat_App_Url"];

        /// <summary>
        /// Gets the couple chat api url from the app config.
        /// </summary>
        private static readonly string Couple_Chat_App_Url = ConfigurationManager.AppSettings["Couple_Chat_App_Url"];

        /// <summary>
        /// Check chat app is enabled.
        /// </summary>
        private static readonly int IsChatAppEnabled = Convert.ToInt32(ConfigurationManager.AppSettings["IsChatAppEnabled"]);
        
        /// <summary>
        /// Check couple chat app is enabled.
        /// </summary>
        private static readonly int IsCoupleChatAppEnabled = Convert.ToInt32(ConfigurationManager.AppSettings["IsCoupleChatAppEnabled"]);
        
        /// <summary>
        /// Gets the api key from the app config.
        /// </summary>
        private static readonly string API_Key = ConfigurationManager.AppSettings["API_Key"];

        /// <summary>
        /// Gets the api key header from the app config.
        /// </summary>
        private static readonly string API_Key_Header = ConfigurationManager.AppSettings["API_Key_Header"];


        /// <summary>
        /// Hits the post API where the system will run enabled services.
        /// </summary>
        public async Task<bool> HitPostApi()
        {
            Log.TraceStart();
            try
            {
                if (IsEnabled == 1)
                {
                    using (var client = new WebClient())
                    {
                       client.Headers.Add("Content-Type:application/json");
                        client.Headers.Add("Accept:application/json");
                       // client.Headers.Add(String.Format("{0}:{1}", API_Key_Header, API_Key));

                        if (IsChatAppEnabled == 1)
                        {
                            //Runs chat app charging service
                            var chatAppChargingUri = new Uri(Chat_App_Url);
                            var response = await client.UploadStringTaskAsync(chatAppChargingUri, JsonConvert.SerializeObject(null));
                            Log.TraceExecute(response);
                        }

                        if (IsCoupleChatAppEnabled == 1)
                        {
                            //Runs couple chat app charging service
                            var coupleChatAppChargingUri = new Uri(Couple_Chat_App_Url);
                            var response = await client.UploadStringTaskAsync(coupleChatAppChargingUri, JsonConvert.SerializeObject(null));
                            Log.TraceExecute(response);
                        }                       
                        return true;
                    }
                }
                else
                {
                    Log.TraceExecute("Schedules are disabled.");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                Console.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                Log.TraceEnd();
            }
        }
    }
}
