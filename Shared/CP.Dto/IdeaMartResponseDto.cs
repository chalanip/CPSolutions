using Nanosoft.IdeaMartAPI;

namespace CP.Dto
{
    public class IdeaMartResponseDto// : IdeaMartResponse
    {
        /*public string applicationId { get; set; }
        public string encoding { get; set; }
        public string message { get; set; }
        public string requestId { get; set; }
        public string sessionId { get; set; }
        public string sourceAddress { get; set; }
        public string ussdOperation { get; set; }
        public string version { get; set; }
        public string vlrAddress { get; set; }*/


        public string applicationId { get; set; }
        public string encoding { get; set; }
        public string message { get; set; }
        public string requestId { get; set; }
        public string sessionId { get; set; }
        public string sourceAddress { get; set; }
        public string[] destinationAddresses{ get; set; }
        public string version { get; set; }

        //public string[] destinationAddresses {
        //    get { return new[] { sourceAddress }; }
        //    set { }; }
        //public string version { get; set; }
        //public string vlrAddress { get; set; }
    }
}
