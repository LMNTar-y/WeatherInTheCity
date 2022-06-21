
namespace WeatherApp.Services
{
    public interface IHttpClientService
    {
        Task<string> GetStringAsync(string city);
    }
}
