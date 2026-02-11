using System.Text.Json;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private const string OpenMeteoBaseUrl = "https://api.open-meteo.com/v1/forecast";
        private const string NominatimBaseUrl = "https://nominatim.openstreetmap.org/search";

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WeatherInfo> GetWeatherByCityAsync(string city)
        {
            try
            {
                // First, get coordinates from city name using Nominatim
                var coordinates = await GetCoordinatesByCityAsync(city);
                if (coordinates == null)
                    throw new Exception($"City '{city}' not found");

                return await GetWeatherByCoordinatesAsync(coordinates.Value.Latitude, coordinates.Value.Longitude, city);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting weather: {ex.Message}", ex);
            }
        }

        public async Task<WeatherInfo> GetWeatherByCoordinatesAsync(double latitude, double longitude, string cityName)
        {
            try
            {
                var url = $"{OpenMeteoBaseUrl}?latitude={latitude}&longitude={longitude}&current=temperature_2m,relative_humidity_2m,apparent_temperature,precipitation,weather_code,wind_speed_10m,wind_direction_10m&timezone=auto";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(content, options);

                if (weatherResponse?.Current == null)
                    throw new Exception("Invalid weather data received");

                return new WeatherInfo
                {
                    Location = cityName,
                    Temperature = weatherResponse.Current.Temperature,
                    Humidity = weatherResponse.Current.RelativeHumidity,
                    Description = GetWeatherDescription(weatherResponse.Current.WeatherCode),
                    WindSpeed = weatherResponse.Current.WindSpeed,
                    UpdatedAt = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting weather by coordinates: {ex.Message}", ex);
            }
        }

        private async Task<(double Latitude, double Longitude)?> GetCoordinatesByCityAsync(string city)
        {
            try
            {
                var url = $"{NominatimBaseUrl}?q={Uri.EscapeDataString(city)}&format=json&limit=1";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var results = JsonSerializer.Deserialize<JsonElement>(content, options);

                if (results.ValueKind == JsonValueKind.Array && results.GetArrayLength() > 0)
                {
                    var firstResult = results[0];
                    if (double.TryParse(firstResult.GetProperty("lat").GetString(), out var lat) &&
                        double.TryParse(firstResult.GetProperty("lon").GetString(), out var lon))
                    {
                        return (lat, lon);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error getting coordinates: {ex.Message}", ex);
            }
        }

        private string GetWeatherDescription(int weatherCode)
        {
            return weatherCode switch
            {
                0 => "Clear sky",
                1 or 2 => "Mostly clear",
                3 => "Overcast",
                45 or 48 => "Foggy",
                51 or 53 or 55 => "Light rain",
                61 or 63 or 65 => "Rain",
                71 or 73 or 75 => "Snow",
                77 => "Snow grains",
                80 or 81 or 82 => "Rain showers",
                85 or 86 => "Snow showers",
                95 or 96 or 99 => "Thunderstorm",
                _ => "Unknown"
            };
        }
    }
}
