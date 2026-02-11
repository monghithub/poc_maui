# Arquitectura de Weather App

Documento técnico que describe la arquitectura, componentes y patrones de diseño de la aplicación.

## 🏗️ Patrón Arquitectónico: MVVM

La aplicación utiliza **Model-View-ViewModel (MVVM)** con las siguientes capas:

```
┌─────────────────────────────────────┐
│   VIEW LAYER (XAML)                 │
│  - MainPage.xaml                    │
│  - ForecastPage.xaml                │
│  - App.xaml / AppShell.xaml         │
└──────────────────┬──────────────────┘
                   │
                   │ Data Binding
                   │
┌──────────────────▼──────────────────┐
│   VIEWMODEL LAYER (C#)              │
│  - MainPageViewModel                │
│  - ForecastPageViewModel            │
│  - ServiceHelper (DI)               │
└──────────────────┬──────────────────┘
                   │
                   │ Method Calls
                   │
┌──────────────────▼──────────────────┐
│   SERVICE LAYER                     │
│  - IWeatherService                  │
│  - WeatherService                   │
│  - SearchHistoryService             │
└──────────────────┬──────────────────┘
                   │
                   │ HTTP / Local Storage
                   │
┌──────────────────▼──────────────────┐
│   EXTERNAL APIS & STORAGE           │
│  - Open-Meteo API                   │
│  - Nominatim API                    │
│  - MAUI Preferences (Local)         │
│  - MAUI Geolocation                 │
└─────────────────────────────────────┘
```

## 📦 Estructura de Directorios

```
WeatherApp/
├── Models/                           # Modelos de datos
│   └── WeatherData.cs
│       ├── WeatherResponse           # Respuesta de Open-Meteo
│       ├── CurrentWeather            # Datos actuales
│       ├── DailyWeather              # Datos diarios
│       ├── WeatherInfo               # Información formateada para UI
│       ├── ForecastDay               # Día de pronóstico
│       ├── SevenDayForecast          # Contenedor de 7 días
│       └── SearchHistoryItem         # Elemento del historial
│
├── Services/                         # Lógica de negocio
│   ├── IWeatherService.cs            # Interface (contrato)
│   ├── WeatherService.cs             # Implementación
│   │   ├── GetWeatherByCityAsync()
│   │   ├── GetWeatherByCoordinatesAsync()
│   │   ├── GetSevenDayForecastAsync()
│   │   ├── GetCoordinatesByCityAsync()
│   │   ├── RequestLocationPermissionAsync()
│   │   ├── GetCurrentLocationAsync()
│   │   └── GetWeatherDescription()
│   ├── SearchHistoryService.cs       # Persistencia
│   │   ├── AddSearchHistory()
│   │   ├── GetSearchHistory()
│   │   ├── RemoveSearchHistory()
│   │   └── ClearSearchHistory()
│   └── ServiceHelper.cs              # Inyección de dependencias
│
├── ViewModels/                       # Lógica de presentación
│   ├── MainPageViewModel.cs
│   │   ├── CurrentWeather (prop)
│   │   ├── SearchCity (prop)
│   │   ├── IsLoading (prop)
│   │   ├── ErrorMessage (prop)
│   │   ├── ShowSearchHistory (prop)
│   │   ├── SearchHistory (collection)
│   │   ├── UseDeviceLocation (prop)
│   │   ├── SearchWeatherCommand
│   │   ├── UseCurrentLocationCommand
│   │   ├── ToggleSearchHistoryCommand
│   │   ├── SelectHistoryItemCommand
│   │   ├── GoToForecastCommand
│   │   ├── LoadSearchHistory()
│   │   └── LoadInitialWeather()
│   └── ForecastPageViewModel.cs
│       ├── ForecastData (prop)
│       ├── IsLoading (prop)
│       ├── ErrorMessage (prop)
│       ├── RefreshForecastCommand
│       ├── LoadForecast()
│       └── [QueryProperty] para parámetros
│
├── Views/                            # Interfaz de usuario
│   ├── MainPage.xaml / MainPage.xaml.cs
│   │   ├── Entrada de ciudad
│   │   ├── Botones de acción (🔍, 📍, 📋)
│   │   ├── Panel de historial (condicional)
│   │   ├── Tarjeta de clima (condicional)
│   │   └── Botón de pronóstico
│   └── ForecastPage.xaml / ForecastPage.xaml.cs
│       ├── Encabezado con ciudad
│       ├── Grid de días (7 tarjetas)
│       └── Pull-to-refresh
│
├── Converters/                       # Conversores de datos XAML
│   ├── StringToBoolConverter.cs      # "" → false, no vacío → true
│   └── NullToVisibilityConverter.cs  # null → false, not null → true
│
├── Platforms/                        # Código específico de plataforma
│   └── Android/
│       ├── AndroidManifest.xml       # Permisos
│       └── MainActivity.cs            # Activity principal
│
├── Resources/                        # Recursos estáticos
│   ├── AppIcon/
│   │   └── appicon.svg
│   ├── Splash/
│   │   └── splash.svg
│   ├── Fonts/                        # (OpenSans)
│   └── Images/                       # (placeholders)
│
├── App.xaml / App.xaml.cs            # Recursos globales
├── AppShell.xaml / AppShell.xaml.cs  # Navegación y shell
├── MauiProgram.cs                    # Punto de entrada y DI
└── WeatherApp.csproj                 # Configuración del proyecto
```

