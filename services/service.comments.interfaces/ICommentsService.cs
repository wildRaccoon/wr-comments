using service.comments.interfaces.documents;
using System.Threading.Tasks;

namespace service.comments.interfaces
{
    public interface ICommentsService
    {
        Task<RegisterArtitleResponse> RegisterArtitleAsync(RegisterArtitleRequest request);
    }
}