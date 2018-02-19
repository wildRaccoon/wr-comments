using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using service.authorise.contracts;

namespace service.authorise
{
    public class Startup
    {
        private string _contentRootPath = "";

        public Startup(IHostingEnvironment env)
        {
            _contentRootPath = env.ContentRootPath;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection sc)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            sc.AddLogging();

            var connectionString = configuration.GetConnectionString("DefaultConnection");
            //%ROOTPATH%
            connectionString = connectionString.Replace("%ROOTPATH%", _contentRootPath);

            sc.AddDbContext<AuthContext>(options => options.UseSqlServer(connectionString));
            
            sc.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, AuthContext authContext)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            authContext.Ensure();
            authContext.Check();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=authorise}/{action=}/{id?}");
            });
        }
    }
}
