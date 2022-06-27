using WeatherApp.Data.Models;

namespace WeatherApp.Data.Services
{
    public interface IWeatherReceiverService
    {
        Task<WeatherInTheCity> GetWeatherAsync(string city);
    }
}
