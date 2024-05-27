using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Todo.Services;
using Xunit;

namespace Todo.Tests.Services
{
    public class GravatarTests
    {
        private readonly Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private readonly HttpClient _httpClient;
        private readonly Gravatar _gravatarService;

        public GravatarTests()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_mockHttpMessageHandler.Object);
            _gravatarService = new Gravatar(_httpClient);
        }

        [Fact]
        public async Task GetGravatarProfileAsync_ReturnsProfile_OnSuccess()
        {
            // Arrange
            var email = "user@example.com";
            var hash = Gravatar.GetHash(email);
            var responseContent = "{\"entry\":[{\"displayName\":\"Test User\"}]}";

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(responseContent),
                });

            // Act
            var result = await _gravatarService.GetGravatarProfileAsync(email);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result.Entry);
            Assert.Equal("Test User", result.Entry[0].DisplayName);
        }

        [Fact]
        public async Task GetGravatarProfileAsync_ReturnsNull_OnNonSuccessStatusCode()
        {
            // Arrange
            var email = "user@example.com";
            var hash = Gravatar.GetHash(email);

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.NotFound,
                });

            // Act
            var result = await _gravatarService.GetGravatarProfileAsync(email);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetGravatarProfileAsync_ReturnsNull_OnException()
        {
            // Arrange
            var email = "user@example.com";
            var hash = Gravatar.GetHash(email);

            _mockHttpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Request error"));

            // Act
            var result = await _gravatarService.GetGravatarProfileAsync(email);

            // Assert
            Assert.Null(result);
        }
    }
}
