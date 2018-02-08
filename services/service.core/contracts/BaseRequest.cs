using System.Runtime.Serialization;

namespace service.core.contracts
{
    [DataContract]
    public class BaseRequest
    {
        [DataMember]        
        public string Token { get; set; }

        [DataMember]
        public string UserIdentity { get; set; }
    }
}