## 🔄 Flujo de Datos

### 1. Búsqueda de Clima por Ciudad

```
MainPage.xaml (Usuario escribe)
         │
         ▼
MainPageViewModel.SearchWeatherCommand
         │
         ├─► Validar entrada
         │
         ├─► IsLoading = true
         │
         ▼
WeatherService.GetCoordinatesByCityAsync()
         │
         ├─► Nominatim API (forward geocoding)
         │
         ▼
WeatherService.GetWeatherByCoordinatesAsync()
         │
         ├─► Open-Meteo API (clima actual)
         │
         ▼
ViewModel.CurrentWeather = result
         │
         ├─► SearchHistoryService.AddSearchHistory()
         │   └─► MAUI Preferences (JSON storage)
         │
         ├─► IsLoading = false
         │
         ▼
MainPage.xaml (UI actualiza via binding)
```

### 2. Geolocalización Automática

```
App.xaml.cs → MainPage.xaml.cs (OnAppearing)
         │
         ▼
MainPageViewModel.LoadInitialWeather()
         │
         ▼
WeatherService.RequestLocationPermissionAsync()
         │
         ├─► MAUI Permissions.CheckStatusAsync()
         │
         ├─► Si no tiene permiso → Request
         │
         ▼
WeatherService.GetCurrentLocationAsync()
         │
         ├─► MAUI Geolocation.GetLocationAsync()
         │   └─► GPS (timeout 10s)
         │
         ├─► GetWeatherByCoordinatesAsync()
         │
         ▼
ViewModel.CurrentWeather = result
         ├─► UseDeviceLocation = true
         │
         ▼
MainPage.xaml (Actualiza con ubicación real)
```

### 3. Pronóstico de 7 Días

```
MainPage.xaml (Botón "📅 View 7-Day Forecast")
         │
         ▼
MainPageViewModel.GoToForecastCommand
         │
         ├─► Validar CurrentWeather existe
         │
         ▼
Shell.GoToAsync("forecast?lat=X&lon=Y&city=Z")
         │
         ▼
ForecastPage.xaml
         │
         ├─► [QueryProperty] desglosa parámetros
         │
         ▼
ForecastPageViewModel.LoadForecast()
         │
         ▼
WeatherService.GetSevenDayForecastAsync()
         │
         ├─► Open-Meteo API (datos diarios)
         │
         ├─► Parsing (7 días máximo)
         │
         ▼
ViewModel.ForecastData = result
         │
         ▼
ForecastPage.xaml (Binding a collection → Grid)
```

