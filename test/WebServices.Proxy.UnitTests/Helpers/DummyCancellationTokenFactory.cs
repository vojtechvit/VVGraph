using System.Collections.Generic;
using System.Threading;

namespace WebServices.Proxy.UnitTests.Helpers
{
    public static class DummyCancellationTokenFactory
    {
        public static IEnumerable<CancellationToken> GetValidCancellationTokens()
            => new[]
            {
                CreateNone(),
                CreateNotNone()
            };

        public static CancellationToken CreateNone()
            => CancellationToken.None;

        public static CancellationToken CreateNotNone()
            => new CancellationToken();
    }
}