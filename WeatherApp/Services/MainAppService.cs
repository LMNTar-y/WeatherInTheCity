
using System.Text.Json;
using WeatherApp.Model;

namespace WeatherApp.Services
{
    public class MainAppService : IMainAppService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IFileStorageService _fileStorageService;
        private readonly string city = "Vilnius";

        public MainAppService(IHttpClientService httpClientService, IFileStorageService fileStorageService)
        {
            _httpClientService = httpClientService;
            _fileStorageService = fileStorageService;
        }

        public async Task Run()
        {
            string content = await _httpClientService.GetStringAsync(city);
            WeatherInTheCity weather = JsonSerializer.Deserialize<WeatherInTheCity>(content);
            Console.WriteLine("{0} TEMPERATURE: {1} °C", weather?.CityName?.ToUpper(), weather?.Temp?.CurrentTemp);
            await _fileStorageService.SaveAsync(weather);
        }

    }
}
