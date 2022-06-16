using System.Text.Json.Serialization;

namespace WeatherApp
{
    public class TempInfo
    {
        [JsonPropertyName("temp")]
        public double CurrentTemp { get; set; }
    }
}
