using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherApp.Data.Model;

namespace WeatherApp.Data.Services
{
    public class WeatherRecieverService : IWeatherRecieverService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly PathToFileConfig _configurations;

        public WeatherRecieverService(IHttpClientFactory httpClientFactory, IOptions<PathToFileConfig> configurations)
        {
            _httpClientFactory = httpClientFactory;
            _configurations = configurations.Value;
        }

        public async Task<WeatherInTheCity> GetWeatherAsync(string city)
        {
            var httpClient = _httpClientFactory.CreateClient();
            if (_configurations.Url != null)
                httpClient.BaseAddress = new Uri(_configurations.Url ?? throw new ArgumentNullException(nameof(city), "Incorrect data in the url section in the appsettings.json"));

            var responce = await httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, httpClient.BaseAddress + $"&q={city}"));
            if (!responce.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Server responce StatusCode: {responce.StatusCode}", nameof(city));
            }

            string json = await responce.Content.ReadAsStringAsync();
            WeatherInTheCity? weather;
            try
            {
                weather = JsonSerializer.Deserialize<WeatherInTheCity>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return weather;
        }
    }
}
