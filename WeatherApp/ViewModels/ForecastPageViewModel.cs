using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public partial class ForecastPageViewModel : ObservableObject
    {
        private readonly IWeatherService _weatherService;
        private double _latitude;
        private double _longitude;
        private string _cityName;

        [ObservableProperty]
        private SevenDayForecast forecast;

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public ForecastPageViewModel()
        {
            _weatherService = ServiceHelper.GetService<IWeatherService>();
        }

        public async Task LoadForecastAsync(double latitude, double longitude, string cityName)
        {
            _latitude = latitude;
            _longitude = longitude;
            _cityName = cityName;

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                Forecast = await _weatherService.GetSevenDayForecastAsync(latitude, longitude, cityName);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading forecast: {ex.Message}";
                Forecast = null;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task RefreshForecast()
        {
            if (_latitude > 0 && _longitude > 0)
            {
                await LoadForecastAsync(_latitude, _longitude, _cityName);
            }
        }
    }
}
