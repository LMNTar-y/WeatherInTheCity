
using WeatherApp.Model;

namespace WeatherApp.Services
{
    public interface IFileStorageService
    {
        Task SaveAsync(WeatherInTheCity weather);
    }
}
