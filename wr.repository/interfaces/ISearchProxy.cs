using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using wr.repository.context;

namespace wr.repository.interfaces
{
    public interface ISearchProxy
    {
        List<EntryContext<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector = null)
            where T: class;

        Task<List<EntryContext<T>>> SearchAsync<T>(Func<SearchDescriptor<T>, ISearchRequest> selector = null)
                where T : class;

        EntryContext<T> Update<T>(EntryContext<T> entry)
               where T : class;

        Task<EntryContext<T>> UpdateAsync<T>(EntryContext<T> entry)
                where T : class;

        EntryContext<T> Add<T>(EntryContext<T> entry)
                where T : class;

        Task<EntryContext<T>> AddAsync<T>(EntryContext<T> entry)
                where T : class;

        Task DeleteAsync<T>(EntryContext<T> entry)
                where T : class;

        void Delete<T>(EntryContext<T> entry)
            where T : class;
    }
}
