using service.comment.interfaces.documents;
using System.Threading.Tasks;

namespace service.comment.interfaces
{
    public interface ICommentsService
    {
        Task<RegisterArtitleResponse> RegisterArtitleAsync(RegisterArtitleRequest request);
    }
}