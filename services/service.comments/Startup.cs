using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nest;
using service.authorise.interfaces;
using wr.repository;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace service.comments
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection sc)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connection = new ConnectionSettings(new Uri("http://localhost:9200"))
                .EnableDebugMode();

            sc.AddSingleton<IConfiguration>(configuration);

            sc.AddAuthoriseClient();

            sc.AddSingleton<IConnectionSettingsValues>(connection);
            sc.AddSingleton<IElasticClient, ElasticClient>();

            sc.AddWRRepository();
            sc.AddLogging();

            sc.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                loggerFactory.AddConsole(LogLevel.Trace);
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=comments}/{action=}/{id?}");
            });
        }
    }
}
