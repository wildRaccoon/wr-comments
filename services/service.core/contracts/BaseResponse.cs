using System.Runtime.Serialization;

namespace service.core.contracts
{
    [DataContract]
    public class BaseResponse
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string Message { get; set; }
    }
}