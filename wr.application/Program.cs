using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Linq;
using wr.contracts;
using wr.repository;
using wr.repository.context;

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

            var client = new ElasticClient(connection);

            sc.AddSingleton<IElasticClient, ElasticClient>();

            var sp = sc.BuildServiceProvider();
            var cli = sp.GetRequiredService<IElasticClient>();

            //cli.Index<Comment>(new Comment()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Content = "Sample comment #2"
            //}, s => s.Index(idx_write));

            var seachContext = new SearchContext<Comment>();
            var resp = cli.Search<Comment>(s => seachContext.ApplyContext(s));

            if (resp.IsValid)
            {
                resp.Hits.ToList().ForEach(x =>
                {
                    Console.WriteLine($"[{x.Id} - {x.Index} - {x.Version}]   {x.Source.Content}");
                });
            }
        }
    }
}
