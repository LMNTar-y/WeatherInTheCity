﻿namespace WeatherApp.Services
{
    internal class HttpClientService : IHttpClientService
    {
        private readonly HttpClient _client;

        public HttpClientService(HttpClient client)
        {
            _client = client;
        }

        public async Task<string> GetStringAsync(string city) =>
            await _client.GetStringAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&units=metric&appid=707e48005e907492b0c924c484103ca8");
    }
}