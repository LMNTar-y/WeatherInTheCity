using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherApp.Data.Model;

namespace WeatherApp.Data.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly PathToFileConfig _configurations;
        private readonly ILogger<FileStorageService> _logger;

        public FileStorageService(IOptions<PathToFileConfig> configurations, 
            ILogger<FileStorageService> logger)
        {
            _configurations = configurations.Value;
            _logger = logger;
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
                    _logger.LogError(ex, 
                        "Error with the serialization data from the Weather object and saving it to the file - {Adress}", 
                        ex.ToString());
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
