using Microsoft.Extensions.Options;
using WeatherApp.Data.Models;
using WeatherApp.Data.Services;

namespace WeatherApp.Data.Tests
{
    public class FileStorageServiceTests
    {
        [Fact]
        public async Task SaveAsync_StoragePassIsNull_ThrowsArgumentNullException()
        {
            //arrange
            WeatherInTheCity? weather = new WeatherInTheCity() { CityName = "Vilnius", Temp = new TempInfo(){CurrentTemp = 22.22} };
            FileStorageService fileStorageService = 
                new FileStorageService(new OptionsWrapper<PathToFileConfig>(new PathToFileConfig(){StoragePath = null}), 
                    null);
            //act
            //assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => fileStorageService.SaveAsync(weather));
        }
    }
}