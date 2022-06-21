using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherApp.Data.Model;

namespace WeatherApp.Data.Services
{
    public class WeatherRecieverService : IWeatherRecieverService
    {
        private readonly HttpClient _client;

        public WeatherRecieverService(HttpClient client, IOptions<PathToFileConfig> configurations)
        {
            _client = client;
            if (configurations.Value.Url != null)
                _client.BaseAddress = new Uri(configurations.Value.Url ?? throw new ArgumentNullException(nameof(configurations), "Incorrect data in the url section in the appsettings.json"));
        }

        public async Task<WeatherInTheCity> GetWeatherAsync(string city)
        {
            var responce = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + $"&q={city}"));
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
