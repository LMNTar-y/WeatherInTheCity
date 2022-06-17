using Microsoft.Extensions.Options;
using WeatherApp.Model;

namespace WeatherApp.Services
{
    internal class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;

        public HttpClientService(HttpClient client, IOptions<PathToFileConfig> configurations)
        {
            _client = client;
            if (configurations.Value.Url != null)
                _client.BaseAddress = new Uri(configurations.Value.Url ?? throw new ArgumentNullException(nameof(configurations), "Incorrect data in the url section in the appsettings.json"));
        }

        public async Task<string> GetStringAsync(string city)
        {
            var responce = await _client.SendAsync(new HttpRequestMessage(HttpMethod.Get, _client.BaseAddress + $"&q={city}"));
            if (!responce.IsSuccessStatusCode)
            {
                throw new ArgumentException($"Server responce StatusCode: {responce.StatusCode}", nameof(city));
            }

            return await responce.Content.ReadAsStringAsync();
        }
    }
}
