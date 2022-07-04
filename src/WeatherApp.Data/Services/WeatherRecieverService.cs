using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;
using WeatherApp.Data.Models;

namespace WeatherApp.Data.Services;

public class WeatherReceiverService : IWeatherReceiverService
{
    private readonly ILogger<WeatherReceiverService> _logger;
    private readonly HttpClient _httpClient;

    public WeatherReceiverService(IHttpClientFactory httpClientFactory,
        ILogger<WeatherReceiverService> logger)
    {
        _logger = logger;
        _httpClient = httpClientFactory.CreateClient("WeatherApp");
    }

    public async Task<WeatherInTheCity> GetWeatherAsync(string city)
    {
        _logger.LogTrace("Start checking City parameter if it null, empty or whiteSpace");
        CheckCityParameterToNullOrWhiteSpaced(city);
        _logger.LogTrace("Checking completed");

        WeatherInTheCity? weather;

        try
        {
            _logger.LogTrace("Send a request to API");
            var response = await _httpClient.SendAsync(
                new HttpRequestMessage(HttpMethod.Get, $"{_httpClient.BaseAddress}&q={city}"));
            _logger.LogTrace("The request succeed and response received");

            var jsonFromResponse = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogTrace("The response was received with an unsuccessful status code ");
                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        _logger.LogCritical("BadRequest - {0}", await response.Content.ReadAsStringAsync());
                        break;
                    case HttpStatusCode.Forbidden:
                        _logger.LogCritical("Forbidden - {0}", await response.Content.ReadAsStringAsync());
                        break;
                    case HttpStatusCode.InternalServerError:
                        _logger.LogCritical("InternalServerError - {0}", await response.Content.ReadAsStringAsync());
                        break;
                    case HttpStatusCode.NotFound:
                        _logger.LogCritical("NotFound - {0}", jsonFromResponse);
                        break;
                    default:
                        _logger.LogCritical("Other bad status - {0}", await response.Content.ReadAsStringAsync());
                        break;
                }

                throw new ArgumentException($"Server response StatusCode: {response.StatusCode}", nameof(city));
            }

            weather = JsonSerializer.Deserialize<WeatherInTheCity>(jsonFromResponse);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex,
                "Error with the response from server or with the deserialization data from the server to the Weather object - {0}",
                ex.ToString());
            throw;
        }

        return weather ?? throw new ArgumentNullException(nameof(city), "Weather cannot be null");
    }

    private void CheckCityParameterToNullOrWhiteSpaced(string city)
    {
        if (!string.IsNullOrWhiteSpace(city)) return;
        _logger.LogCritical("The city which was put in the method cannot be null, empty or whiteSpace");
        throw new ArgumentNullException(nameof(city),
            "The city which was put in the method cannot be null, empty or whiteSpace");
    }
}