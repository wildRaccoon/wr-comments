using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using wr.repository.context;

namespace wr.repository.interfaces
{
    public interface ISearchContext<T>
        where T: BaseContract
    {
        SearchDescriptor<T> ApplyContext(SearchDescriptor<T> searchDescriptor);

        IIndexRequest<T> ApplyContext(IndexDescriptor<T> searchDescriptor);

        IIndexRequest<T> ApplyContext(IndexDescriptor<T> searchDescriptor, T entry);

        IDeleteRequest ApplyContext(DeleteDescriptor<T> sd, T entry);
    }
}
