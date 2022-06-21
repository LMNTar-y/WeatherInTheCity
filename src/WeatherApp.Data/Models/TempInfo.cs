using System.Text.Json.Serialization;

namespace WeatherApp.Data.Model
{
    public class TempInfo
    {
        [JsonPropertyName("temp")]
        public double CurrentTemp { get; set; }
    }
}
