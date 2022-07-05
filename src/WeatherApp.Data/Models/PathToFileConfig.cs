using System.Text.Json.Serialization;

namespace WeatherApp.Data.Models;

public class PathToFileConfig
{
    [JsonPropertyName("storagePath")] 
    public string? StoragePath { get; set; }
}