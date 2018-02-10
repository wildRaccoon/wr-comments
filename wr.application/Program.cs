using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using service.comments.interfaces;
using service.comments.interfaces.documents;
using System;
using System.IO;
using System.Linq;
using service.comments.contracts;
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

        public static void CheckComment()
        {
            var sc = new ServiceCollection();

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            sc.AddSingleton<IConfiguration>(configuration);
            sc.AddLogging();
            sc.AddCommentsClient();

            var sp = sc.BuildServiceProvider();

            var logFactory = sp.GetRequiredService<ILoggerFactory>();
            logFactory.AddConsole();

            var com = sp.GetRequiredService<ICommentsService>();
            var log = sp.GetRequiredService<ILogger<Program>>();

            try
            {
                var task = com.RegisterArtitleAsync(new RegisterArtitleRequest()
                {
                    Token = Guid.NewGuid().ToString(),
                    UserIdentity = "my_user"
                });

                task.Wait();

                log.LogInformation($"Result: {task.Result.Success}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Error while executing query.");
            }

            log.LogInformation("Completed");
            Console.ReadLine();
        }

        static void Main(string[] args)
        {
            //CheckComment();

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
