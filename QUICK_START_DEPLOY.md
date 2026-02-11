# Quick Start: Instalar en tu Móvil Android

Tu dispositivo está conectado (acfa8848). Aquí está la forma más rápida de instalar la app.

## 🎯 Opción Recomendada (5 minutos en Windows/Mac)

### Paso 1: Preparar tu Android
```
En el móvil:
1. Configuración → Información → Número de Compilación (toca 7 veces)
2. Vuelve atrás
3. Opciones de Desarrollador → Depuración USB (ON)
4. Conecta por USB a tu PC/Mac
5. Toca "Confiar" si aparece dialogo
```

### Paso 2: Compilar en Windows o Mac
```bash
# En Windows/Mac con .NET SDK instalado:
cd WeatherApp
dotnet restore
dotnet publish -f net9.0-android -c Release
```

### Paso 3: Instalar
```bash
# Con ADB (recomendado)
adb install -r WeatherApp/bin/Release/net9.0-android/publish/com.monghit.weatherapp.apk

# O copia el APK al móvil y toca para instalar
```

**¡Listo!** 🎉 Abre "Weather App" en tu móvil.

---

## ☁️ Alternativa: Compilar en la Nube (Gratis)

Si no tienes Windows/Mac:

### 1. Sube a GitHub
```bash
git remote add origin https://github.com/tu-usuario/tu-repo.git
git push -u origin master
```

### 2. Espera 3-5 minutos
El workflow se ejecuta automáticamente en GitHub Actions.

### 3. Descarga el APK
- GitHub → Actions → Build Android APK → Download artifact
- O espera el email de GitHub

### 4. Instala en tu móvil
```bash
# Conecta por USB y:
adb install -r weather-app-apk/com.monghit.weatherapp.apk

# O: Conecta por WiFi (sin cable)
adb tcpip 5555
adb connect <tu-ip>:5555
```

---

## 📱 Sin ADB (Método Manual)

Si no tienes ADB disponible:

1. **Descarga el APK** a tu PC/Mac
2. **Copia el APK a tu móvil** (por email, Google Drive, etc.)
3. **En tu Android**:
   - Abre el navegador de archivos
   - Encuentra el APK
   - Toca → Instalar
   - Toca "Abrir"

---

## ✅ Verificar que funciona

Una vez abierta la app en tu móvil:

- ✅ Solicita permiso de ubicación → Concede
- ✅ Muestra clima de tu ubicación actual
- ✅ Botón 🔍 buscar ciudad
- ✅ Botón 📋 ver historial
- ✅ Botón 📅 ver pronóstico 7 días

---

## 🚨 Si no funciona

**"MAUI SDK not found"** en Linux/Mac:
→ Necesitas Windows para compilar MAUI
→ Usa GitHub Actions en su lugar

**"APK no se instala"**:
```bash
adb uninstall com.monghit.weatherapp
adb install -r com.monghit.weatherapp.apk
```

**"adb no funciona"**:
```bash
# Windows
set PATH=%PATH%;C:\Android\Sdk\platform-tools

# Mac/Linux
export PATH=$PATH:~/Android/Sdk/platform-tools
```

**"No detecta ubicación"**:
- Android: Configuración → Privacidad → Ubicación (ON)
- App: Permite permiso cuando se solicita
- Presiona 📍 nuevamente

---

## 📖 Documentación Completa

Para información detallada, mira:
- **DEPLOY_MOBILE.md** - Guía completa con troubleshooting
- **ANDROID_BUILD.md** - Detalles técnicos de compilación
- **FEATURES.md** - Qué puede hacer la app

---

**Tiempo estimado**:
- Windows/Mac: 5-10 minutos (compilación + instalación)
- GitHub Actions: 5 minutos (esperar) + 2 minutos (instalar)
- Manual: 10 minutos (sin compilación)

¡Disfruta! 🌦️
