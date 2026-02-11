using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using WeatherApp.Models;
using WeatherApp.Services;

namespace WeatherApp.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private readonly IWeatherService _weatherService;
        private readonly SearchHistoryService _searchHistoryService;

        [ObservableProperty]
        private WeatherInfo currentWeather;

        [ObservableProperty]
        private string searchCity = "London";

        [ObservableProperty]
        private bool isLoading = false;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private bool showSearchHistory = false;

        [ObservableProperty]
        private ObservableCollection<SearchHistoryItem> searchHistory = new();

        [ObservableProperty]
        private bool useDeviceLocation = false;

        [ObservableProperty]
        private double currentLatitude = 0;

        [ObservableProperty]
        private double currentLongitude = 0;

        public MainPageViewModel()
        {
            _weatherService = ServiceHelper.GetService<IWeatherService>();
            _searchHistoryService = new SearchHistoryService();
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

                if (CurrentWeather != null)
                {
                    var coords = await _weatherService.GetCoordinatesByCityAsync(SearchCity);
                    if (coords.HasValue)
                    {
                        CurrentLatitude = coords.Value.Latitude;
                        CurrentLongitude = coords.Value.Longitude;
                        _searchHistoryService.AddSearchHistory(SearchCity, coords.Value.Latitude, coords.Value.Longitude);
                        LoadSearchHistory();
                    }
                }
                ShowSearchHistory = false;
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

        [RelayCommand]
        public void ToggleSearchHistory()
        {
            ShowSearchHistory = !ShowSearchHistory;
            if (ShowSearchHistory)
            {
                LoadSearchHistory();
            }
        }

        [RelayCommand]
        public async Task SelectHistoryItem(SearchHistoryItem item)
        {
            if (item == null) return;

            SearchCity = item.CityName;
            CurrentLatitude = item.Latitude;
            CurrentLongitude = item.Longitude;
            ShowSearchHistory = false;

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                CurrentWeather = await _weatherService.GetWeatherByCoordinatesAsync(item.Latitude, item.Longitude, item.CityName);
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

        [RelayCommand]
        public void ClearHistoryItem(SearchHistoryItem item)
        {
            if (item != null)
            {
                _searchHistoryService.RemoveSearchHistory(item.CityName);
                LoadSearchHistory();
            }
        }

        [RelayCommand]
        public async Task UseCurrentLocation()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                ShowSearchHistory = false;

                var location = await _weatherService.GetCurrentLocationAsync();
                if (location.HasValue)
                {
                    CurrentLatitude = location.Value.Latitude;
                    CurrentLongitude = location.Value.Longitude;
                    UseDeviceLocation = true;

                    SearchCity = "Current Location";
                    CurrentWeather = await _weatherService.GetWeatherByCoordinatesAsync(location.Value.Latitude, location.Value.Longitude, "Current Location");
                }
                else
                {
                    ErrorMessage = "Unable to access your location. Please check location permissions.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error accessing location: {ex.Message}";
                CurrentWeather = null;
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        public async Task GoToForecast()
        {
            if (CurrentWeather == null)
            {
                ErrorMessage = "Please search for a location first";
                return;
            }

            try
            {
                var navigationParameter = new Dictionary<string, object>
                {
                    { "latitude", CurrentLatitude },
                    { "longitude", CurrentLongitude },
                    { "cityName", CurrentWeather.Location }
                };

                await Shell.Current.GoToAsync($"forecast?latitude={CurrentLatitude}&longitude={CurrentLongitude}&cityName={Uri.EscapeDataString(CurrentWeather.Location)}");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error navigating to forecast: {ex.Message}";
            }
        }

        public void LoadSearchHistory()
        {
            try
            {
                var history = _searchHistoryService.GetSearchHistory();
                SearchHistory.Clear();
                foreach (var item in history)
                {
                    SearchHistory.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading search history: {ex.Message}");
            }
        }

        public async Task LoadInitialWeather()
        {
            LoadSearchHistory();

            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var location = await _weatherService.GetCurrentLocationAsync();
                if (location.HasValue)
                {
                    CurrentLatitude = location.Value.Latitude;
                    CurrentLongitude = location.Value.Longitude;
                    UseDeviceLocation = true;
                    SearchCity = "Current Location";
                    CurrentWeather = await _weatherService.GetWeatherByCoordinatesAsync(location.Value.Latitude, location.Value.Longitude, "Current Location");
                }
                else
                {
                    CurrentWeather = await _weatherService.GetWeatherByCityAsync(SearchCity);
                    var coords = await _weatherService.GetCoordinatesByCityAsync(SearchCity);
                    if (coords.HasValue)
                    {
                        CurrentLatitude = coords.Value.Latitude;
                        CurrentLongitude = coords.Value.Longitude;
                    }
                }
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
