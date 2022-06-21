using System.Text.Json.Serialization;

namespace WeatherApp.Model
{
    public class TempInfo
    {
        [JsonPropertyName("temp")]
        public double CurrentTemp { get; set; }
    }
}
