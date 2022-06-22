using Microsoft.Extensions.Logging;
using WeatherApp.Data.Model;
using WeatherApp.Data.Services;

namespace WeatherApp.Business.Services
{
    public class ReceiveAndSaveService : IReceiveAndSaveService
    {
        private readonly IWeatherRecieverService _weatherRecieverService;
        private readonly IFileStorageService _fileStorageService;
        private readonly string city = "Vilnius";
        private readonly ILogger<IReceiveAndSaveService> _logger;

        public ReceiveAndSaveService(IWeatherRecieverService weatherRecieverService, IFileStorageService fileStorageService, ILogger<IReceiveAndSaveService> logger)
        {
            _weatherRecieverService = weatherRecieverService;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task Run()
        {
            _logger.LogInformation("Start application");
            WeatherInTheCity? weather;
            try
            {
                _logger.LogTrace("Attempt to connect to weather API to get a weather object");
                weather = await _weatherRecieverService.GetWeatherAsync(city);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "Connection was not established - {Adress}", ex.ToString());
                return;
            }

            _logger.LogTrace("The weather object was received");

            if (weather == null || weather.CityName == null || weather.Temp == null)
            {
                _logger.LogError("One or more weather properties is null");
            }
            else
            {
                Console.WriteLine("{0} TEMPERATURE: {1} °C", weather.CityName.ToUpper(), weather.Temp.CurrentTemp);
                try
                {
                    _logger.LogTrace("Attempt to save weather info to the file");
                    await _fileStorageService.SaveAsync(weather);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Weather info was not saved a file - {Adress}", ex.StackTrace);
                }
            
            }

            _logger.LogInformation("End application");
        }
    }
}
