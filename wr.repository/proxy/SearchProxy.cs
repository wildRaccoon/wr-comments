using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using wr.repository.context;
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

        public List<EntryContext<T>> Search<T>(Func<QueryContainerDescriptor<T>, QueryContainer> queryDesc)
            where T : class
        {
            var sc = new SearchContext<T>();

            var resp = _client.Search<T>(s => sc
                .ApplyContext(s)
                .Query(qt => queryDesc?.Invoke(qt))
            );

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