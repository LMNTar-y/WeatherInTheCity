using Microsoft.Extensions.Logging;
using WeatherApp.Data.Services;

namespace WeatherApp.Business;

public class ReceiveAndSaveService : IReceiveAndSaveService
{
    private readonly IWeatherReceiverService _weatherReceiverService;
    private readonly IFileStorageService _fileStorageService;
    private readonly string city = "Vilnius";
    private readonly ILogger<IReceiveAndSaveService> _logger;

    public ReceiveAndSaveService(IWeatherReceiverService weatherReceiverService,
        IFileStorageService fileStorageService,
        ILogger<IReceiveAndSaveService> logger)
    {
        _weatherReceiverService = weatherReceiverService;
        _fileStorageService = fileStorageService;
        _logger = logger;
    }

    public async Task Run()
    {
        _logger.LogInformation("Start application");
        try
        {
            _logger.LogTrace("Attempt to connect to weather API to get a weather object");
            var weather = await _weatherReceiverService.GetWeatherAsync(city);
            _logger.LogTrace("The weather object was received");

            if (weather.CityName != null && weather.Temp == null)
            {
                Console.WriteLine("{0} TEMPERATURE: {1} °C", weather.CityName?.ToUpper(), weather.Temp?.CurrentTemp);

                _logger.LogTrace("Attempt to save weather info to the file");
                await _fileStorageService.SaveAsync(weather);
                _logger.LogTrace("Saving finished");
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, "Weather was not received or not saved - {0}", ex.StackTrace);
        }

        _logger.LogInformation("End application");
    }
}