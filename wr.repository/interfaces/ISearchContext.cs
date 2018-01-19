using Nest;
using System;
using System.Collections.Generic;
using System.Text;

namespace wr.repository.interfaces
{
    public interface ISearchContext<T>
        where T: class
    {
        SearchDescriptor<T> ApplyContext(SearchDescriptor<T> searchDescriptor);
    }
}
