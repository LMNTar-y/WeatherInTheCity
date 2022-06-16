
using WeatherApp.Model;

namespace WeatherApp.Services
{
    public class MainAppService : IMainAppService
    {
        private readonly IHttpClientService _httpClientService;
        private readonly IFileStorageService _fileStorageService;

        public MainAppService(IHttpClientService httpClientService, IFileStorageService fileStorageService)
        {
            _httpClientService = httpClientService;
            _fileStorageService = fileStorageService;
        }

        public async Task<string> GetStringAsync(string city) => await _httpClientService.GetStringAsync(city);
        public async Task SaveAsync(WeatherInTheCity weather) => await _fileStorageService.SaveAsync(weather);

    }
}
