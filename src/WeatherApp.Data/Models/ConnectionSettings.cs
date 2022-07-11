using System.Text.Json.Serialization;

namespace WeatherApp.Data.Models;

public class ConnectionSettings
{
    [JsonPropertyName("url")]
    public string? Url { get; set; }
}