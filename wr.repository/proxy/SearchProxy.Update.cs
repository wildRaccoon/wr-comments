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
        private T CheckResponseUpdate<T>(IIndexResponse resp, T entry)
            where T : BaseContract
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

        public T Update<T>(T entry)
            where T : BaseContract
        {
            var resp = _client.Index<T>(entry, s => s.ApplyContext(entry));
            return CheckResponseUpdate(resp, entry);
        }

        public async Task<T> UpdateAsync<T>(T entry)
                where T : BaseContract
        {
            var resp = await _client.IndexAsync<T>(entry, s => s.ApplyContext(entry));
            return CheckResponseUpdate(resp, entry);
        }
        #endregion
    }
}