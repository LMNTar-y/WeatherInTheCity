using WeatherApp.Data.Model;

namespace WeatherApp.Data.Services
{
    public interface IFileStorageService
    {
        Task SaveAsync(WeatherInTheCity weather);
    }
}
