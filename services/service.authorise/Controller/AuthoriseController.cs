using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using service.authorise.documents;
using service.authorise.interfaces;

namespace service.authorise.controller
{
    public class AuthoriseController : Controller, IAuthoriseService
    {
        private ILogger<AuthoriseController> _logger { get; set; }

        public AuthoriseController(ILogger<AuthoriseController> logger)
        {
            _logger = logger;
        }

        [HttpPost("[controller]/checktoken")]
        public async Task<CheckTokenResponse> CheckTokenAsync([FromBody] CheckTokenRequest request)
        {
            _logger.LogInformation($"CheckToken for {request.UserIdentity} : {request.Token}");

            var resp = new CheckTokenResponse()
            {
                Success = request.Token.ToList().Contains('0')
            };

            return resp;
        }

        [HttpPost("[controller]/checktoken")]
        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            return null;
        }
    }
}