using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using wr.repository.context;
using wr.repository.extension;

namespace wr.repository.proxy
{
    public partial class SearchProxy
    {
        #region ISearchProxy
        private List<EntryContext<T>> CheckResponse<T>(ISearchResponse<T> resp)
            where T : class
        {
            if (resp.IsValid)
            {
                return resp.Hits.Select(item => new EntryContext<T>(item)).ToList();
            }
            else
            {
                throw new Exception($"{resp.ServerError}");
            }
        }

        public List<EntryContext<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector)
            where T : class
        {
            var resp = _client.Search<T>(s => {
                    s = s.ApplyContext();
                    return selector?.Invoke(s) ?? s;
                });

            return CheckResponse(resp);
        }

        public async Task<List<EntryContext<T>>> SearchAsync<T>(Func<SearchDescriptor<T>, ISearchRequest> selector = null)
            where T : class 
        {
            var resp = await _client.SearchAsync<T>(s => {
                s = s.ApplyContext();
                return selector?.Invoke(s) ?? s;
            });

            return CheckResponse(resp);
        }
        #endregion
    }
}