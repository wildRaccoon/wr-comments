using System.Runtime.Serialization;
using service.core.contracts;

namespace service.authorise.documents
{
    [DataContract]
    public class LoginRequest : BaseRequest
    {
        [DataMember]
        public string UserIdentity { get; set; }
        
        [DataMember]
        public string Password { get; set; }
    }
}