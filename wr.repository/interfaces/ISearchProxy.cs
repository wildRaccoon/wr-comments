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
        List<T> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector = null)
            where T : BaseContract;

        Task<List<T>> SearchAsync<T>(Func<SearchDescriptor<T>, ISearchRequest> selector = null)
                where T : BaseContract;

        T Update<T>(T entry)
               where T : BaseContract;

        Task<T> UpdateAsync<T>(T entry)
                where T : BaseContract;

        T Add<T>(T entry)
                where T : BaseContract;

        Task<T> AddAsync<T>(T entry)
                where T : BaseContract;

        Task DeleteAsync<T>(T entry)
                where T : BaseContract;

        void Delete<T>(T entry)
            where T : BaseContract;
    }
}
