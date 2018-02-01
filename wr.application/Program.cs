using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
            sc.AddLogging();

            var sp = sc.BuildServiceProvider();

            var logFactory = sp.GetRequiredService<ILoggerFactory>();
            logFactory.AddConsole();


            var cli = sp.GetRequiredService<ISearchProxy>();
            var log = sp.GetRequiredService<ILogger<Program>>();

            try
            {
                #region add data
                //var r1 = sp.GetRequiredService<IElasticClient>().Index<Comment>(new Comment()
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    Content = "Sample comment 7"
                //}, s => s.Index("wr_write"));

                //sp.GetRequiredService<IElasticClient>().Index<Comment>(new Comment()
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    Content = "Sample comment 6"
                //}, s => s.Index("wr_write"));
                //
                //return; 
                #endregion

                var resp = cli.Search<Comment>();

                resp.ToList().ForEach(x =>
                        log.LogInformation($"[{x.Item.Id} - {x.Index} - {x.Version}]   {x.Item.Content}")
                    );

                foreach (Comment item in resp)
                {
                    log.LogInformation($"[{item.Id}] {item.Content}");
                }

                resp.Add(new Comment());
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error while executing query.");
            }

            Console.ReadLine();
        }
    }
}
