﻿using Microsoft.Extensions.Logging;
using WeatherApp.Data.Services;

namespace WeatherApp.Business;

public class ReceiveAndSaveService : IReceiveAndSaveService
{
    private readonly IWeatherReceiverService _weatherReceiverService;
    private readonly IFileStorageService _fileStorageService;
    private readonly ILogger<IReceiveAndSaveService> _logger;

    public string City { get; set; } = "Vilnius";

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
            do
            {
                Console.WriteLine("Please enter the name of the city");
                var tempCity = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(tempCity))
                {
                    City = tempCity;
                    break;
                }
            } 
            while (true);

            _logger.LogTrace("Attempt to connect to weather API to get a weather object");
            var weather = await _weatherReceiverService.GetWeatherAsync(City);
            _logger.LogTrace("The weather object was received");

            _logger.LogTrace("Start checking weather object properties on null");
            if (!string.IsNullOrWhiteSpace(weather?.CityName) && weather?.Temp != null)
            {
                Console.WriteLine("{0} TEMPERATURE: {1} °C", weather.CityName.ToUpper(), weather.Temp.CurrentTemp);

                _logger.LogTrace("Attempt to save weather info to the file");
                await _fileStorageService.SaveAsync(weather);
                _logger.LogTrace("Saving finished");
            }
            else
            {
                _logger.LogCritical($"For the request value {City} weather response object validation has failed");
                throw new ArgumentNullException(City, $"For the request value {City} weather response object validation has failed");
            }

            _logger.LogTrace(
                $"Complete checking weather object properties: CityName - {weather.CityName}, Temp - {weather.Temp.CurrentTemp}");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex, $"Weather was not received or not saved. Message - {ex.Message}, StackTrace - {ex.StackTrace} ");
        }

        _logger.LogInformation("End application");
    }
}