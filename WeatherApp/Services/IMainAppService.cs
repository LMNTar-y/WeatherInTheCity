
namespace WeatherApp.Services
{
    public interface IMainAppService
    {
        Task<string> GetStringAsync(string city);
    }
}
