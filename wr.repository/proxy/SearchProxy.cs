using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using wr.repository.context;
using wr.repository.extension;
using wr.repository.interfaces;

namespace wr.repository.proxy
{
    public partial class SearchProxy : ISearchProxy
    {
        #region properties
        public IElasticClient _client { get; set; }
        #endregion

        #region constructor
        public SearchProxy(IElasticClient client)
        {
            _client = client;
        }
        #endregion

        #region ISearchProxy
        public List<EntryContext<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector)
            where T : class
        {
            var resp = _client.Search<T>(s => {
                    s = s.ApplyContext();
                    return selector?.Invoke(s) ?? s;
                });

            if (resp.IsValid)
            {
                return resp.Hits.Select(item => new EntryContext<T>(item)).ToList();
            }
            else
            {
                throw new Exception($"{resp.ServerError}");
            }
        }
        #endregion
    }
}