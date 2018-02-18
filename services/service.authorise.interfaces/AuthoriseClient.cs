using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using service.authorise.documents;
using service.core;

namespace service.authorise.interfaces
{
    public class AuthoriseClient : IAuthoriseService
    {
        public ILogger<AuthoriseClient> _logger { get; private set; }
        public ServiceClient _client { get; set; }

        public AuthoriseClient(ILogger<AuthoriseClient> logger, IConfiguration configuration)
        {
            _logger = logger;
            _client = new ServiceClient(configuration["services:authorise"]);
        }

        #region IAuthoriseService
        public async Task<CheckTokenResponse> CheckTokenAsync(CheckTokenRequest request)
        {
            var resp = await _client.PostAsync<CheckTokenRequest, CheckTokenResponse>("checktoken", request);
            return resp;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            var resp = await _client.PostAsync<LoginRequest, LoginResponse>("login", request);

            return resp;
        }
        #endregion
    }
}
