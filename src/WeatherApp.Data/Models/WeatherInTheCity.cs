﻿using System.Text.Json.Serialization;

namespace WeatherApp.Data.Models
{
    public class WeatherInTheCity
    {
        [JsonPropertyName("main")]
        public TempInfo? Temp { get; set; }

        [JsonPropertyName("name")]
        public string? CityName { get; set; }
    }
}
