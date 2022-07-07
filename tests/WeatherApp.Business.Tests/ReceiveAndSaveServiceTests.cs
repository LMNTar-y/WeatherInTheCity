using Microsoft.Extensions.Logging;
using Moq;
using WeatherApp.Data.Services;

namespace WeatherApp.Business.Tests
{
    public class ReceiveAndSaveServiceTests
    {
        private readonly Mock<ILogger<ReceiveAndSaveService>> _loggerMock = new();
        private readonly Mock<IFileStorageService> _fileStorageMock = new();
        private readonly Mock<IWeatherReceiverService> _weatherReceiverMock = new();

        [Fact(Skip = "Changed the targetClassm it does not throw an exception now")]
        public async Task Run_ReceivedNullWeatherObject_ThrowsArgumentNullException()
        {
            //arrange
            _weatherReceiverMock.Setup(c => c.GetWeatherAsync("")).ReturnsAsync(() => null);
            var sut = new ReceiveAndSaveService(_weatherReceiverMock.Object, _fileStorageMock.Object, _loggerMock.Object);
            //act
            //assert
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => sut.Run());
        }


        [Theory(Skip = "Changed the targetClassm it does not throw an exception now")]
        [InlineData("")]
        [InlineData("  ")]
        [InlineData(null)]
        public async Task Run_IncorrectCityParameter_ThrowsArgumentNullException(object city)
        {
            //arrange
            var sut = new ReceiveAndSaveService(_weatherReceiverMock.Object, _fileStorageMock.Object, _loggerMock.Object) {City = (string)city};
            //act
            //assert
            await Assert.ThrowsAnyAsync<ArgumentNullException>(() => sut.Run());
        }
    }
}