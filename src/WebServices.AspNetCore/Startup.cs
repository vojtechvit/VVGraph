using Domain.Algorithms.Contracts;
using Domain.Factories;
using Domain.Factories.Contracts;
using Domain.Repositories.Contracts;
using Domain.Validation;
using Domain.Validation.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using DataAccess.Neo4j;
using DataAccess.Neo4j.Contracts;
using DataAccess.Neo4j.DelegatedAlgorithms;
using DataAccess.Neo4j.Repositories;

namespace WebServices.AspNetCore
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddSingleton<IGraphValidator, GraphValidator>();
            services.AddSingleton<INodeValidator, NodeValidator>();

            services.AddSingleton<IGraphFactory, GraphFactory>();
            services.AddSingleton<IPathFactory, PathFactory>();

            services.AddSingleton<INeo4jDriver, Neo4jDriver>();
            services.AddSingleton<IPathFinder, Neo4jPathFinder>();
            services.AddSingleton<IGraphRepository, Neo4jGraphRepository>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseMvc();
        }
    }
}