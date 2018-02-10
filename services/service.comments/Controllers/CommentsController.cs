using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using service.authorise.documents;
using service.authorise.interfaces;
using service.comments.contracts;
using service.comments.interfaces;
using service.comments.interfaces.documents;
using service.core.contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wr.repository.interfaces;

namespace service.comments.Controllers
{
    public class CommentsController : Controller, ICommentsService
    {
        private IAuthoriseService _autorise { get; set; }

        private ISearchProxy _searchProxy { get; set; }

        private ILogger<CommentsController> _logger { get; set; }

        #region Constructor

        public CommentsController(ILogger<CommentsController> logger, IAuthoriseService autorise, ISearchProxy searchProxy)
        {
            _logger = logger;
            _autorise = autorise;
            _searchProxy = searchProxy;
        }

        #endregion

        #region ICommentsController

        private async Task<TRes> CheckToken<TReq, TRes>(TReq req, Func<TReq, Task<TRes>> action)
            where TReq : BaseRequest
            where TRes : BaseResponse, new()
        {
            var ar = await _autorise.CheckTokenAsync(new CheckTokenRequest()
            {
                Token = req.Token,
                UserIdentity = req.UserIdentity
            });

            if (ar.Success)
            {
                return await action(req);
            }
            else
            {
                return new TRes()
                {   
                    Success = false,
                    Message = "Invalid Token"
                };
            }
        }

        [HttpPost("[controller]/registerartitle")]
        public async Task<RegisterArtitleResponse> RegisterArtitleAsync([FromBody] RegisterArtitleRequest request)
        {
            return await CheckToken(request, async (req) =>
             {
                 var rs = await _searchProxy.SearchAsync<Artitle>(s => s.Query(q => q.Ids(si => si.Values(req.Id))));

                 if (rs?.Count > 0)
                 {
                     return new RegisterArtitleResponse()
                     {
                         Success = false,
                         Message = $"Entry with Id {req.Id} already exists"
                     };
                 }
                 else
                 {
                     await _searchProxy.AddAsync(new Artitle()
                     {
                         Content = req.Content,
                         CreateDate = DateTime.Now,
                         Tags = new List<string>(),
                         UserIdenty = req.UserIdentity,
                         Id = req.Id
                     });

                     return new RegisterArtitleResponse();
                 }
             });
        }
        #endregion
    }
}