namespace WeatherApp.Services
{
    public static class ServiceHelper
    {
        public static TService GetService<TService>() where TService : class
        {
            if (Application.Current?.Handler?.MauiContext?.Services.GetService(typeof(TService)) is TService service)
                return service;

            throw new Exception($"Unable to resolve type {typeof(TService).Name}");
        }
    }
}
