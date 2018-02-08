using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using service.comment.interfaces.documents;
using service.core;

namespace service.comment.interfaces
{
    public class CommentsCllient : ICommentsService
    {
        public ILogger<CommentsCllient> _logger { get; private set; }
        public ServiceClient _client { get; set; }

        public CommentsCllient(ILogger<CommentsCllient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new ServiceClient(configuration["services:comment"]);
        }

        public async Task<RegisterArtitleResponse> RegisterArtitleAsync(RegisterArtitleRequest request)
        {
            var resp = await _client.PostAsync<RegisterArtitleRequest, RegisterArtitleResponse>("registerartitle", request);
            return resp;
        }
    }
}
