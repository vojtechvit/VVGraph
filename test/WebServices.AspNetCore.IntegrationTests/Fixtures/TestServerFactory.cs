using System;
using System.Collections.Concurrent;

namespace WebServices.AspNetCore.IntegrationTests.Fixtures
{
    public sealed class TestServerFactory : IDisposable
    {
        private readonly ConcurrentBag<IDisposable> toDispose = new ConcurrentBag<IDisposable>();

        public void Dispose()
        {
            IDisposable disposable;

            while (toDispose.TryTake(out disposable))
            {
                disposable.Dispose();
            }
        }

        public TestServerBuilder Create()
        {
            var builder = new TestServerBuilder();

            toDispose.Add(builder);

            return builder;
        }
    }
}