# Weather App - Características Completas

Este documento detalla todas las características implementadas en la Weather App MAUI.

## 🌍 1. Geolocalización Automática

### Descripción
La aplicación detecta automáticamente la ubicación del dispositivo al iniciar y muestra el clima para esa localización.

### Funcionalidades
- **Detección automática**: Se obtiene la ubicación GPS al abrir la app (si el usuario lo permite)
- **Botón de actualización**: Presiona 📍 para obtener la ubicación nuevamente
- **Indicador visual**: Muestra "(Using device location)" cuando se usa geolocalización
- **Permisos**: Solicita permiso `ACCESS_FINE_LOCATION` la primera vez
- **Fallback**: Si el usuario deniega permisos, muestra London por defecto

### Implementación Técnica
- **API**: `MAUI Geolocation` (Microsoft.Maui.Devices.Sensors)
- **Reverse Geocoding**: Nominatim para convertir coordenadas a nombre de ciudad
- **Timeout**: 10 segundos máximo para obtener ubicación
- **Archivos**:
  - `Services/WeatherService.cs`: Métodos `RequestLocationPermissionAsync()`, `GetCurrentLocationAsync()`
  - `ViewModels/MainPageViewModel.cs`: Comando `UseCurrentLocationCommand`
  - `Views/MainPage.xaml`: Botón 📍 de geolocalización

### Uso
1. Abre la app → solicita permiso de ubicación
2. Concede permiso → muestra clima de tu ubicación
3. Presiona 📍 → actualiza tu ubicación en cualquier momento

## 📅 2. Pronóstico de 7 Días

### Descripción
Visualiza el pronóstico del clima para los próximos 7 días con detalles completos.

### Datos Mostrados por Día
- 🗓️ **Fecha**: Día de la semana y fecha completa
- 🌡️ **Temperatura**: Máxima y mínima
- 📊 **Descripción**: Estado del clima (Claro, Nublado, Lluvia, etc.)
- 💧 **Precipitación**: Milímetros de lluvia esperados
- 💨 **Viento**: Velocidad máxima en km/h

### Funcionalidades
- **Tarjetas por día**: Información bien organizada para cada día
- **Pull-to-refresh**: Desliza hacia abajo para actualizar pronóstico
- **Navegación**: Accede desde el botón "📅 View 7-Day Forecast" en la página principal
- **Retención de datos**: Mantiene los datos cuando cambias entre vistas

### Implementación Técnica
- **API**: Open-Meteo endpoint `/daily`
- **Parámetros**:
  - `temperature_2m_max`, `temperature_2m_min`
  - `weather_code`, `precipitation_sum`, `wind_speed_10m_max`
  - `timezone=auto` para zona horaria automática
- **Archivos**:
  - `Services/WeatherService.cs`: Método `GetSevenDayForecastAsync()`
  - `Models/WeatherData.cs`: Clases `DailyWeather`, `ForecastDay`, `SevenDayForecast`
  - `ViewModels/ForecastPageViewModel.cs`: Lógica de carga y refresh
  - `Views/ForecastPage.xaml`: UI con grid de días
  - `AppShell.xaml`: Ruta de navegación

### Uso
1. Busca una ciudad en la página principal
2. Presiona "📅 View 7-Day Forecast" en la parte inferior
3. Desliza hacia abajo para actualizar los datos

## 📋 3. Historial de Búsquedas

### Descripción
La aplicación guarda automáticamente las últimas 10 ciudades buscadas para acceso rápido.

### Características
- **Almacenamiento automático**: Se guarda cada búsqueda realizada
- **Máximo 10 ciudades**: Las más recientes se mantienen al inicio de la lista
- **Coordenadas incluidas**: Carga rápida sin necesidad de geocodificación
- **Panel de historial**: Presiona 📋 para ver/ocultar lista de búsquedas
- **Selección rápida**: Haz clic en una ciudad para cargar su clima al instante
- **Eliminación individual**: Botón ✕ para borrar ciudades específicas del historial

### Datos Almacenados
Por cada búsqueda se guarda:
- Nombre de la ciudad
- Latitud y Longitud
- Timestamp de la búsqueda

### Almacenamiento
- **Sistema**: MAUI Preferences (almacenamiento local del dispositivo)
- **Ubicación**: `/data/data/com.monghit.weatherapp/` en Android
- **Formato**: JSON serializado
- **Persistencia**: Sobrevive a cerrar y reabrir la app

### Implementación Técnica
- **Archivos**:
  - `Services/SearchHistoryService.cs`: Clase que gestiona almacenamiento
  - `Models/WeatherData.cs`: Clase `SearchHistoryItem`
  - `ViewModels/MainPageViewModel.cs`: Comandos para historial
  - `Views/MainPage.xaml`: Panel y lista de historial
- **Métodos principales**:
  - `AddSearchHistory(city, lat, lon)`: Agregar búsqueda
  - `GetSearchHistory()`: Obtener lista
  - `RemoveSearchHistory(city)`: Eliminar una ciudad
  - `ClearSearchHistory()`: Borrar todo

### Uso
1. Busca ciudades normalmente (Londres, París, Nueva York, etc.)
2. Presiona 📋 para ver el historial
3. Haz clic en cualquier ciudad para cargar su clima
4. Presiona ✕ para eliminar del historial

## 🎨 Interfaz de Usuario

### Página Principal (MainPage)

