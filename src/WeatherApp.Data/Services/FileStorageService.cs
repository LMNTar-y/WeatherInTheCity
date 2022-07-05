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
        _logger.LogTrace("Start checking StoragePass if it null, empty or whiteSpace");
        if (IfStoragePassIsNullOrWhiteSpaced(_configurations)) return;
        _logger.LogTrace("Checking completed");

        try
        {
            if (!string.IsNullOrWhiteSpace(_configurations?.StoragePath))
            {
                _logger.LogTrace("Open streamWriter and start to write info to the file");

                await using var sw = new StreamWriter(_configurations.StoragePath, false);
                await sw.WriteAsync(JsonSerializer.Serialize(weather));

                _logger.LogTrace("The info has been written successfully");
            }
            else
            {
                _logger.LogCritical("The storagepass in the appsettings file is null, empty or whiteSpace");
            }
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex,
                "Error with the serialization data from the Weather object and saving it to the file - {Adress}",
                ex.ToString());
            throw;
        }
    }

    private bool IfStoragePassIsNullOrWhiteSpaced(PathToFileConfig storagePath)
    {
        if (string.IsNullOrWhiteSpace(storagePath?.StoragePath))
            throw new ArgumentNullException(storagePath?.StoragePath,
                "StoragePath cannot be null, empty or whiteSpace");
        return false;
    }
}