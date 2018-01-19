using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;
using System.Linq;
using wr.contracts;
using wr.repository;

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

            var idx_read = "wr_read";
            var idx_write = "wr_write";

            var data = 
            (
            from a in AppDomain.CurrentDomain.GetAssemblies()
            from t in a.GetTypes()
            let atrs = t.GetCustomAttributes(typeof(ElasticsearchTypeAttribute), true)
            from atr in atrs
            where atr != null
            select atr as ElasticsearchTypeAttribute
            ).ToList();

            cli.Map<Comment>(d =>
                d.AutoMap()
                .Index($"{idx_write}")
            );

            //cli.Index<Comment>(new Comment()
            //{
            //    Id = Guid.NewGuid().ToString(),
            //    Content = "Sample comment #2"
            //}, s => s.Index(idx_write));

            var resp = cli.Search<Comment>(s => s.Index(idx_read).Que);

            if (resp.IsValid)
            {
                var items = resp.Hits.ToList().Select(x =>
                {
                    x.Source.SourceIndex = x.Index;
                    return x.Source;
                }).ToList();

                items.ForEach(x => Console.WriteLine($"[{x.Id} - {x.SourceIndex}]   {x.Content}"));
            }
        }
    }
}
