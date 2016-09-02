using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace WebServices.AspNetCore.IntegrationTests.Fixtures
{
    public sealed class TestServerBuilder : IDisposable
    {
        private static string websitePath = Path.Combine(
            Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf(@"test\WebServices.AspNetCore.IntegrationTests", StringComparison.Ordinal)),
            @"src\WebServices.AspNetCore\");

        private IWebHostBuilder webHostBuilder = new WebHostBuilder()
            .UseWebRoot(websitePath)
            .UseContentRoot(websitePath)
            .UseStartup<Startup>();

        private readonly ConcurrentBag<IDisposable> toDispose = new ConcurrentBag<IDisposable>();

        public void Dispose()
        {
            IDisposable disposable;

            while (toDispose.TryTake(out disposable))
            {
                disposable.Dispose();
            }
        }

        public TestServerBuilder ConfigureServices(Action<IServiceCollection> configureServices)
        {
            webHostBuilder.ConfigureServices(configureServices);

            return this;
        }

        public TestServer Build()
        {
            var server = new TestServer(webHostBuilder);

            toDispose.Add(server);

            return server;
        }
    }
}