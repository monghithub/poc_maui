# 📥 Descargar e Instalar APK desde GitHub

Tu código ya está en GitHub y **GitHub Actions está compilando la APK automáticamente** ⚙️

## 🔗 Tu Repositorio

```
https://github.com/monghithub/poc_maui
```

## ⏳ Estado Actual

✅ Código pusheado
🔨 **GitHub Actions compilando** (5-10 minutos)
⏳ APK casi lista

---

## 📍 Paso 1: Ir a GitHub Actions

1. Abre: https://github.com/monghithub/poc_maui
2. Haz clic en la pestaña **"Actions"**
3. Deberías ver un workflow corriendo: **"Build Android APK"**

```
https://github.com/monghithub/poc_maui/actions
```

---

## ⏳ Paso 2: Esperar a que termine (5-10 minutos)

El estado mostrará:

```
🟡 RUNNING  →  ✅ COMPLETED
```

Verás algo como:
```
Build Android APK
└─ Setup .NET ✅
   Restore dependencies ✅
   Build APK ✅
   Upload artifact ✅
```

---

## 📥 Paso 3: Descargar el APK

Una vez completado (status ✅):

### Opción A: Desde GitHub Actions (RECOMENDADO)
1. Entra en el workflow completado
2. Baja hasta **"Artifacts"**
3. Haz clic en **"weather-app-apk"**
4. Se descargará un ZIP con el APK

### Opción B: Desde Releases (Si creaste un tag)
```bash
# Para crear una release oficial:
git tag v1.0.0
git push github v1.0.0
```

Luego:
1. Ve a: https://github.com/monghithub/poc_maui/releases
2. Descarga **WeatherApp.apk**

---

## 📱 Paso 4: Instalar en tu Móvil

### Método 1: Con ADB (Recomendado - 30 segundos)

```bash
# Descomprime el ZIP si es necesario
# Luego:
adb install -r com.monghit.weatherapp.apk

# Espera el mensaje: "Success"
```

### Método 2: Por Bluetooth/WiFi

1. **Sube el APK a Google Drive**
2. **Descárgalo en tu Android**
3. **Abre el APK**
4. **Toca "Instalar"**

### Método 3: Por USB directo

1. **Copia el APK a tu Android** vía USB
2. **Abre el APK con el navegador de archivos**
3. **Toca "Instalar"**

---

## ✅ Verificar la Instalación

Una vez abierta la app:

```
☑️ Solicita permiso de ubicación
☑️ Muestra tu clima actual
☑️ Botón 🔍 funciona
☑️ Botón 📋 muestra historial
☑️ Botón 📅 abre pronóstico
```

---

## 🔄 Actualizar la App

Cada vez que hagas push a master, GitHub Actions compila una nueva APK:

```bash
# Hacer cambios
git add .
git commit -m "feat: nueva característica"
git push github master

# En 5-10 minutos, nueva APK disponible
# Descarga y reinstala:
adb install -r com.monghit.weatherapp.apk
```

---

## 📊 Información de la APK

| Propiedad | Valor |
|-----------|-------|
| **Paquete** | com.monghit.weatherapp |
| **Versión** | 1.0 |
| **Android mín.** | 5.1 (API 21) |
| **Android máx.** | 14+ (API 34+) |
| **Tamaño** | ~40-60 MB |
| **Arquitecturas** | arm64-v8a (ARM), x86_64 |

---

## 🚨 Solución de Problemas

### "El workflow aún está corriendo"
```
⏳ Espera 5-10 minutos
   Los primeros builds toman más tiempo
```

### "No encuentro el artifact"
```
1. Verifica que el workflow diga ✅ "Success"
2. Scroll down en la página de detalles
3. Busca la sección "Artifacts"
```

### "Artifact no está descargando"
```
1. Abre en Firefox o Chrome
2. Intenta incógnito/privado
3. Baja a la sección Artifacts nuevamente
```

### "El APK no instala"
```bash
# Desinstala la versión anterior
adb uninstall com.monghit.weatherapp

# Reinstala
adb install -r com.monghit.weatherapp.apk
```

### "Error de conexión en GitHub"
```bash
# Verifica tu conexión SSH
ssh -T git@github.com

# Debería responder:
# "Hi [tu usuario]! You've successfully authenticated..."
```

---

## 📋 Checklist Completo

- [ ] Código pusheado a GitHub ✅ (YA HECHO)
- [ ] GitHub Actions compilando ✅ (EN PROGRESO)
- [ ] APK descargada desde Artifacts
- [ ] APK instalada en móvil con `adb install`
- [ ] App abierta en el móvil
- [ ] Permiso de ubicación concedido
- [ ] Clima actual mostrando ✅

---

## 🎯 Resumen

```
┌─────────────────────────────────────┐
│ 1. GitHub está compilando           │
│    (5-10 minutos)                   │
├─────────────────────────────────────┤
│ 2. Descarga el APK                  │
│    (Pestaña Actions → Artifacts)    │
├─────────────────────────────────────┤
│ 3. Instala en tu móvil              │
│    (adb install -r [APK])           │
├─────────────────────────────────────┤
│ 4. Abre la app                      │
│    (Weather App en aplicaciones)    │
└─────────────────────────────────────┘
```

---

## 🔗 Enlaces Útiles

- **Repositorio**: https://github.com/monghithub/poc_maui
- **Actions**: https://github.com/monghithub/poc_maui/actions
- **Código**: https://github.com/monghithub/poc_maui/tree/master/WeatherApp

---

## ⏱️ Cronograma

```
NOW:     Código pusheado ✅
+2 min:  GitHub Actions iniciado
+5 min:  Compilación en progreso
+8 min:  APK lista para descargar
+10min:  Tu app instalada en el móvil
+11min:  ¡Disfrutando Weather App! 🎉
```

---

**¿Necesitas ayuda?** Revisa QUICK_START_DEPLOY.md o DEPLOY_MOBILE.md

¡La APK estará lista en pocos minutos! ⏳
