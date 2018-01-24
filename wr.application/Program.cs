using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Linq;
using wr.contracts;
using wr.repository;
using wr.repository.interfaces;

namespace wr.application
{
    class Program
    {
        static void Main(string[] args)
        {
            var sc = new ServiceCollection();

            var connection = new ConnectionSettings(new Uri("http://localhost:9200"))
                .EnableDebugMode();

            sc.AddSingleton<IConnectionSettingsValues>(connection);
            sc.AddSingleton<IElasticClient, ElasticClient>();
            sc.AddWRRepository();

            var sp = sc.BuildServiceProvider();

            var cli = sp.GetRequiredService<ISearchProxy>();

            //cli.Index<Comment>(new Comment()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Content = "Sample comment #2"
            //}, s => s.Index(idx_write));

            var resp = cli.Search<Comment>();
            
            resp.ForEach(x => 
                    Console.WriteLine($"[{x.Item.Id} - {x.Index} - {x.Version}]   {x.Item.Content}")
                );

            foreach (Comment c in resp)
            {
                Console.WriteLine(c.Content);
            }
        }
    }
}
