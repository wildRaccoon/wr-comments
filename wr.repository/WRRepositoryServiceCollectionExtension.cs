using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using wr.repository.interfaces;
using wr.repository.proxy;

namespace wr.repository
{
    public static class WRRepositoryServiceCollectionExtension
    {
        public static IServiceCollection AddWRRepository(this IServiceCollection entry)
        {
            entry.AddSingleton<ISearchProxy, SearchProxy>();
            return entry;
        }
    }
}
