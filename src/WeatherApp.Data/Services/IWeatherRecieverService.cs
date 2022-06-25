using WeatherApp.Data.Model;

namespace WeatherApp.Data.Services
{
    public interface IWeatherReceiverService
    {
        Task<WeatherInTheCity> GetWeatherAsync(string city);
    }
}
