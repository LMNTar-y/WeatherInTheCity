using Microsoft.Extensions.Logging;
using Moq;
using WeatherApp.Data.Services;
using WeatherApp.Data.Models;
using Microsoft.Extensions.Options;
using System.Net;
using Moq.Protected;

namespace WeatherApp.Data.Tests
{
    public class WeatherRecieverServiceTests
    {
        private WeatherReceiverService _sut;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock = new Mock<IHttpClientFactory>();
        private readonly Mock<ILogger<WeatherReceiverService>> _loggerMock = new Mock<ILogger<WeatherReceiverService>>();
        private readonly Mock<IOptions<PathToFileConfig>> _configurationsMock = new Mock<IOptions<PathToFileConfig>>();

        [Fact]
        public void Constructor_GetsNullUrlConfigurations_ThrowsArgumentNullException()
        {
            //arrange
            var pathToFileConfig = new PathToFileConfig() { Url = null };
            _configurationsMock.Setup(p => p.Value).Returns(pathToFileConfig);
            
            //act
            //accert            
            Assert.Throws<ArgumentNullException>(() => new WeatherReceiverService(_httpClientFactoryMock.Object, _configurationsMock.Object, _loggerMock.Object));
        }

        [Theory]
        [InlineData(HttpStatusCode.BadRequest)]
        [InlineData(HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.InternalServerError)]
        [InlineData(HttpStatusCode.NotFound)]
        [InlineData(HttpStatusCode.LoopDetected)]
        [InlineData(HttpStatusCode.BadGateway)]
        [InlineData(HttpStatusCode.ExpectationFailed)]
        public async Task GetWeatherAsync_ResponceStatusCodeIsNotSuccess_ThrowsExceptions(object httpStatusCode)
        {
            //arrange
            var pathToFileConfig = new PathToFileConfig() { Url = "https://test/" };
            _configurationsMock.Setup(x => x.Value).Returns(pathToFileConfig);

            var httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            var responce = new HttpResponseMessage((HttpStatusCode)httpStatusCode);

            httpMessageHandlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(() => responce);

            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(() => new HttpClient(httpMessageHandlerMock.Object));

            _sut = new WeatherReceiverService(_httpClientFactoryMock.Object, _configurationsMock.Object, _loggerMock.Object);

            //act
            //accert
            await Assert.ThrowsAnyAsync<ArgumentException>(() => _sut.GetWeatherAsync("Vilnius"));
        }

        [Fact]
        public async Task GetWeatherAsync_GetCorrectResponce_ReturnsResult()
        {
            //arrange
            var pathToFileConfig = new PathToFileConfig() { Url = "https://test/" };
            _configurationsMock.Setup(x => x.Value).Returns(pathToFileConfig);

            var weather = new WeatherInTheCity() { CityName = "Vilnius", Temp = new TempInfo() { CurrentTemp = 16.49 } };
            var handlerMock = new Mock<HttpMessageHandler>();
            var responce = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(@"{""main"":{ ""temp"":16.49},""name"":""Vilnius""}")
            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(() => responce);

            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(() => new HttpClient(handlerMock.Object));

            _sut = new WeatherReceiverService(_httpClientFactoryMock.Object, _configurationsMock.Object, _loggerMock.Object);

            //act
           var result = await _sut.GetWeatherAsync("dcs");

            //accert
            Assert.NotNull(result);
            Assert.Equal(weather.CityName, result.CityName);
            Assert.Equal(weather.Temp.CurrentTemp, result?.Temp?.CurrentTemp);
        }
    }
}
