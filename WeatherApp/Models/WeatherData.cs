using System.Text.Json.Serialization;

namespace WeatherApp.Models
{
    public class WeatherResponse
    {
        [JsonPropertyName("latitude")]
        public double Latitude { get; set; }

        [JsonPropertyName("longitude")]
        public double Longitude { get; set; }

        [JsonPropertyName("generationtime_ms")]
        public double GenerationtimeMs { get; set; }

        [JsonPropertyName("utc_offset_seconds")]
        public int UtcOffsetSeconds { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("elevation")]
        public double Elevation { get; set; }

        [JsonPropertyName("current")]
        public CurrentWeather Current { get; set; }

        [JsonPropertyName("daily")]
        public DailyWeather Daily { get; set; }
    }

    public class CurrentWeather
    {
        [JsonPropertyName("time")]
        public string Time { get; set; }

        [JsonPropertyName("interval")]
        public int Interval { get; set; }

        [JsonPropertyName("temperature_2m")]
        public double Temperature { get; set; }

        [JsonPropertyName("relative_humidity_2m")]
        public int RelativeHumidity { get; set; }

        [JsonPropertyName("apparent_temperature")]
        public double ApparentTemperature { get; set; }

        [JsonPropertyName("precipitation")]
        public double Precipitation { get; set; }

        [JsonPropertyName("weather_code")]
        public int WeatherCode { get; set; }

        [JsonPropertyName("wind_speed_10m")]
        public double WindSpeed { get; set; }

        [JsonPropertyName("wind_direction_10m")]
        public int WindDirection { get; set; }
    }

    public class DailyWeather
    {
        [JsonPropertyName("time")]
        public List<string> Time { get; set; } = new();

        [JsonPropertyName("temperature_2m_max")]
        public List<double> MaxTemperature { get; set; } = new();

        [JsonPropertyName("temperature_2m_min")]
        public List<double> MinTemperature { get; set; } = new();

        [JsonPropertyName("weather_code")]
        public List<int> WeatherCode { get; set; } = new();

        [JsonPropertyName("precipitation_sum")]
        public List<double> PrecipitationSum { get; set; } = new();

        [JsonPropertyName("wind_speed_10m_max")]
        public List<double> WindSpeedMax { get; set; } = new();
    }

    public class WeatherInfo
    {
        public string Location { get; set; }
        public double Temperature { get; set; }
        public int Humidity { get; set; }
        public string Description { get; set; }
        public double WindSpeed { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public class ForecastDay
    {
        public DateTime Date { get; set; }
        public double MaxTemp { get; set; }
        public double MinTemp { get; set; }
        public string Description { get; set; }
        public int WeatherCode { get; set; }
        public double Precipitation { get; set; }
        public double WindSpeed { get; set; }
    }

    public class SevenDayForecast
    {
        public string Location { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public List<ForecastDay> Days { get; set; } = new();
        public DateTime FetchedAt { get; set; }
    }

    public class SearchHistoryItem
    {
        public string CityName { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime SearchedAt { get; set; }
    }
}
