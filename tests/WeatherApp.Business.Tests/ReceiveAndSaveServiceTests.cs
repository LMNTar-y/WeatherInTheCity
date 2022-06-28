using Moq;
using Microsoft.Extensions.Logging;
using WeatherApp.Business.Services;
using WeatherApp.Data.Services;
using WeatherApp.Data.Models;

namespace WeatherApp.Business.Tests
{
    public class ReceiveAndSaveServiceTests
    {
        private readonly ReceiveAndSaveService _sut;
        private readonly Mock<IWeatherReceiverService> _weatherReceiverServiceMock = new Mock<IWeatherReceiverService>();
        private readonly Mock<IFileStorageService> _fileStorageServiceMock = new Mock<IFileStorageService>();
        private readonly Mock<ILogger<IReceiveAndSaveService>> _loggerMock = new Mock<ILogger<IReceiveAndSaveService>>();
        private readonly string city = "Vilnius";

        public ReceiveAndSaveServiceTests()
        {
            _sut = new ReceiveAndSaveService(_weatherReceiverServiceMock.Object, _fileStorageServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task RunAsync_GetNullWeather_ThrowsArgumentNullException()
        {
            //arrange
            _weatherReceiverServiceMock.Setup(x => x.GetWeatherAsync(city)).ReturnsAsync(() => null);

            //act
            //accert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.Run());
        }
    }
}