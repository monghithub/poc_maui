using WeatherApp.ViewModels;

namespace WeatherApp.Views
{
    [QueryProperty(nameof(Latitude), "latitude")]
    [QueryProperty(nameof(Longitude), "longitude")]
    [QueryProperty(nameof(CityName), "cityName")]
    public partial class ForecastPage : ContentPage
    {
        private double _latitude;
        private double _longitude;
        private string _cityName;

        public double Latitude
        {
            get => _latitude;
            set
            {
                _latitude = value;
                OnQueryPropertyChanged();
            }
        }

        public double Longitude
        {
            get => _longitude;
            set
            {
                _longitude = value;
                OnQueryPropertyChanged();
            }
        }

        public string CityName
        {
            get => _cityName;
            set
            {
                _cityName = Uri.UnescapeDataString(value ?? string.Empty);
                OnQueryPropertyChanged();
            }
        }

        private ForecastPageViewModel _viewModel;

        public ForecastPage()
        {
            InitializeComponent();
            _viewModel = new ForecastPageViewModel();
            BindingContext = _viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            OnQueryPropertyChanged();
        }

        private async void OnQueryPropertyChanged()
        {
            if (_latitude > 0 && _longitude > 0 && !string.IsNullOrEmpty(_cityName))
            {
                await _viewModel.LoadForecastAsync(_latitude, _longitude, _cityName);
            }
        }
    }
}
