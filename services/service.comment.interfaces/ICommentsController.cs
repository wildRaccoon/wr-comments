using service.comment.interfaces.documents;

namespace service.comment.interfaces
{
    public interface ICommentsController
    {
        RegisterArtitleResponse RegisterArtitle(RegisterArtitleRequest request);
    }
}