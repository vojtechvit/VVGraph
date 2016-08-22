using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WebServices.ApiModel;
using WebServices.Proxy.UnitTests.Helpers;
using Xunit;

namespace WebServices.Proxy.UnitTests.VVGraphClient
{
    public class PutGraphAsync : VVGraphClientTests
    {
        //// Mock call outputs

        protected readonly string jsonSerializerSerializeOutput = "{ test: 1 }";

        protected readonly Uri urlHelperGetGraphUrlOutput = new Uri("http://www.example.org/graph-url");

        protected readonly Task<HttpResponseMessage> httpClientPutAsyncOutput = Task.FromResult(new HttpResponseMessage(HttpStatusCode.NoContent));

        //// Tested method valid inputs

        public PutGraphAsync()
        {
            //// Arrange valid mock setup

            jsonSerializerMock
                .Setup(m => m.Serialize(It.IsAny<object>()))
                .Returns(jsonSerializerSerializeOutput);

            urlHelperMock
                .Setup(m => m.GetGraphUrl(It.IsAny<Uri>(), It.IsAny<string>()))
                .Returns(urlHelperGetGraphUrlOutput);

            httpClientMock
                .Setup(m => m.PutAsync(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<Encoding>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Returns(httpClientPutAsyncOutput);
        }

        public static IEnumerable<object[]> GetValidInputs()
            => GetValidInputs(null);

        public static IEnumerable<object[]> GetValidInputs(bool[] mask)
            => VVEnumerable.Permute(
                mask == null || mask[0] ? DummyGraphFactory.GetValidGraphs() : null,
                mask == null || mask[1] ? DummyCancellationTokenFactory.GetValidCancellationTokens() : null);

        [Theory]
        [MemberData(nameof(GetValidInputs))]
        public async Task ValidMocksAndInput_ValidOutputValidMocks(
            Graph graph,
            CancellationToken cancellationToken)
        {
            //// 1. Arrange

            // 1.1 Mocks

            // 1.2 Input

            //// 2. Act
            await client.PutGraphAsync(graph, cancellationToken);

            //// 3. Assert

            // 3.1 Output

            // 3.2 Mocks
            jsonSerializerMock
                .Verify(m => m.Serialize(graph), Times.Once);

            urlHelperMock
                .Verify(m => m.GetGraphUrl(configuration.BaseUrl, graph.Name));

            httpClientMock.Verify(m => m.PutAsync(urlHelperGetGraphUrlOutput, jsonSerializerSerializeOutput, Encoding.UTF8, "application/json", cancellationToken));
        }

        [Theory]
        [MemberData(nameof(GetValidInputs), new[] { false, true })]
        public async Task GraphNull_ArgumentNullException(
            CancellationToken cancellationToken)
        {
            //// 1. Arrange

            // 1.1 Mocks

            // 1.2 Input
            Graph graph = null;

            //// 2. Act

            Func<Task> call = () => client.PutGraphAsync(graph, cancellationToken);

            //// 3. Assert

            // 3.1 Output
            await Assert.ThrowsAsync<ArgumentNullException>("graph", call);

            // 3.2 Mocks
        }

        [Theory]
        [MemberData(nameof(GetValidInputs))]
        public async Task JsonSerializerThrowsException_ExceptionWrappedAndRethrown(
            Graph graph,
            CancellationToken cancellationToken)
        {
            //// 1. Arrange

            // 1.1 Mocks
            var thrownException = new Exception();

            jsonSerializerMock
                .Setup(m => m.Serialize(It.IsAny<object>()))
                .Throws(thrownException);

            // 1.2 Input

            //// 2. Act

            Func<Task> call = () => client.PutGraphAsync(graph, cancellationToken);

            //// 3. Assert

            // 3.1 Output
            var exception = await Assert.ThrowsAsync<VVGraphClientException>(call);
            Assert.Equal(exception.Data["GraphName"], graph.Name);
            Assert.StrictEqual(thrownException, exception.InnerException);

            // 3.2 Mocks
        }

        [Theory]
        [MemberData(nameof(GetValidInputs))]
        public async Task UrlHelperThrowsException_ExceptionWrappedAndRethrown(
            Graph graph,
            CancellationToken cancellationToken)
        {
            //// 1. Arrange

            // 1.1 Mocks
            var thrownException = new Exception();

            urlHelperMock
                .Setup(m => m.GetGraphUrl(It.IsAny<Uri>(), It.IsAny<string>()))
                .Throws(thrownException);

            // 1.2 Input

            //// 2. Act

            Func<Task> call = () => client.PutGraphAsync(graph, cancellationToken);

            //// 3. Assert

            // 3.1 Output
            var exception = await Assert.ThrowsAsync<VVGraphClientException>(call);
            Assert.Equal(exception.Data["GraphName"], graph.Name);
            Assert.StrictEqual(thrownException, exception.InnerException);

            // 3.2 Mocks
        }

        [Theory]
        [MemberData(nameof(GetValidInputs))]
        public async Task HttpClientThrowsException_ExceptionWrappedAndRethrown(
            Graph graph,
            CancellationToken cancellationToken)
        {
            //// 1. Arrange

            // 1.1 Mocks
            var thrownException = new Exception();

            httpClientMock
                .Setup(m => m.PutAsync(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<Encoding>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(thrownException);

            // 1.2 Input

            //// 2. Act

            Func<Task> call = () => client.PutGraphAsync(graph, cancellationToken);

            //// 3. Assert

            // 3.1 Output
            var exception = await Assert.ThrowsAsync<VVGraphClientException>(call);
            Assert.Equal(exception.Data["GraphName"], graph.Name);
            Assert.StrictEqual(thrownException, exception.InnerException);

            // 3.2 Mocks
        }

        [Theory]
        [MemberData(nameof(GetValidInputs))]
        public async Task HttpClientThrowsTaskCanceledException_ExceptionRethrown(
            Graph graph,
            CancellationToken cancellationToken)
        {
            //// 1. Arrange

            // 1.1 Mocks
            var thrownException = new TaskCanceledException();

            httpClientMock
                .Setup(m => m.PutAsync(It.IsAny<Uri>(), It.IsAny<string>(), It.IsAny<Encoding>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .Throws(thrownException);

            // 1.2 Input

            //// 2. Act

            Func<Task> call = () => client.PutGraphAsync(graph, cancellationToken);

            //// 3. Assert

            // 3.1 Output
            var exception = await Assert.ThrowsAsync<TaskCanceledException>(call);
            Assert.StrictEqual(thrownException, exception);

            // 3.2 Mocks
        }
    }
}