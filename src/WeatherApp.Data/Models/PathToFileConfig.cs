using System.Text.Json.Serialization;

namespace WeatherApp.Data.Model
{
    public class PathToFileConfig
    {
        [JsonPropertyName("storagePath")]
        public string? StoragePath { get; set; }
        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}
