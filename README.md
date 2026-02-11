# Weather App - MAUI PoC

Prueba de concepto de una aplicación MAUI (.NET) para Android que se conecta a un servidor público de clima.

## 📋 Descripción

Esta aplicación MAUI simple permite:
- Buscar información del clima por ciudad
- Mostrar temperatura, humedad, velocidad del viento
- Usar APIs públicas sin autenticación
- Ser compilada para Android

## 🔧 Requisitos

- **Visual Studio 2022** o **Visual Studio Code** con extensiones
- **.NET 9.0 SDK** o superior
- **MAUI Workload** instalado
- **Android SDK** (para compilación a Android)
- **Java Development Kit (JDK)** versión 11 o superior

## 📦 Instalación

### 1. Instalar .NET y MAUI Workload

```bash
# Instalar .NET SDK (si no lo tienes)
dotnet --version  # Verifica que tengas .NET 9.0 o superior

# Instalar MAUI Workload
dotnet workload install maui
```

### 2. Instalar Android SDK (si no tienes)

```bash
# En Windows/Mac/Linux, puedes usar Android Studio o
dotnet workload install android
```

### 3. Clonar/Descargar el Proyecto

```bash
cd WeatherApp
```

## 🚀 Compilación y Ejecución

### Compilación Automática con GitHub Actions (Recomendado)

La compilación automática se ejecuta en **macOS** (donde MAUI es completamente soportado):

1. Cada push a `master`, `main` o `develop` dispara el workflow
2. El APK se compila automáticamente
3. El artefacto está disponible en **Actions** → **Latest Run** → **Artifacts**
4. Para crear una release con el APK, crea un tag: `git tag v1.0 && git push github v1.0`

**Ver compilaciones:** https://github.com/monghithub/poc_maui/actions

### Compilar Localmente (Solo macOS)

```bash
# Compilar en modo debug
dotnet build -f net9.0-android

# Compilar en modo release
dotnet publish -f net9.0-android -c Release
```

**Nota:** La compilación en **Linux** no es soportada por MAUI. Usa GitHub Actions en su lugar.

### Ejecutar en emulador o dispositivo

```bash
# Ejecutar en emulador (requiere emulador configurado)
dotnet build -t Run -f net9.0-android

# O usando VS2022, simplemente selecciona Android Emulator y presiona F5
```

### Compilar para otras plataformas

```bash
# iOS (requiere Mac)
dotnet build -f net9.0-ios

# Windows
dotnet build -f net9.0-windows10.0.19041.0

# macCatalyst
dotnet build -f net9.0-maccatalyst
```

## 📐 Estructura del Proyecto

```
WeatherApp/
├── Models/                 # Modelos de datos (WeatherData.cs)
├── Services/              # Servicios (WeatherService, IWeatherService)
├── ViewModels/            # ViewModels (MainPageViewModel)
├── Views/                 # Páginas XAML (MainPage.xaml)
├── Converters/            # ValueConverters
├── Platforms/
│   └── Android/           # Archivos específicos de Android
├── Resources/
│   ├── AppIcon/          # Iconos de la app
│   ├── Splash/           # Pantalla de splash
│   ├── Fonts/            # Fuentes personalizadas
│   └── Images/           # Imágenes
├── App.xaml & App.xaml.cs
├── AppShell.xaml & AppShell.xaml.cs
├── MauiProgram.cs        # Punto de entrada
└── WeatherApp.csproj     # Archivo de proyecto
```

## 🌐 APIs Utilizadas

### Open-Meteo (Clima)
- **URL**: `https://api.open-meteo.com/v1/forecast`
- **Autenticación**: No requerida
- **Datos**: Temperatura, humedad, velocidad del viento, código de clima
- **Documentación**: https://open-meteo.com/

### Nominatim (Geocodificación)
- **URL**: `https://nominatim.openstreetmap.org/search`
- **Autenticación**: No requerida
- **Datos**: Coordenadas (lat/lon) desde nombre de ciudad
- **Documentación**: https://nominatim.org/

## 🎨 Características

