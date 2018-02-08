using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace service.comment.interfaces
{
    public static class IServiceCollectionExtension
    {
        public static void AddCommentsClient(this IServiceCollection sc)
        {
            sc.AddSingleton<ICommentsService, CommentsCllient>();
        }
    }
}
