using DataAccess.Neo4j;
using DataLoader.Serialization;
using DataLoader.Serialization.Contracts;
using Domain.Repositories.Contracts;
using Infrastructure.DependencyInjection;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

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

            application.Command("load", LoadCommand(serviceProvider));

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

                var graphNameArg = c.Argument("[graph-name]", "The name of the graph to be created/replaced");
                var pathArg = c.Argument("[path]", "The directory with input files");
                var uriOpt = c.Option("-uri <uri>", "The URI of the Neo4j instance", CommandOptionType.SingleValue);
                var usernameOpt = c.Option("-usr|--username <username>", "The username to use to connect to Neo4j", CommandOptionType.SingleValue);
                var passwordOpt = c.Option("-pwd|--password <password>", "The password to use to connect to Neo4j", CommandOptionType.SingleValue);
                c.HelpOption("-?|-h|--help");

                c.OnExecute(() => ExecuteLoad(c, serviceProvider, graphNameArg, pathArg, uriOpt, usernameOpt, passwordOpt));
            };
        }

        private static int ExecuteLoad(
            CommandLineApplication application,
            IServiceProvider serviceProvider,
            CommandArgument graphNameArg,
            CommandArgument pathArg,
            CommandOption uriOpt,
            CommandOption usernameOpt,
            CommandOption passwordOpt)
        {
            if (string.IsNullOrWhiteSpace(graphNameArg.Value))
            {
                return Error(application, "The 'graph-name' is missing");
            }

            if (string.IsNullOrWhiteSpace(pathArg.Value) || !Directory.Exists(pathArg.Value))
            {
                return Error(application, "The 'path' is missing or nonexistent");
            }

            Uri uri;

            if (!uriOpt.HasValue() || !Uri.TryCreate(uriOpt.Value(), UriKind.Absolute, out uri))
            {
                return Error(application, "The 'uri' is missing or could not be parsed");
            }

            if (!usernameOpt.HasValue())
            {
                return Error(application, "The 'username' is missing");
            }

            if (!passwordOpt.HasValue())
            {
                return Error(application, "The 'password' is missing");
            }

            var neo4jConfiguration = new Neo4jConfiguration
            {
                Uri = uriOpt.Value(),
                Username = usernameOpt.Value(),
                Password = passwordOpt.Value()
            };

            var graphDeserializer = serviceProvider.GetService<IFileSystemGraphDeserializer>();
            var graphRepository = serviceProvider.GetService<IGraphRepository>();
            var nodeRepository = serviceProvider.GetService<INodeRepository>();

            var graphDeserializationResult = graphDeserializer.Deserialize(graphNameArg.Value, pathArg.Value);

            //// We need to clean up previous records first.
            //// Since we consider a node an aggregate root for performance reasons,
            //// we may be relying on eventual consistency between graph and its nodes,
            //// depending on the DAL implementation.

            // Delete all nodes of the graph.
            nodeRepository.DeleteAllForGraph(graphNameArg.Value);

            // Delete the graph.
            graphRepository.Delete(graphNameArg.Value);

            //// Now we create new records

            // Create the graph
            graphRepository.Create(graphDeserializationResult.Graph);

            // Create the graph nodes and edges
            nodeRepository.CreateAll(graphDeserializationResult.Nodes);

            return Success(application, "The graph was successfully created/updated");
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

            services.AddVVGraphCommon();

            services.AddSingleton<IFileSystemNodeDeserializer, XmlFileNodeDeserializer>();
            services.AddSingleton<IFileSystemGraphDeserializer, DirectoryGraphDeserializer>();

            return services.BuildServiceProvider();
        }
    }
}