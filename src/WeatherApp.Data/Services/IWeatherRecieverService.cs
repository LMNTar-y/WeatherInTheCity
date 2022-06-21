using WeatherApp.Data.Model;

namespace WeatherApp.Data.Services
{
    public interface IWeatherRecieverService
    {
        Task<WeatherInTheCity> GetWeatherAsync(string city);
    }
}
