using service.authorise.documents;
using System.Threading.Tasks;

namespace service.authorise.interfaces
{
    public interface IAuthoriseService
    {
        Task<CheckTokenResponse> CheckTokenAsync(CheckTokenRequest request);

        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}