### 4. Historial de Búsquedas

```
MainPage.xaml (Botón "📋")
         │
         ▼
MainPageViewModel.ToggleSearchHistoryCommand
         │
         ├─► ShowSearchHistory = !ShowSearchHistory
         │
         ├─► Si true → LoadSearchHistory()
         │
         ▼
SearchHistoryService.GetSearchHistory()
         │
         ├─► MAUI Preferences.Get("SearchHistory")
         │
         ├─► JsonDeserialize()
         │
         ▼
MainPageViewModel.SearchHistory (ObservableCollection)
         │
         ▼
MainPage.xaml (BindableLayout renderiza lista)
         │
         ├─► Usuario hace click
         │
         ▼
SelectHistoryItemCommand
         │
         ├─► WeatherService.GetWeatherByCoordinatesAsync()
         │   (sin geocodificación, coordenadas almacenadas)
         │
         ▼
ViewModel.CurrentWeather = result (muy rápido)
```

## 🔌 Inyección de Dependencias

### Configuración en MauiProgram.cs

```csharp
builder.Services
    .AddSingleton<IWeatherService, WeatherService>()
    .AddSingleton<SearchHistoryService>()
    .AddSingleton<MainPage>()
    .AddSingleton<MainPageViewModel>()
    .AddSingleton<ForecastPage>()
    .AddSingleton<ForecastPageViewModel>()
```

### Resolución en ViewModels

```csharp
// Opción 1: Constructor (recomendado, pero complejo con MVVM Toolkit)
public MainPageViewModel(IWeatherService weatherService)
{
    _weatherService = weatherService;
}

// Opción 2: ServiceHelper (usado en proyecto)
private readonly IWeatherService _weatherService =
    ServiceHelper.GetService<IWeatherService>();
```

## 📡 Comunicación con APIs Externas

### Open-Meteo

**Endpoints utilizados**:

1. **Clima Actual**
```
GET /v1/forecast?
  latitude={lat}&
  longitude={lon}&
  current=temperature_2m,relative_humidity_2m,apparent_temperature,
           precipitation,weather_code,wind_speed_10m,wind_direction_10m&
  timezone=auto
```

Response:
```json
{
  "latitude": 51.5,
  "longitude": -0.1,
  "current": {
    "temperature_2m": 15.5,
    "weather_code": 0,
    "wind_speed_10m": 12.5
  }
}
```

2. **Pronóstico Diario**
```
GET /v1/forecast?
  latitude={lat}&
  longitude={lon}&
  daily=temperature_2m_max,temperature_2m_min,
        weather_code,precipitation_sum,wind_speed_10m_max&
  timezone=auto
```

Response:
```json
{
  "daily": {
    "time": ["2024-02-11", "2024-02-12", ...],
    "temperature_2m_max": [15.5, 16.0, ...],
    "temperature_2m_min": [10.2, 11.5, ...],
    "weather_code": [0, 1, ...],
    "precipitation_sum": [0, 0.5, ...],
    "wind_speed_10m_max": [12.5, 14.0, ...]
  }
}
```

### Nominatim (OpenStreetMap)

**Endpoints utilizados**:

1. **Forward Geocoding** (ciudad → coords)
```
GET /search?q={city}&format=json&limit=1

Response:
[
  {
    "lat": "51.5073",
    "lon": "-0.1276",
    "display_name": "London, England"
  }
]
```

2. **Reverse Geocoding** (coords → ciudad)
```
GET /reverse?lat={lat}&lon={lon}&format=json

Response:
{
  "address": {
    "city": "London",
    "town": "London",
    "county": "Greater London"
  }
}
```

## 💾 Persistencia Local

### MAUI Preferences

La aplicación usa `MAUI Preferences` para guardar el historial:

