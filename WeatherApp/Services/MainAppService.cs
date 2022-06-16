
using WeatherApp.Model;

namespace WeatherApp.Services
{
    internal class MainAppService
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
