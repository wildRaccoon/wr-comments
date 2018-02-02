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
        private EntryContext<T> CheckResponseUpdate<T>(IIndexResponse resp, EntryContext<T> entry)
            where T : class
        {
            if (resp.IsValid && resp.Result == Result.Updated)
            {
                entry.UpdateContext(resp.Index, resp.Version);
            }
            else
            {
                throw new Exception($"{resp.ServerError}");
            }

            return entry;
        }

        public EntryContext<T> Update<T>(EntryContext<T> entry)
            where T : class
        {
            var resp = _client.Index<T>(entry.Item, s => s.ApplyContext(entry));
            return CheckResponseUpdate(resp, entry);
        }

        public async Task<EntryContext<T>> UpdateAsync<T>(EntryContext<T> entry)
                where T : class
        {
            var resp = await _client.IndexAsync<T>(entry.Item, s => s.ApplyContext(entry));
            return CheckResponseUpdate(resp, entry);
        }
        #endregion
    }
}