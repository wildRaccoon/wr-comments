using Nest;
using System;
using System.Collections.Generic;
using System.Text;
using wr.repository.context;

namespace wr.repository.extension
{
    public static class ISearchContextExtension
    {
        public static SearchDescriptor<T> ApplyContext<T>(this SearchDescriptor<T> sd)
            where T: class
        {
            var sc = new SearchContext<T>();
            return sc.ApplyContext(sd);
        }
    }
}
