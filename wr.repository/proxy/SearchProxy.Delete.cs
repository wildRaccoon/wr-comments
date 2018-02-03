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
        private void CheckResponseDelete<T>(IDeleteResponse resp, T entry)
            where T: BaseContract
        {
            if (resp.IsValid && resp.Result == Result.Deleted)
            {
                entry.InitializeEntry();
            }
            else
            {
                throw new Exception($"{resp.ServerError}");
            }
        }

        public void Delete<T>(T entry)
            where T : BaseContract
        {
            var resp = _client.Delete<T>(entry, s => s.ApplyContext(entry));

            CheckResponseDelete(resp,entry);
        }

        public async Task DeleteAsync<T>(T entry)
                where T : BaseContract
        {
            var resp = await _client.DeleteAsync<T>(entry, s => s.ApplyContext(entry));

            CheckResponseDelete(resp,entry);
        }
        #endregion
    }
}