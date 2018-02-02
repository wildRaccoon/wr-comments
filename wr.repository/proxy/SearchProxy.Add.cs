using Nest;
using System;
using System.Threading.Tasks;
using wr.repository.context;
using wr.repository.extension;

namespace wr.repository.proxy
{
    public partial class SearchProxy
    {
        #region ISearchProxy
        private EntryContext<T> CheckResponseAdd<T>(IIndexResponse resp, EntryContext<T> entry)
            where T : class
        {
            if (resp.IsValid && resp.Result == Result.Created)
            {
                entry.UpdateContext(resp.Index, resp.Version);
            }
            else
            {
                throw new Exception($"created: {resp.Created} : {resp.ServerError}");
            }

            return entry;
        }

        public EntryContext<T> Add<T>(EntryContext<T> entry)
            where T : class
        {
            var resp = _client.Index<T>(entry.Item, s => s.ApplyContext());
            return CheckResponseAdd(resp, entry);
        }

        public async Task<EntryContext<T>> AddAsync<T>(EntryContext<T> entry)
                where T : class
        {
            var resp = await _client.IndexAsync<T>(entry.Item, s => s.ApplyContext());
            return CheckResponseAdd(resp, entry);
        }
        #endregion
    }
}