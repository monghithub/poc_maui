using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IWeatherService _weatherService;

        [ObservableProperty]
        private WeatherInfo currentWeather;

        [ObservableProperty]
        private string searchCity = "London";

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        public MainPageViewModel()
        {
            _weatherService = ServiceHelper.GetService<IWeatherService>();
        }

        [RelayCommand]
        public async Task SearchWeather()
        {
            if (string.IsNullOrWhiteSpace(SearchCity))
            {
                ErrorMessage = "Please enter a city name";
                return;
            }

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                CurrentWeather = await _weatherService.GetWeatherByCityAsync(SearchCity);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
                CurrentWeather = null;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task LoadInitialWeather()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                CurrentWeather = await _weatherService.GetWeatherByCityAsync(SearchCity);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }
    }
}
