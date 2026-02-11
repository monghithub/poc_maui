using WeatherApp.Models;

namespace WeatherApp.Services
{
    public interface IWeatherService
    {
        Task<WeatherInfo> GetWeatherByCityAsync(string city);
        Task<WeatherInfo> GetWeatherByCoordinatesAsync(double latitude, double longitude, string cityName);
    }
}
