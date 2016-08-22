using Moq;
using System;
using WebServices.Proxy.Contracts;

namespace WebServices.Proxy.UnitTests.VVGraphClient
{
    public abstract class VVGraphClientTests
    {
        protected readonly VVGraphClientConfiguration configuration;

        protected readonly Mock<IJsonSerializer> jsonSerializerMock;

        protected readonly Mock<IUrlHelper> urlHelperMock;

        protected readonly Mock<IHttpClient> httpClientMock;

        protected readonly IVVGraphClient client;

        protected VVGraphClientTests()
        {
            // Stubs
            configuration = new VVGraphClientConfiguration
            {
                BaseUrl = new Uri("http://www.example.org")
            };

            // Mocks
            jsonSerializerMock = new Mock<IJsonSerializer>(MockBehavior.Strict);
            urlHelperMock = new Mock<IUrlHelper>(MockBehavior.Strict);
            httpClientMock = new Mock<IHttpClient>(MockBehavior.Strict);

            // Subject under test
            client = new Proxy.VVGraphClient(configuration, httpClientMock.Object, jsonSerializerMock.Object, urlHelperMock.Object);
        }
    }
}