using System.Linq;
using System.Security.Cryptography;
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

        private string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                return Encoding.UTF8.GetString(result);
            }
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
    }
}