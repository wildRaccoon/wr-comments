using service.core.contracts;

namespace service.authorise.documents
{
    public class CheckTokenResponse : BaseResponse
    {
        public string UserIdentity { get; set; }
    }
}