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
        private void CheckResponseDelete<T>(IDeleteResponse resp, EntryContext<T> entry)
            where T: class
        {
            if (resp.IsValid && resp.Result == Result.Deleted)
            {
                entry.ResetContext();
            }
            else
            {
                throw new Exception($"{resp.ServerError}");
            }
        }

        public void Delete<T>(EntryContext<T> entry)
            where T : class
        {
            var resp = _client.Delete<T>(entry.Item, s => s.ApplyContext(entry));

            CheckResponseDelete(resp,entry);
        }

        public async Task DeleteAsync<T>(EntryContext<T> entry)
                where T : class
        {
            var resp = await _client.DeleteAsync<T>(entry.Item, s => s.ApplyContext(entry));

            CheckResponseDelete(resp,entry);
        }
        #endregion
    }
}