**Ubicación física**:
- **Android**: `/data/data/com.monghit.weatherapp/shared_prefs/`
- **iOS**: `Library/Preferences/`
- **Windows**: `Registry` (HKEY_CURRENT_USER)

**Formato almacenado**:
```
Key: "SearchHistory"
Value: JSON serializado
[
  {
    "cityName": "London",
    "latitude": 51.5074,
    "longitude": -0.1278,
    "searchedAt": "2024-02-11T10:30:00"
  },
  ...
]
```

**Métodos usados**:
```csharp
Preferences.Set("key", jsonValue);     // Guardar
Preferences.Get("key", defaultValue);  // Leer
Preferences.Remove("key");             // Eliminar
```

## 🎯 Patrones de Diseño Utilizados

### 1. MVVM
- Separación de concerns
- Data binding automático
- Fácil de testear

### 2. Singleton
- Instancia única de servicios
- Compartida entre viewmodels
- Reduce consumo de memoria

### 3. Repository Pattern
- `IWeatherService` abstrae acceso a datos
- `WeatherService` implementa detalles
- Fácil de hacer mock para pruebas

### 4. Command Pattern
- `RelayCommand` para interacciones del usuario
- Desacoplamiento de Vista y ViewModel
- Soporte automático para can execute

### 5. Observable Pattern
- `ObservableObject` notifica cambios
- `ObservableCollection` para listas
- UI actualiza automáticamente

## 🧪 Testabilidad

### Puntos fáciles de testear:

1. **Services** (sin dependencia de UI):
```csharp
var service = new WeatherService();
var weather = await service.GetWeatherByCityAsync("London");
Assert.NotNull(weather);
```

2. **ViewModels** (mockear IWeatherService):
```csharp
var mockService = new Mock<IWeatherService>();
mockService.Setup(x => x.GetWeatherByCityAsync("London"))
    .ReturnsAsync(expectedWeather);
var vm = new MainPageViewModel();
// Test...
```

3. **Converters**:
```csharp
var converter = new StringToBoolConverter();
var result = converter.Convert("text", null, null, null);
Assert.True(result);
```

## 🚀 Performance Considerations

### Optimizaciones implementadas:

1. **Async/Await**: Todas las operaciones de red sin bloqueo
2. **Singleton Services**: Una sola instancia
3. **Geolocation Timeout**: 10 segundos máximo
4. **API Caching**: Datos se mantienen en memoria entre páginas
5. **Lazy Loading**: Historia se carga solo cuando se abre panel

### Posibles mejoras:

1. **HTTP Caching**: Implementar cache headers
2. **Pagination**: Mostrar 5 ciudades en historial, cargar más
3. **Database**: SQLite para historial más grande
4. **Image Caching**: Cache de iconos de clima
5. **Background Tasks**: Actualizar clima en background

## 📊 Flujo de Excepción

Todas las excepciones se capturan y convierten en mensajes de usuario:

```csharp
try
{
    // Operación
}
catch (Exception ex)
{
    ErrorMessage = $"Error: {ex.Message}";
    // Log si es necesario
    System.Diagnostics.Debug.WriteLine(ex);
}
finally
{
    IsLoading = false;
}
```

Tipos de errores manejados:
- **Network errors**: "Unable to connect to weather service"
- **Location errors**: "Unable to access your location"
- **Parse errors**: "Invalid weather data received"
- **Not found**: "City 'XYZ' not found"

## 🔐 Seguridad

### Consideraciones de seguridad:

1. **Sin credenciales**: No requiere API keys
2. **HTTPS**: Todas las conexiones a APIs usan HTTPS
3. **Permisos**: Solo pide lo que necesita
4. **No tracking**: No envía datos de usuario
5. **Local storage**: Datos guardados solo en dispositivo

### Recomendaciones:

1. Validar entrada de usuario
2. Escapar parámetros en URLs
3. No guardar datos sensibles en Preferences
4. Usar HTTPS para cualquier API futura
5. Implementar certificate pinning si es necesario
