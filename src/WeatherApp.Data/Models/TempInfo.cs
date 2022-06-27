using System.Text.Json.Serialization;

namespace WeatherApp.Data.Models
{
    public class TempInfo
    {
        [JsonPropertyName("temp")]
        public double CurrentTemp { get; set; }
    }
}
