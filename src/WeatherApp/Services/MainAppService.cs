
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
            WeatherInTheCity? weather;
            try
            {
                weather = JsonSerializer.Deserialize<WeatherInTheCity>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            Console.WriteLine("{0} TEMPERATURE: {1} °C", weather?.CityName?.ToUpper(), weather?.Temp?.CurrentTemp);
            if (weather != null)
            {
                await _fileStorageService.SaveAsync(weather);
            }
        }
    }
}
