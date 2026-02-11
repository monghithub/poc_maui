using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using WeatherApp.Views;
using WeatherApp.ViewModels;
using WeatherApp.Services;

namespace WeatherApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .Services
                    .AddSingleton<IWeatherService, WeatherService>()
                    .AddSingleton<MainPage>()
                    .AddSingleton<MainPageViewModel>();

            return builder.Build();
        }
    }
}
