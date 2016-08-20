using DataAccess.Neo4j.Contracts;
using Microsoft.Extensions.Options;
using Neo4j.Driver.V1;
using System;

namespace DataAccess.Neo4j
{
    public sealed class Neo4jDriver : INeo4jDriver, IDisposable
    {
        private readonly IDriver driver;

        private bool disposed;

        public Neo4jDriver(
            IOptions<Neo4jConfiguration> options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.driver = GraphDatabase.Driver(
                options.Value.Uri,
                AuthTokens.Basic(options.Value.Username, options.Value.Password));
        }

        public ISession Session() => driver.Session();

        public void Dispose()
        {
            if (!disposed)
            {
                this.Dispose();
                disposed = true;
            }
        }
    }
}