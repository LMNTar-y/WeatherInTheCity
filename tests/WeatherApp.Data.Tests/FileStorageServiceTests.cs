using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherApp.Data.Models;
using WeatherApp.Data.Services;
using Moq;

namespace WeatherApp.Data.Tests
{
    public class FileStorageServiceTests
    {
        private FileStorageService? _sut;
        private readonly Mock<ILogger<FileStorageService>> _loggerMock = new Mock<ILogger<FileStorageService>>();
        private readonly Mock<IOptions<PathToFileConfig>> _configurationsMock = new Mock<IOptions<PathToFileConfig>>();

        private readonly WeatherInTheCity _weather = new()
        { CityName = "Vilnius", Temp = new TempInfo() { CurrentTemp = 22.22 } };


        [Fact]
        public async Task SaveAsync_StoragePassIsNull_ThrowsArgumentNullException()
        {
            //arrange
            var pathToFileConfig = new PathToFileConfig() { StoragePath = null };
            _configurationsMock.Setup(p => p.Value).Returns(pathToFileConfig);

            _sut = new FileStorageService(_configurationsMock.Object, _loggerMock.Object);

            //act
            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => _sut.SaveAsync(_weather));
        }

        [Fact]
        public async Task SaveAsync_StoragePassIsEmpty_ThrowsArgumentException()
        {
            //arrange
            var pathToFileConfig = new PathToFileConfig() { StoragePath = string.Empty };
            _configurationsMock.Setup(p => p.Value).Returns(pathToFileConfig);

            _sut = new FileStorageService(_configurationsMock.Object, _loggerMock.Object);
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(() => _sut.SaveAsync(_weather));
        }

        [Fact]
        public async Task SaveAsync_StorageFolderDoesNotExists_ThrowsArgumentException()
        {
            //arrange
            var pathToFileConfig = new PathToFileConfig() { StoragePath = "Randomname/weatherapp-test-result.json" };
            _configurationsMock.Setup(p => p.Value).Returns(pathToFileConfig);

            _sut = new FileStorageService(_configurationsMock.Object, _loggerMock.Object);
            //act
            //assert
            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => _sut.SaveAsync(_weather));
        }

        [Fact]
        public async Task SaveAsync_StoragePassIsCorrect_TrueIfFileExistsAndTextEqualToTestObject()
        {
            //arrange
            string path = "../weatherapp-result.json";
            var pathToFileConfig = new PathToFileConfig() { StoragePath = path };
            _configurationsMock.Setup(p => p.Value).Returns(pathToFileConfig);

            _sut = new FileStorageService(_configurationsMock.Object, _loggerMock.Object);
            //act
            await _sut.SaveAsync(_weather);

            //assert
            Assert.True(File.Exists(path));
            string result = JsonSerializer.Serialize(_weather);
            Assert.Equal(File.ReadAllText(path), result);            
        }
    }
}