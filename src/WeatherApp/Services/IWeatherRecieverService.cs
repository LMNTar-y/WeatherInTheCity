
using WeatherApp.Model;

namespace WeatherApp.Services
{
    public interface IWeatherRecieverService
    {
        Task<WeatherInTheCity> GetWeatherAsync(string city);
    }
}
