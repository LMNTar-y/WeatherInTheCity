using WeatherApp.Data.Model;
using WeatherApp.Data.Services;

namespace WeatherApp.Business.Services
{
    public class ReceiveAndSaveService : IReceiveAndSaveService
    {
        private readonly IWeatherRecieverService _weatherRecieverService;
        private readonly IFileStorageService _fileStorageService;
        private readonly string city = "Vilnius";

        public ReceiveAndSaveService(IWeatherRecieverService weatherRecieverService, IFileStorageService fileStorageService)
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
                await _fileStorageService.SaveAsync(weather);                
            }
        }
    }
}
