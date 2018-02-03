using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using wr.repository.context;

namespace wr.repository.extension
{
    public static class ISearchContextExtension
    {
        #region Search
        public static SearchDescriptor<T> ApplyContext<T>(this SearchDescriptor<T> sd)
            where T : BaseContract
        {
            var sc = new SearchContext<T>();
            return sc.ApplyContext(sd);
        }
        #endregion

        #region Add
        public static IIndexRequest<T> ApplyContext<T>(this IndexDescriptor<T> sd)
            where T : BaseContract
        {
            var sc = new SearchContext<T>();
            return sc.ApplyContext(sd);
        }
        #endregion

        #region Update
        public static IIndexRequest<T> ApplyContext<T>(this IndexDescriptor<T> sd, T entry)
            where T : BaseContract
        {
            var sc = new SearchContext<T>();
            return sc.ApplyContext(sd,entry);
        }
        #endregion

        #region Delete
        public static IDeleteRequest ApplyContext<T>(this DeleteDescriptor<T> sd, T entry)
            where T : BaseContract
        {
            var sc = new SearchContext<T>();
            return sc.ApplyContext(sd, entry);
        }
        #endregion
    }
}
