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
        private T CheckResponseAdd<T>(IIndexResponse resp, T entry)
            where T : BaseContract
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

        public T Add<T>(T entry)
            where T : BaseContract
        {
            var resp = _client.Index<T>(entry, s => s.ApplyContext());
            return CheckResponseAdd(resp, entry);
        }

        public async Task<T> AddAsync<T>(T entry)
                where T : BaseContract
        {
            var resp = await _client.IndexAsync<T>(entry, s => s.ApplyContext());
            return CheckResponseAdd(resp, entry);
        }
        #endregion
    }
}