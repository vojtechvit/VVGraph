using DataLoader.Serialization;
using DataLoader.Serialization.Contracts;
using Domain.Factories;
using Domain.Factories.Contracts;
using Domain.Validation;
using Domain.Validation.Contracts;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebServices.ApiModel;
using WebServices.ApiModel.Mappers;
using WebServices.ApiModel.Mappers.Contracts;
using WebServices.AspNetCore.Proxy;
using WebServices.AspNetCore.Proxy.Contracts;

namespace DataLoader
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var serviceProvider = CreateServiceProvider();

            var application = new CommandLineApplication
            {
                Name = "dataloader",
                Description = "Utility used to load graphs stored as files on local filesystem into VV Graph database.",
                FullName = "VV Graph Data Loader"
            };

            application.Command("load-graph", LoadCommand(serviceProvider));

            // show the help for the application
            application.OnExecute(() =>
            {
                application.ShowHelp();
                return 2;
            });

            return application.Execute(args);
        }

        private static Action<CommandLineApplication> LoadCommand(IServiceProvider serviceProvider)
        {
            return c =>
            {
                c.Description = "Loads the XML node files from the specified directory into VV Graph database."
                + " If a graph with the specified name already exists, it will get replaced.";

                var graphNameArg = c.Argument("[name]", "The name of the graph to be created/replaced");
                var directoryOpt = c.Option("-directory <directory>", "The directory with input files", CommandOptionType.SingleValue);
                var baseUrlOpt = c.Option("-url <url>", "The base URL of VV Graph API", CommandOptionType.SingleValue);
                c.HelpOption("-?|-h|--help");

                c.OnExecute(() => ExecuteLoad(c, serviceProvider, graphNameArg, directoryOpt, baseUrlOpt));
            };
        }

        private static int ExecuteLoad(
            CommandLineApplication application,
            IServiceProvider serviceProvider,
            CommandArgument graphNameArg,
            CommandOption directoryOpt,
            CommandOption baseUrlOpt)
        {
            if (string.IsNullOrWhiteSpace(graphNameArg.Value))
            {
                return Error(application, "The 'graph-name' is missing");
            }

            if (!directoryOpt.HasValue() || !Directory.Exists(directoryOpt.Value()))
            {
                return Error(application, "The 'directory' is missing or nonexistent");
            }

            Uri baseUrl;

            if (!baseUrlOpt.HasValue() || !Uri.TryCreate(baseUrlOpt.Value(), UriKind.Absolute, out baseUrl))
            {
                return Error(application, "The 'uri' is missing or could not be parsed");
            }

            var neo4jConfiguration = new VVGraphClientConfiguration
            {
                BaseUrl = baseUrl
            };

            var graphDeserializer = serviceProvider.GetService<IFileSystemGraphDeserializer>();
            var graphMapper = serviceProvider.GetService<IGraphMapper>();
            var vvgraphClient = serviceProvider.GetService<IVVGraphClient>();

            var graphDeserializationResult = graphDeserializer.Deserialize(
                graphNameArg.Value,
                directoryOpt.Value());

            UpdateDatabaseAsync(
                graphNameArg,
                vvgraphClient,
                graphMapper,
                graphDeserializationResult,
                CancellationToken.None).Wait();

            return Success(application, "The graph was successfully created/updated");
        }

        private static async Task UpdateDatabaseAsync(
            CommandArgument graphNameArg,
            IVVGraphClient vvgraphClient,
            IGraphMapper graphMapper,
            GraphDeserializationResult graphDeserializationResult,
            CancellationToken cancellationToken)
        {
            var domainGraph = graphDeserializationResult.Graph;

            var graph = graphMapper.Map(graphDeserializationResult.Graph, graphDeserializationResult.Nodes);

            await vvgraphClient.PutGraphAsync(graph, cancellationToken);
        }

        private static int Error(CommandLineApplication application, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            application.ShowHelp();
            return 2;
        }

        private static int Success(CommandLineApplication application, string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ResetColor();
            application.ShowHelp();
            return 2;
        }

        private static IServiceProvider CreateServiceProvider()
        {
            var services = new ServiceCollection();

            // Domain Validators
            services.AddSingleton<IGraphValidator, GraphValidator>();
            services.AddSingleton<INodeValidator, NodeValidator>();

            // Domain Factories
            services.AddSingleton<IGraphFactory, GraphFactory>();
            services.AddSingleton<INodeFactory, NodeFactory>();
            services.AddSingleton<IPathFactory, PathFactory>();

            // Serializers
            services.AddSingleton<IFileSystemNodeDeserializer, XmlFileNodeDeserializer>();
            services.AddSingleton<IFileSystemGraphDeserializer, DirectoryGraphDeserializer>();

            // Api Model Mappers
            services.AddSingleton<IGraphMapper, GraphMapper>();
            services.AddSingleton<INodeMapper, NodeMapper>();

            // VVGraph Proxy
            services.AddSingleton<IVVGraphClient, VVGraphClient>();

            return services.BuildServiceProvider();
        }
    }
}