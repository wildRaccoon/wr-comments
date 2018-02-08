using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service.authorise.interfaces
{
    public static class IServiceCollectionExtension
    {
        public static void AddAuthoriseClient(this IServiceCollection sc)
        {
            sc.AddSingleton<IAuthoriseService, AuthoriseClient>();
        }
    }
}
