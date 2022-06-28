using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherApp.Data.Models;
using WeatherApp.Data.Services;

namespace WeatherApp.Data.Tests
{
    public class FileStorageServiceTests
    {
        private readonly WeatherInTheCity _weather = new()
        { CityName = "Vilnius", Temp = new TempInfo() { CurrentTemp = 22.22 } };


        [Fact]
        public async Task SaveAsync_StoragePassIsNull_ThrowsArgumentNullException()
        {
            //arrange

            FileStorageService fileStorageService =
                new FileStorageService(
                    new OptionsWrapper<PathToFileConfig>(new PathToFileConfig() { StoragePath = null }),
                    new Logger<FileStorageService>(new LoggerFactory()));

            //act
            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => fileStorageService.SaveAsync(_weather));
        }

        [Fact]
        public async Task SaveAsync_StoragePassIsEmpty_ThrowsArgumentException()
        {
            //arrange
            FileStorageService fileStorageService =
                new FileStorageService(
                    new OptionsWrapper<PathToFileConfig>(new PathToFileConfig() { StoragePath = string.Empty }),
                    new Logger<FileStorageService>(new LoggerFactory()));
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentException>(() => fileStorageService.SaveAsync(_weather));
        }

        [Fact]
        public async Task SaveAsync_StorageFolderDoesNotExists_ThrowsArgumentException()
        {
            //arrange
            FileStorageService fileStorageService =
                new FileStorageService(
                    new OptionsWrapper<PathToFileConfig>(new PathToFileConfig() { StoragePath = "Randomname/weatherapp-test-result.json" }),
                    new Logger<FileStorageService>(new LoggerFactory()));
            //act
            //assert
            await Assert.ThrowsAsync<DirectoryNotFoundException>(() => fileStorageService.SaveAsync(_weather));
        }

        [Fact]
        public async Task SaveAsync_StoragePassIsCorrect_TrueIfFileExistsAndTextEqualToTestObject()
        {
            //arrange
            string path = "../weatherapp-test-result.json";
            FileStorageService fileStorageService =
                new FileStorageService(
                    new OptionsWrapper<PathToFileConfig>(new PathToFileConfig() { StoragePath = path }),
                    new Logger<FileStorageService>(new LoggerFactory()));
            //act
            await fileStorageService.SaveAsync(_weather);

            //assert
            Assert.True(File.Exists(path));
            string result = JsonSerializer.Serialize(_weather);
            Assert.Equal(File.ReadAllText(path), result);            
        }
    }
}