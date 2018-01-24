using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using wr.repository.context;

namespace wr.repository.interfaces
{
    public interface ISearchProxy
    {
        List<EntryContext<T>> Search<T>(Func<SearchDescriptor<T>, ISearchRequest> selector = null)
            where T: class;
    }
}
