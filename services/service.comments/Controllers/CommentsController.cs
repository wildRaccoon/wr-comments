using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using service.authorise.documents;
using service.authorise.interfaces;
using service.comments.interfaces;
using service.comments.interfaces.documents;
using System.Threading.Tasks;

namespace service.comments.Controllers
{
    public class CommentsController : Controller, ICommentsService
    {
        private IAuthoriseService _autorise { get; set; }
        private ILogger<CommentsController> _logger { get; set; }

        #region Constructor

        public CommentsController(ILogger<CommentsController> logger,IAuthoriseService autorise)
        {
            _logger = logger;
            _autorise = autorise;
        }

        #endregion

        #region ICommentsController
        [HttpPost("[controller]/registerartitle")]
        public async Task<RegisterArtitleResponse> RegisterArtitleAsync([FromBody] RegisterArtitleRequest request)
        {
            var ar = await _autorise.CheckTokenAsync(new CheckTokenRequest()
            {
                Token = request.Token,
                UserIdentity = request.UserIdentity
            });

            if (ar.Success)
            {
                return new RegisterArtitleResponse()
                {
                    Success = false
                };
            }
            else
            {
                return new RegisterArtitleResponse()
                {
                    Success = false
                };
            }
        }
        #endregion
    }
}