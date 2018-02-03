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
        public static Comment CreateNew()
        {
            return new Comment()
            {
                Content = $"Created On {DateTime.Now}",

                Id = Guid.NewGuid().ToString()
            };
        }

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
                #region search
                var respTask = cli.SearchAsync<Comment>();
                respTask.Wait();

                var resp = respTask.Result;

                resp.ToList().ForEach(x =>
                        log.LogInformation($"Sync - [{x.Id} - {x.Index} - {x.Version}]   {x.Content}")
                    );

                resp = cli.Search<Comment>();

                resp.ToList().ForEach(x =>
                        log.LogInformation($"Async - [{x.Id} - {x.Index} - {x.Version}]   {x.Content}")
                    ); 
                #endregion

                #region sync
                var addedItem = cli.Add(CreateNew());
                log.LogInformation($"Add - [{addedItem.Id} - {addedItem.Index} - {addedItem.Version}]   {addedItem.Content}");

                addedItem.Content += $"\r\n Updated : {DateTime.Now}";
                cli.Update(addedItem);
                log.LogInformation($"Update - [{addedItem.Id} - {addedItem.Index} - {addedItem.Version}]   {addedItem.Content}");

                cli.Delete(addedItem);
                log.LogInformation($"Delete - [{addedItem.Id} - {addedItem.Index} - {addedItem.Version}]   {addedItem.Content}");
                #endregion

                #region async
                var taskAdd = cli.AddAsync(CreateNew());
                taskAdd.Wait();
                addedItem = taskAdd.Result;
                log.LogInformation($"AddAsync - [{addedItem.Id} - {addedItem.Index} - {addedItem.Version}]   {addedItem.Content}");

                addedItem.Content += $"\r\n Updated : {DateTime.Now}";
                cli.UpdateAsync(addedItem).Wait();
                log.LogInformation($"UpdateAsync - [{addedItem.Id} - {addedItem.Index} - {addedItem.Version}]   {addedItem.Content}");

                cli.DeleteAsync(addedItem).Wait();
                log.LogInformation($"DeleteAsync - [{addedItem.Id} - {addedItem.Index} - {addedItem.Version}]   {addedItem.Content}"); 
                #endregion
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error while executing query.");
            }

            Console.ReadLine();
        }
    }
}
