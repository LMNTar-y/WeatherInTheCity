using WeatherApp.Data.Models;

namespace WeatherApp.Data.Services
{
    public interface IFileStorageService
    {
        Task SaveAsync(WeatherInTheCity weather);
    }
}
