using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using service.authorise.contracts;
using service.authorise.documents;
using service.authorise.interfaces;

namespace service.authorise.controller
{
    public class AuthoriseController : Controller, IAuthoriseService
    {
        private ILogger<AuthoriseController> _logger { get; set; }
        private AuthContext _authContext { get; set; }

        private const int ExpiredInSeconds = 600; //10 minutes

        public AuthoriseController(ILogger<AuthoriseController> logger, AuthContext authContext)
        {
            _logger = logger;
            _authContext = authContext;
        }

        [HttpPost("[controller]/checktoken")]
        public async Task<CheckTokenResponse> CheckTokenAsync([FromBody] CheckTokenRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request?.UserIdentity) || string.IsNullOrEmpty(request?.Token))
                {
                    return new CheckTokenResponse()
                    {
                        Success = false,
                        Message = "Invalid Request"
                    };
                }

                _logger.LogInformation($"CheckToken for {request.UserIdentity} : {request.Token}");
                
                var user = (from u in _authContext.Users
                            where u.UserIdentity == request.UserIdentity.ToLower()
                            select u).FirstOrDefault();

                var session = (from s in _authContext.UserSessions
                               where s.UserDataId == user.UserDataId
                               select s).FirstOrDefault();

                if (session?.Token == request.Token && session?.LastUpdated.AddSeconds(ExpiredInSeconds) > DateTime.Now)
                {
                    session.LastUpdated = DateTime.Now;
                    _authContext.Update(session);
                    await _authContext.SaveChangesAsync();

                    return new CheckTokenResponse()
                    {
                        Success = true,
                        Message = "Success"
                    };
                }

                return new CheckTokenResponse()
                {
                    Success = false,
                    Message = "Invalid Token"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when processing user check token.");

                return new CheckTokenResponse()
                {
                    Success = false,
                    Message = "Server Error"
                };
            }
        }

        [HttpPost("[controller]/login")]
        public async Task<LoginResponse> LoginAsync([FromBody] LoginRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request?.UserIdentity) || string.IsNullOrEmpty(request?.Password))
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid Request"
                    };
                }

                var user = (from u in _authContext.Users
                            where u.UserIdentity == request.UserIdentity.ToLower()
                            select u).FirstOrDefault();
                
                if (user == null || user?.Password != request.Password)
                {
                    return new LoginResponse()
                    {
                        Success = false,
                        Message = "Invalid User Or Password"
                    };
                }

                UserSession session = (from s in _authContext.UserSessions
                                       where s.UserDataId == user.UserDataId
                                       select s).FirstOrDefault();

                if ((session = user.Session) != null)
                {
                    if (session.LastUpdated.AddSeconds(ExpiredInSeconds) < DateTime.Now)
                    {
                        _authContext.UserSessions.Remove(session);
                        session = null;
                    }
                    else
                    {
                        session.LastUpdated = DateTime.Now;
                        _authContext.UserSessions.Update(session);
                    }
                }
                
                if(session == null)
                {
                    session = new UserSession() {
                        UserDataId = user.UserDataId,
                        Token = Guid.NewGuid().ToString()
                    };

                    _authContext.Add(session);
                }

                await _authContext.SaveChangesAsync();

                return new LoginResponse() {
                    Success = true,
                    Message = "Success",
                    Token = session.Token
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"Error when processing user login.");

                return new LoginResponse() {
                    Success = false,
                    Message = "Server Error"
                };
            }            
        }
    }
}