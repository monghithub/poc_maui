# Guía de Compilación para Android

Esta guía detalla cómo compilar la aplicación Weather App para Android.

## Requisitos Previos

### 1. Software Instalado

- **Visual Studio 2022** Community/Professional o Superior
- **.NET 9.0 SDK** o superior
- **Android SDK** (API 21 mínimo, recomendado API 33+)
- **Java Development Kit (JDK)** 11 o superior
- **Android Emulator** o dispositivo Android físico

### 2. Instalación en Visual Studio 2022

Si usas Visual Studio 2022, asegúrate de tener instalados estos componentes en el instalador:

1. Abre Visual Studio Installer
2. Selecciona tu instalación de Visual Studio 2022
3. Haz clic en "Modificar"
4. En la pestaña "Cargas de trabajo", selecciona:
   - ✅ Desarrollo móvil con .NET (MAUI)
   - ✅ Desarrollo de Android
5. En la pestaña "Componentes individuales", asegúrate de tener:
   - ✅ Android SDK (nivel 33+)
   - ✅ Emulador de Android
   - ✅ Intel HAXM (aceleración de hardware)
6. Haz clic en "Modificar" y espera a que se instalen

### 3. Instalación Manual de MAUI Workload

Si usas la línea de comandos:

```bash
# Instalar MAUI
dotnet workload install maui

# Instalar Android Workload
dotnet workload install android

# Restaurar dependencias
cd WeatherApp
dotnet restore
```

## Compilación

### Opción 1: Usando Visual Studio 2022

1. Abre la solución `WeatherApp.sln`
2. En la barra superior, selecciona la configuración:
   - Selecciona **Debug** o **Release**
   - Selecciona **net9.0-android** en el dropdown de plataformas
   - Selecciona un **Android Emulator** o dispositivo físico
3. Presiona **F5** para compilar y ejecutar

### Opción 2: Usando Línea de Comandos

#### Compilar APK de Debug

```bash
cd WeatherApp

# Restaurar paquetes
dotnet restore

# Compilar para Android
dotnet build -f net9.0-android -c Debug

# O directamente publicar
dotnet publish -f net9.0-android -c Debug
```

#### Compilar APK de Release

```bash
cd WeatherApp

# Compilar y publicar
dotnet publish -f net9.0-android -c Release

# Esto generará un APK en:
# bin/Release/net9.0-android/publish/com.monghit.weatherapp.apk
```

#### Ejecutar en Emulador

```bash
# Asegúrate de que el emulador está corriendo primero
# Luego:

dotnet build -t Run -f net9.0-android
```

#### Desplegar en Dispositivo Físico

1. Conecta tu dispositivo Android vía USB
2. Habilita el modo de depuración en tu dispositivo (Configuración > Opciones de desarrollador > Depuración USB)
3. Ejecuta:

```bash
dotnet build -t Run -f net9.0-android
```

## Ubicación de Archivos Generados

Después de una compilación exitosa:

```
WeatherApp/
├── bin/
│   └── Debug/
│       └── net9.0-android/
│           ├── AndroidPackages/           # APK sin firmar
│           └── publish/
│               └── com.monghit.weatherapp.apk  # APK de depuración
│
└── obj/
    └── Debug/
        └── net9.0-android/
```

## Configuración del Emulador de Android

### Crear un Emulador (primera vez)

1. Abre **Android Emulator Manager** desde Visual Studio
2. Haz clic en **Crear** (Create AVD)
3. Selecciona un dispositivo (recomendado: Pixel 4)
4. Selecciona una imagen del sistema (recomendado: Android 13 o superior)
5. Configura la RAM (recomendado: 2-4 GB)
6. Haz clic en **Crear**

### Iniciar el Emulador

Desde Visual Studio:
- En el dropdown de selección de dispositivo, elige tu emulador
- Presiona el botón de reproducción

O manualmente:
```bash
emulator -avd <avd_name> -wipe-data
```

## Solución de Problemas

### Error: "Android SDK not found"

```bash
# Verifica la instalación
dotnet workload list

# Reinstala si es necesario
dotnet workload repair
```

### Error: "MAUI framework not installed"

```bash
# Instala o repara MAUI
dotnet workload install maui --skip-manifest-update
```

### Error: "Java not found"

```bash
# Verifica JAVA_HOME está configurado
echo %JAVA_HOME%  # Windows
echo $JAVA_HOME   # Linux/Mac

# Si no está configurado, configura la variable de entorno
# JAVA_HOME=C:\Program Files\Java\jdk-11
```

### El emulador es muy lento

- Habilita la aceleración de hardware (Intel HAXM o Hyper-V)
- Aumenta la RAM del emulador a 2-4 GB
- Usa una imagen del sistema más ligera (API 30 o 31)

### APK no se instala en el dispositivo

```bash
# Desinstala versiones anteriores
adb uninstall com.monghit.weatherapp

# Luego compila e instala de nuevo
dotnet build -t Run -f net9.0-android
```

## Distribución (Google Play)

Para publicar en Google Play Store:

1. **Generar APK firmado**:
```bash
# Primero, crear un keystore
keytool -genkey -v -keystore my-release-key.keystore -keyalg RSA -keysize 2048 -validity 10000 -alias my-key-alias

# Luego, compilar con firma
dotnet publish -f net9.0-android -c Release \
    -p:AndroidKeyStore=true \
    -p:AndroidSigningKeyStore=True \
    -p:AndroidSigningKeyAlias=my-key-alias \
    -p:AndroidSigningKeyPass=my-key-password \
    -p:AndroidSigningStorePass=my-store-password
```

2. **Crear cuenta de Google Play Developer** (costo único de $25)

3. **Preparar la aplicación**:
   - Aumentar versionCode en el .csproj
   - Actualizar ApplicationVersion
   - Crear descripción en Google Play Console

4. **Subir el APK**:
   - Ir a Google Play Console
   - Crear nueva app
   - Subir el APK firmado
   - Completar información y enviar para revisión

## Recursos Adicionales

- [MAUI Documentation](https://learn.microsoft.com/en-us/dotnet/maui/)
- [Android Deployment](https://learn.microsoft.com/en-us/dotnet/maui/android/)
- [Google Play Store Guidelines](https://play.google.com/console/developer)

## Notas Importantes

- El APK de debug es solo para desarrollo y pruebas
- El APK de release debe ser firmado para publicación en Play Store
- La aplicación requiere conexión a Internet para obtener datos del clima
- Los permisos se definen en `Platforms/Android/AndroidManifest.xml`
