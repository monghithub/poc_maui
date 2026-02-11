using WeatherApp.Models;

namespace WeatherApp.Services
{
    public interface IWeatherService
    {
        Task<WeatherInfo> GetWeatherByCityAsync(string city);
        Task<WeatherInfo> GetWeatherByCoordinatesAsync(double latitude, double longitude, string cityName);
        Task<SevenDayForecast> GetSevenDayForecastAsync(double latitude, double longitude, string cityName);
        Task<(double Latitude, double Longitude)?> GetCoordinatesByCityAsync(string city);
        Task<bool> RequestLocationPermissionAsync();
        Task<(double Latitude, double Longitude)?> GetCurrentLocationAsync();
    }
}
