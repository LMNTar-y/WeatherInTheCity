using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;
using WeatherApp.Data.Model;

namespace WeatherApp.Data.Services
{
    public class WeatherRecieverService : IWeatherRecieverService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PathToFileConfig _configurations;
        private readonly ILogger<WeatherRecieverService> _logger;

        public WeatherRecieverService(IHttpClientFactory httpClientFactory, IOptions<PathToFileConfig> configurations, ILogger<WeatherRecieverService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _configurations = configurations.Value;
            _logger = logger;
        }

        public async Task<WeatherInTheCity> GetWeatherAsync(string city)
        {
            var httpClient = _httpClientFactory.CreateClient();
            if (_configurations.Url != null)
                httpClient.BaseAddress = new Uri(_configurations.Url ?? throw new ArgumentNullException(nameof(city), "Incorrect data in the url section in the appsettings.json"));

            HttpResponseMessage responce;
            try
            {
                responce = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, httpClient.BaseAddress + $"&q={city}"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Issue with sending request - {Adress}", ex.ToString());
                throw;
            }

            switch (responce.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    throw new ArgumentException($"{responce.StatusCode} - The server cannot or will not process the request due to an apparent client error", nameof(city));
                case HttpStatusCode.Forbidden:
                    throw new ArgumentException($"{responce.StatusCode} - The request contained valid data and was understood by the server, but the server is refusing action. " +
                        $"Possible because of the lack of permissions", nameof(city));
                case HttpStatusCode.InternalServerError:
                    throw new ArgumentException($"{responce.StatusCode} - Unexpected condition was encountered", nameof(city));                    
                case HttpStatusCode.NotFound:
                    throw new ArgumentException($"{responce.StatusCode} - The requested resource could not be found but may be available in the future. Subsequent requests by the client are permissible.", nameof(city));
            }

            if (!responce.IsSuccessStatusCode || responce == null)
            {
                throw new ArgumentException($"Server responce StatusCode: {responce?.StatusCode}", nameof(city));
            }

            string json = await responce.Content.ReadAsStringAsync();
            WeatherInTheCity? weather;
            try
            {
                weather = JsonSerializer.Deserialize<WeatherInTheCity>(json);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error with the deserialisation data from the server to the Weater object - {Adress}", ex.ToString());
                throw;
            }

            return weather;
        }
    }
}
