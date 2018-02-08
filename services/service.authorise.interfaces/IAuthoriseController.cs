using service.authorise.documents;

namespace service.authorise.interfaces
{
    public interface IAuthoriseController
    {
        CheckTokenResponse CheckToken(CheckTokenRequest request);
    }
}