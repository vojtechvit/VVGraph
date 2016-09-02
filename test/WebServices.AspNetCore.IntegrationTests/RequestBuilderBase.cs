using WebServices.AspNetCore.IntegrationTests.Fixtures;

namespace WebServices.AspNetCore.IntegrationTests
{
    public abstract class RequestBuilderBase
    {
        protected RequestBuilderBase(TestServerBuilder serverBuilder)
        {
            if (serverBuilder == null)
                throw new System.ArgumentNullException(nameof(serverBuilder));

            ServerBuilder = serverBuilder;
        }

        protected TestServerBuilder ServerBuilder { get; }
    }
}