**Sección Superior**:
- Título "Weather Forecast"
- Campo de entrada para nombre de ciudad
- 3 botones de acción:
  - 🔍 (Rojo): Buscar ciudad
  - 📍 (Verde): Usar ubicación del dispositivo
  - 📋 (Azul): Toggle historial de búsquedas

**Sección Media** (dos vistas intercambiables):
1. **Vista de Clima** (por defecto):
   - Nombre de ubicación
   - Indicador de geolocalización (si aplica)
   - Temperatura actual (grande)
   - Descripción del clima
   - Humedad y velocidad del viento
   - Última actualización
   - Botón 📅 para pronóstico

2. **Vista de Historial** (cuando presionas 📋):
   - Lista de ciudades recientes
   - Botón de selección para cada ciudad
   - Botón ✕ para eliminar

**Errores y Estados**:
- Mensajes de error en rojo
- Indicador de carga (spinning circle)
- Mensajes "Sin historial" cuando está vacío

### Página de Pronóstico (ForecastPage)

- Título con nombre de ciudad
- Tarjetas para cada día con:
  - Fecha formateada
  - Min/Max de temperaturas
  - Descripción del clima
  - Precipitación
  - Velocidad del viento
- Pull-to-refresh para actualizar
- Botón "Atrás" para volver

## 🌐 APIs Externas Utilizadas

### 1. Open-Meteo (Clima)
- **URL Base**: https://api.open-meteo.com/v1/forecast
- **Métodos**:
  - Actual: `/forecast?latitude=X&longitude=Y&current=...`
  - Pronóstico: `/forecast?latitude=X&longitude=Y&daily=...`
- **Autenticación**: No requerida
- **Límites**: Uso libre sin restricción para PoC
- **Documentación**: https://open-meteo.com/

### 2. Nominatim (Geocodificación)
- **URL Base**: https://nominatim.openstreetmap.org
- **Métodos**:
  - Forward: `/search?q=CITY&format=json`
  - Reverse: `/reverse?lat=X&lon=Y&format=json`
- **Autenticación**: No requerida
- **Documentación**: https://nominatim.org/

## 📊 Códigos de Clima

La aplicación interpreta los códigos WMO de Open-Meteo:

| Código | Descripción |
|--------|------------|
| 0 | Clear sky |
| 1-2 | Mostly clear |
| 3 | Overcast |
| 45-48 | Foggy |
| 51-55 | Drizzle/Light rain |
| 61-65 | Rain |
| 71-75 | Snow |
| 80-82 | Rain showers |
| 85-86 | Snow showers |
| 95-99 | Thunderstorm |

## 🔐 Permisos de Android

La aplicación requiere estos permisos (definidos en `AndroidManifest.xml`):

```xml
<uses-permission android:name="android.permission.INTERNET" />
<uses-permission android:name="android.permission.ACCESS_COARSE_LOCATION" />
<uses-permission android:name="android.permission.ACCESS_FINE_LOCATION" />
```

- **INTERNET**: Para conectar a APIs
- **ACCESS_COARSE_LOCATION**: Ubicación aproximada (red celular/WiFi)
- **ACCESS_FINE_LOCATION**: GPS preciso

## 🚀 Flujo de Usuario Completo

### Primer Inicio
1. App abre → solicita permiso de ubicación
2. Usuario concede/deniega permisos
3. Si concede: muestra clima de su ubicación actual
4. Si deniega: muestra clima de Londres (default)
5. Historial cargado (vacío en primer inicio)

### Uso Normal
1. **Ver clima actual**:
   - App carga automáticamente con ubicación o ciudad default
   - Presiona 📍 para actualizar ubicación
   - Presiona 🔍 después de escribir ciudad

2. **Acceder a historial**:
   - Presiona 📋 para mostrar últimas 10 búsquedas
   - Haz clic en cualquiera para cargar al instante
   - Presiona ✕ para eliminar de historial
   - Presiona 📋 nuevamente para ocultar

3. **Ver pronóstico**:
   - Una vez tengas clima de una ciudad
   - Presiona "📅 View 7-Day Forecast"
   - Desliza para actualizar si es necesario
   - Botón "Atrás" para volver a clima actual

## 📱 Compatibilidad

- ✅ **Android 5.1+** (API 21+)
- ✅ **iOS** (configurado, requiere macOS para compilar)
- ✅ **Windows 10+**
- ✅ **macCatalyst**

## 🔄 Datos en Caché

- **Clima actual**: Actualiza en cada búsqueda
- **Pronóstico**: Se mantiene hasta que navegas a otra ciudad
- **Historial**: Persiste en el dispositivo (MAUI Preferences)
- **Ubicación**: Se obtiene fresh con cada tap en 📍

## 📈 Planes Futuros (Sugerencias)

- Favoritos personalizados además del historial
- Notificaciones de alerta de mal tiempo
- Gráficos de temperatura históricos
- Múltiples unidades (Fahrenheit, mph)
- Modo oscuro automático
- Widgets en pantalla de inicio
- Integración con alarmas basada en clima
- Soporte offline con caché local

## ⚙️ Variables de Configuración

Puedes ajustar en `SearchHistoryService.cs`:

```csharp
private const int MaxHistoryItems = 10; // Máximo de búsquedas almacenadas
```

Y en `WeatherService.cs`:

```csharp
TimeSpan.FromSeconds(10) // Timeout de geolocalización
```
