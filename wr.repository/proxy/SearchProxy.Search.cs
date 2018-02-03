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
        private List<T> CheckResponse<T>(ISearchResponse<T> resp)
            where T : BaseContract
        {
            if (resp.IsValid)
            {
                return resp.Hits.Select(item => {
                    var entry = item.Source;
                    entry.UpdateContext(item.Index, item.Version.GetValueOrDefault());
                    return entry;
                }
                ).ToList();
            }
            else
            {
                throw new Exception($"{resp.ServerError}");
            }
        }

        public List<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector)
            where T : BaseContract
        {
            var resp = _client.Search<T>(s => {
                    s = s.ApplyContext();
                    return selector?.Invoke(s) ?? s;
                });

            return CheckResponse(resp);
        }

        public async Task<List<T>> SearchAsync<T>(Func<SearchDescriptor<T>, ISearchRequest> selector = null)
            where T : BaseContract
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