### Core
- ✅ Búsqueda de clima por nombre de ciudad
- ✅ Información en tiempo real
- ✅ UI moderna con MAUI XAML
- ✅ MVVM con CommunityToolkit.Mvvm
- ✅ Manejo de errores
- ✅ Indicador de carga
- ✅ Actualización de hora

### Avanzadas
- ✅ **Geolocalización automática** - Detecta ubicación del dispositivo
- ✅ **Pronóstico de 7 días** - Predicción detallada para próxima semana
- ✅ **Historial de búsquedas** - Guarda últimas 10 ciudades buscadas
- ✅ **Almacenamiento local** - Persiste datos en el dispositivo
- ✅ **Reverse geocoding** - Convierte coordenadas a nombres de ciudad
- ✅ **Pull-to-refresh** - Actualiza datos deslizando hacia abajo

## 🔐 Permisos de Android

La aplicación requiere los siguientes permisos:
- `INTERNET` - Para conectar a las APIs
- `ACCESS_COARSE_LOCATION` - Información de ubicación aproximada
- `ACCESS_FINE_LOCATION` - Información de ubicación precisa

Estos se definen en `Platforms/Android/AndroidManifest.xml`

## 🐛 Troubleshooting

### Error: "MAUI workload not compatible with this platform"
Este error ocurre en **Linux**. La compilación de MAUI no es soportada en Linux.

**Solución:** Usa **GitHub Actions** que compila automáticamente en macOS. El workflow `android-build-macos.yml` se ejecuta automáticamente en cada push:
- Compila el APK en macOS
- Lo publica como artefacto en GitHub
- Está disponible para descargar en la sección de Actions

No necesitas compilar localmente en Linux. ✅

### Error: "Android SDK not found"
Instala el Android SDK:
```bash
dotnet workload install android
```

### Error: "No suitable Android device found"
Asegúrate de que:
- Un emulador Android está corriendo, O
- Un dispositivo Android está conectado vía USB

## 📚 Documentación Detallada

Para información completa sobre las características avanzadas, consulta:
- **[FEATURES.md](./FEATURES.md)** - Documentación detallada de todas las características
- **[ANDROID_BUILD.md](./ANDROID_BUILD.md)** - Guía de compilación para Android

## 🎯 Flujo de la Aplicación

```
┌─────────────────┐
│  Abrir App      │
└────────┬────────┘
         │
         ▼
┌─────────────────────────┐
│  ¿Permitir ubicación?   │
└────────┬────────────────┘
         │
    ┌────┴────┐
    │          │
    ▼          ▼
[SÍ]         [NO]
    │          │
    │          ▼
    │    └──────────────┐
    │                   │
    ▼                   ▼
┌──────────────┐  ┌─────────────┐
│ Ubicación    │  │ London      │
│ del GPS      │  │ (Default)   │
└───┬──────────┘  └──────┬──────┘
    │                    │
    └────────┬───────────┘
             │
             ▼
    ┌────────────────┐
    │ Mostrar Clima  │
    │ Actual         │
    └────┬───────────┘
         │
    ┌────┴────────────────────────────┐
    │                                 │
    ▼                                 ▼
 ┌────────┐                    ┌──────────────┐
 │ 📍     │ - Actualizar      │ 📋 Historial │
 │ 🔍     │ - Nueva búsqueda  │ 📅 Pronóstico│
 │ 📋     │ - Ver historial   └──────────────┘
 └────────┘
```

## 📝 Notas

- La aplicación usa `Open-Meteo` y `Nominatim` que son servicios públicos y gratuitos
- No requiere API keys
- La información del clima se actualiza en tiempo real
- Los datos se obtienen en la unidad de medida del servidor (Celsius para temperatura, km/h para velocidad del viento)
- El historial se almacena localmente en el dispositivo usando MAUI Preferences
- La geolocalización requiere permiso explícito del usuario en Android

## 📄 Licencia

Proyecto de demostración educativa.

## 📞 Soporte

Para reportar issues o sugerencias, contacta con el equipo de desarrollo.
