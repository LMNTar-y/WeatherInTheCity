
using System.Text.Json;
using WeatherApp.Model;

namespace WeatherApp.Services
{
    public class MainAppService : IMainAppService
    {
        private readonly IWeatherRecieverService _weatherRecieverService;
        private readonly IFileStorageService _fileStorageService;
        private readonly string city = "Vilnius";

        public MainAppService(IWeatherRecieverService weatherRecieverService, IFileStorageService fileStorageService)
        {
            _weatherRecieverService = weatherRecieverService;
            _fileStorageService = fileStorageService;
        }

        public async Task Run()
        {
            WeatherInTheCity? weather;
            try
            {
                weather = await _weatherRecieverService.GetWeatherAsync(city);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            
            if (weather == null || weather.CityName == null || weather.Temp == null)
            {
                Console.WriteLine("Weather is not found");
            }
            else
            {
                Console.WriteLine("{0} TEMPERATURE: {1} °C", weather.CityName.ToUpper(), weather.Temp.CurrentTemp);
                if (weather != null)
                {
                    await _fileStorageService.SaveAsync(weather);
                }
            }
        }
    }
}
