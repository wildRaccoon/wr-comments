using System.Runtime.Serialization;
using service.core.contracts;

namespace service.authorise.documents
{
    [DataContract]
    public class LoginResponse : BaseResponse
    {
        [DataMember]
        public string Token { get; set; }
    }
}