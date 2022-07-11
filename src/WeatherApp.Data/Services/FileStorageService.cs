using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherApp.Data.Models;

namespace WeatherApp.Data.Services;

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
        try
        {
            _logger.LogTrace("Start checking StoragePass if it null, empty or whiteSpace");
            var storagePath = GetStoragePassOrThrowException(_configurations);
            _logger.LogTrace("Checking completed");

            _logger.LogTrace("Open streamWriter and start to write info to the file");

            await using var sw = new StreamWriter(storagePath, false);
            await sw.WriteAsync(JsonSerializer.Serialize(weather));

            _logger.LogTrace("The info has been written successfully");
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex,
                "Error with the serialization data from the Weather object and saving it to the file - {Adress}",
                ex.ToString());
            throw;
        }
    }

    private string GetStoragePassOrThrowException(PathToFileConfig configurations)
    {
        if (configurations == null || string.IsNullOrWhiteSpace(configurations.StoragePath))
            throw new ArgumentNullException(nameof(configurations),
                "PathToFileConfig object cannot be null or its StoragePath property cannot be null, empty or whiteSpace");

        return configurations.StoragePath;
    }
}