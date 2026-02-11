# Desplegar Weather App en Móvil Android

Guía para instalar la aplicación en tu dispositivo Android.

## 📱 Opción 1: Compilar y Desplegar Localmente (Recomendado)

### Requisitos
- **Windows** o **macOS** (Linux no soporta MAUI completamente)
- **.NET 9.0 SDK** instalado
- **Android SDK** configurado
- **Dispositivo Android conectado vía USB** con modo de depuración habilitado

### Paso a Paso

#### 1. Preparar el Dispositivo Android

En tu Android:
1. Ve a **Configuración** → **Información del Teléfono**
2. Busca **Número de Compilación** y toca 7 veces rápidamente
3. Aparecerá "Eres desarrollador"
4. Ve a **Configuración** → **Opciones de Desarrollador**
5. Habilita **Depuración por USB**
6. Conecta tu dispositivo por USB a la computadora
7. Aparecerá un diálogo - toca **Permitir/Confiar**

#### 2. Compilar la APK

En tu máquina Windows/Mac con el proyecto:

```bash
# Navega a la carpeta del proyecto
cd WeatherApp

# Restaura las dependencias
dotnet restore

# Compila para Android
dotnet publish -f net9.0-android -c Release
```

Esto generará el archivo APK en:
```
WeatherApp/bin/Release/net9.0-android/publish/com.monghit.weatherapp.apk
```

#### 3. Instalar en el Dispositivo

**Opción A: Usando ADB (más rápido)**
```bash
# Verifica que el dispositivo está conectado
adb devices

# Instala la APK
adb install -r WeatherApp/bin/Release/net9.0-android/publish/com.monghit.weatherapp.apk

# Espera el mensaje "Success"
```

**Opción B: Manual (si no tienes ADB)**
1. Copia el archivo `com.monghit.weatherapp.apk` a tu dispositivo
2. Abre el navegador de archivos en Android
3. Navega a dónde guardaste el APK
4. Toca el archivo APK
5. Toca **Instalar**
6. Toca **Abrir** cuando termine

#### 4. Ejecutar la Aplicación

1. En tu Android, ve a **Aplicaciones**
2. Busca **Weather App**
3. Toca para abrir
4. Concede los permisos solicitados:
   - ✅ Acceso a la ubicación
   - ✅ (Opcional) Almacenamiento

---

## ☁️ Opción 2: GitHub Actions (Cloud Compilation)

Si no tienes Windows/Mac, la aplicación se puede compilar automáticamente en la nube.

### Configuración

1. **Sube el proyecto a GitHub**:
```bash
git remote add origin https://github.com/tu-usuario/tu-repo.git
git push -u origin master
```

2. **El workflow se ejecutará automáticamente** en cada push

3. **Descarga el APK**:
   - Ve a tu repositorio en GitHub
   - Haz clic en la pestaña **Actions**
   - Selecciona el último "Build Android APK"
   - Descarga el artifact "weather-app-apk"

### Para crear una versión oficial (Release):

```bash
# Crea un tag
git tag v1.0.0
git push origin v1.0.0

# GitHub Actions compilará y creará una Release automáticamente
```

---

## 🔧 Compilación en Linux (Alternativas)

Si insistes en compilar en Linux, tienes estas opciones:

### A. Usar Docker con Windows como base

```dockerfile
FROM mcr.microsoft.com/windows/servercore:ltsc2022

# Instala .NET, Android SDK, etc.
# (Nota: Los contenedores Windows requieren Windows como host)
```

### B. Máquina Virtual Windows en Linux

1. Instala VirtualBox o KVM
2. Crea una VM con Windows 10/11
3. Instala .NET SDK y MAUI workload
4. Compila el proyecto dentro de la VM

### C. Servicio de Compilación en la Nube

Usa plataformas como:
- **GitHub Actions** (recomendado, gratis)
- **Azure Pipelines**
- **Travis CI**

---

## ✅ Verificación Post-Instalación

Una vez instalada, verifica que todo funciona:

1. **Ubicación automática**:
   - Abre la app
   - Permite acceso a ubicación
   - Debería mostrar clima de tu ubicación actual ✓

2. **Búsqueda**:
   - Escribe "París" en el campo de búsqueda
   - Presiona 🔍
   - Debería mostrar clima de París ✓

3. **Historial**:
   - Presiona 📋
   - Debería mostrar "París" en el historial ✓

4. **Pronóstico**:
   - Presiona "📅 View 7-Day Forecast"
   - Debería mostrar 7 días de pronóstico ✓

---

## 🚨 Solución de Problemas

