using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using service.authorise.documents;
using service.authorise.interfaces;
using service.comment.interfaces;
using service.comment.interfaces.documents;

namespace service.comment.Controllers
{

    public class CommentsController : Controller, ICommentsController
    {
        private IAuthoriseController _autorise { get; set; }
        private ILogger<CommentsController> _logger { get; set; }

        #region Constructor

        public CommentsController(ILogger<CommentsController> logger,IAuthoriseController autorise)
        {
            _logger = logger;
            _autorise = autorise;
        }

        #endregion

        #region ICommentsController
        [HttpPost]
        public RegisterArtitleResponse RegisterArtitle(RegisterArtitleRequest request)
        {
            var ar = _autorise.CheckToken(new CheckTokenRequest()
            {
                Token = request.Token
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