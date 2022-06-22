using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherApp.Data.Model;

namespace WeatherApp.Data.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly PathToFileConfig _configurations;

        public FileStorageService(IOptions<PathToFileConfig> configurations)
        {
            _configurations = configurations.Value;
        }

        public async Task SaveAsync(WeatherInTheCity weather)
        {
            if (_configurations.StoragePath != null)
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(_configurations.StoragePath, false))
                    {
                        await sw.WriteAsync(JsonSerializer.Serialize(weather));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    throw;
                }
            }
            else
            {
                throw new ArgumentNullException(nameof(weather), "Storagepass in appsettings is null");
            }
        }
    }
}