### Error: "adb: command not found"

**Solución**:
```bash
# En Windows (cmd)
set PATH=%PATH%;C:\Android\Sdk\platform-tools

# En Mac/Linux
export PATH=$PATH:~/Android/Sdk/platform-tools
adb devices
```

### Error: "No se ha podido instalar el paquete"

**Causas comunes**:
- Espacio insuficiente en el dispositivo
- Versión de Android incompatible (requiere Android 5.1+)
- APK corrupta

**Solución**:
```bash
# Desinstala la versión anterior
adb uninstall com.monghit.weatherapp

# Reinstala
adb install -r com.monghit.weatherapp.apk
```

### Error: "Permission denied" al ejecutar adb

En Linux/Mac:
```bash
# Da permisos al ejecutable
chmod +x ~/Android/Sdk/platform-tools/adb

# O usa sudo
sudo ~/Android/Sdk/platform-tools/adb devices
```

### La app no detecta ubicación

1. Verifica en tu Android:
   - **Configuración** → **Privacidad** → **Ubicación** → habilita
   - Asegúrate de que **Weather App** tiene permiso de ubicación

2. En la app:
   - Presiona 📍 nuevamente
   - Espera 10 segundos máximo

3. Si sigue fallando:
   - Desinstala la app
   - Borra datos de la app: **Configuración** → **Aplicaciones** → **Weather App** → **Almacenamiento** → **Borrar datos**
   - Reinstala

### La app es muy lenta

**Posibles causas**:
- API slow (servidor ocupado)
- Conexión de red lenta
- Ubicación tarda mucho (GPS sin señal)

**Soluciones**:
- Intenta en WiFi en lugar de datos móviles
- Busca una ciudad en lugar de usar GPS (más rápido)
- Espera unos segundos

---

## 📊 Especificaciones de la APK

| Propiedad | Valor |
|-----------|-------|
| **Nombre de paquete** | com.monghit.weatherapp |
| **Versión app** | 1.0 |
| **Código de versión** | 1 |
| **Android mínimo** | 5.1 (API 21) |
| **Android objetivo** | 14 (API 34) |
| **Permisos** | INTERNET, LOCATION |
| **Tamaño aproximado** | 100-150 MB (sin comprimir) |
| | 40-60 MB (APK) |

---

## 🔄 Actualizar la Aplicación

Una vez instalada, para actualizar a una versión nueva:

```bash
# Compila la nueva versión
dotnet publish -f net9.0-android -c Release

# Instala sobre la versión anterior
adb install -r WeatherApp/bin/Release/net9.0-android/publish/com.monghit.weatherapp.apk
```

El flag `-r` permite reemplazar sin desinstalar primero.

---

## 📦 Distribución en Google Play Store

Para publicar en Play Store (opcional):

1. Crea una cuenta de desarrollador (pago único de $25)
2. Genera un keystore firmado:
```bash
keytool -genkey -v -keystore my-release-key.keystore \
  -keyalg RSA -keysize 2048 -validity 10000 \
  -alias my-key-alias
```

3. Compila APK firmado:
```bash
dotnet publish -f net9.0-android -c Release \
  -p:AndroidKeyStore=true \
  -p:AndroidSigningKeyAlias=my-key-alias \
  -p:AndroidSigningKeyPass=<password>
```

4. Sube a Google Play Console

---

## ✨ Tips y Trucos

### Compilación rápida
```bash
# Debug (más rápido, mayor tamaño)
dotnet build -f net9.0-android -c Debug

# Release (más lento, menor tamaño, optimizado)
dotnet publish -f net9.0-android -c Release
```

### Ver logs en tiempo real
```bash
# Muestra todos los logs
adb logcat

# Filtrar solo Weather App
adb logcat | findstr "WeatherApp"  # Windows
adb logcat | grep "WeatherApp"     # Mac/Linux
```

### Desinstalar completamente
```bash
adb uninstall com.monghit.weatherapp
```

### Conectar por WiFi (sin USB)
```bash
# En tu PC/Mac
adb tcpip 5555

# En tu Android
# Ve a Configuración → IP → anota la IP

# De vuelta en PC/Mac
adb connect <IP_DEL_ANDROID>:5555
```

---

## 📞 Soporte

Si tienes problemas:

1. Revisa los **ANDROID_BUILD.md** y **ARCHITECTURE.md**
2. Verifica que el dispositivo esté correctamente conectado: `adb devices`
3. Asegúrate de tener la última versión de Android SDK
4. Intenta compilar en Windows si estás en Mac/Linux

¡Disfruta tu Weather App! 🌦️
