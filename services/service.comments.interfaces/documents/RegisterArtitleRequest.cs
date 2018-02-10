using service.core.contracts;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace service.comments.interfaces.documents
{
    [DataContract]
    public class RegisterArtitleRequest : BaseRequest
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string Content { get; set; }
    }
}