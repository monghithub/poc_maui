# Obtener el APK Compilado desde GitHub Actions

## 📥 Descargar el APK

### Opción 1: Desde la interfaz web de GitHub (Más Fácil)

1. Abre https://github.com/monghithub/poc_maui
2. Haz clic en la pestaña **Actions** (arriba en el repositorio)
3. Haz clic en el último workflow ejecutado (verás el nombre del commit)
4. En la sección **Artifacts**, descarga `weather-app-apk`
5. Descomprime el ZIP para obtener `com.monghit.weatherapp.apk`

### Opción 2: Desde la línea de comandos

```bash
# Descargar el último artefacto
gh run list -R monghithub/poc_maui --limit 1

# Copiar el ID del run de la salida anterior
gh run download <RUN_ID> -R monghithub/poc_maui -n weather-app-apk
```

## 📱 Instalar en tu Dispositivo Android

### Con ADB (Recomendado)

```bash
# Conecta tu dispositivo Android vía USB (modo de desarrollador activado)
adb install -r com.monghit.weatherapp.apk
```

### Sin ADB (Desde el dispositivo)

1. Descarga el APK en tu teléfono
2. Abre el gestor de archivos
3. Navega al APK descargado
4. Toca para instalar
5. Otorga los permisos cuando se solicite

## 🔄 Flujo de Compilación Automática

```
┌──────────────┐
│ Hacer push   │
│ a git        │
└──────┬───────┘
       │
       ▼
┌──────────────────────────┐
│ GitHub Actions activa    │
│ el workflow              │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│ Compila en macOS         │
│ (MAUI completamente      │
│  soportado aquí)         │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│ Genera APK Release       │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│ Sube como Artifact       │
│ a GitHub                 │
└──────┬───────────────────┘
       │
       ▼
┌──────────────────────────┐
│ Descarga desde Actions   │
│ Instala en tu móvil      │
└──────────────────────────┘
```

## 🎯 Crear una Release Etiquetada

Si quieres crear una versión específica:

```bash
# Crear un tag
git tag v1.0.0
git push github v1.0.0

# El workflow automáticamente creará una Release en GitHub
# con el APK adjunto
```

Luego verás la release en: https://github.com/monghithub/poc_maui/releases

## ✅ Verificación

Después de instalar, la app debería:
- ✅ Solicitar permisos de ubicación
- ✅ Mostrar el clima actual (si no das ubicación, usa Londres por defecto)
- ✅ Permitir buscar por ciudad
- ✅ Mostrar historial de búsquedas
- ✅ Mostrar pronóstico de 7 días

## 🐛 Solución de Problemas

### "Instalación bloqueada por seguridad"
- Habilita **Instalar desde fuentes desconocidas** en Configuración → Seguridad

### "No se puede instalar en este dispositivo"
- Verifica que tu dispositivo sea Android 8.0+ (API 26+)
- Intenta desinstalar versiones anteriores primero: `adb uninstall com.monghit.weatherapp`

### "Aplicación se bloquea al abrir"
- Intenta nuevamente instalar sin dejar versiones anteriores
- Verifica que tengas habilitados los permisos de ubicación

## 📞 Más Ayuda

Si tienes problemas:
1. Verifica los logs del workflow en Actions
2. Asegúrate de que tu dispositivo cumpla los requisitos
3. Intenta con un dispositivo/emulador diferente
