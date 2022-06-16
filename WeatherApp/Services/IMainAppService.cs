
using WeatherApp.Model;

namespace WeatherApp.Services
{
    public interface IMainAppService
    {
        Task<string> GetStringAsync(string city);
        Task SaveAsync(WeatherInTheCity weather);
    }
}
