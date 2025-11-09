# Guía de Despliegue en Render

Esta guía te ayudará a desplegar la aplicación en Render usando el archivo `render.yaml`.

## 📋 Requisitos Previos

1. Cuenta en [Render](https://render.com)
2. Repositorio Git (GitHub, GitLab, o Bitbucket) con el código
3. Variables de entorno preparadas (ver más abajo)

## 🚀 Pasos para Desplegar

### 1. Conectar el Repositorio

1. Inicia sesión en [Render Dashboard](https://dashboard.render.com)
2. Haz clic en "New +" y selecciona "Blueprint"
3. Conecta tu repositorio Git
4. Render detectará automáticamente el archivo `render.yaml`

### 2. Configurar Variables de Entorno

Render creará dos servicios automáticamente. Necesitas configurar las siguientes variables de entorno en cada servicio:

#### Para el servicio `productsapi` (Backend):

Ve a **Dashboard > productsapi > Environment** y agrega:

```
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:5000
ConnectionStrings__SqlConnection=Server=tu-servidor.database.windows.net;Database=prueba_indigo;User Id=usuario;Password=contraseña;
ConnectionStrings__BlobStorageConnection=sp=racwdli&st=...
Jwt__Key=TuClaveSecretaMuyLargaDeAlMenos32CaracteresParaProduccion2024!
Jwt__Issuer=ProductsAPI
Jwt__Audience=ProductsAPIUsers
Cors__AllowedOrigins=https://frontend.onrender.com
```

**Nota:** Reemplaza `https://frontend.onrender.com` con la URL real de tu servicio frontend en Render (la obtendrás después de desplegar).

#### Para el servicio `frontend`:

No necesitas configurar variables manualmente. Render configurará automáticamente `BACKEND_URL` usando la referencia al servicio `productsapi`.

### 3. Desplegar

1. Una vez configuradas las variables de entorno, Render comenzará el despliegue automáticamente
2. Puedes ver el progreso en la pestaña "Events" de cada servicio
3. El despliegue puede tardar varios minutos la primera vez

### 4. Obtener las URLs

Después del despliegue:

1. Ve al Dashboard de Render
2. Encuentra el servicio `productsapi` y copia su URL (ej: `https://productsapi-xxxx.onrender.com`)
3. Encuentra el servicio `frontend` y copia su URL (ej: `https://frontend-xxxx.onrender.com`)

### 5. Actualizar CORS

1. Ve al servicio `productsapi` > Environment
2. Actualiza `Cors__AllowedOrigins` con la URL real del frontend:
   ```
   Cors__AllowedOrigins=https://frontend-xxxx.onrender.com
   ```
3. Guarda los cambios (Render reiniciará automáticamente el servicio)

## 🔧 Configuración Alternativa (Sin Blueprint)

Si prefieres crear los servicios manualmente:

### Backend (productsapi)

1. **New +** > **Web Service**
2. Conecta tu repositorio
3. Configuración:
   - **Name:** `productsapi`
   - **Environment:** `Docker`
   - **Dockerfile Path:** `ProductsAPI/Dockerfile`
   - **Docker Context:** `ProductsAPI`
   - **Health Check Path:** `/api/v1/auth/login`
4. Agrega las variables de entorno mencionadas arriba
5. Haz clic en **Create Web Service**

### Frontend

1. **New +** > **Web Service**
2. Conecta tu repositorio
3. Configuración:
   - **Name:** `frontend`
   - **Environment:** `Docker`
   - **Dockerfile Path:** `frontend/Dockerfile`
   - **Docker Context:** `frontend`
4. En **Environment Variables**, agrega:
   - **Key:** `BACKEND_URL`
   - **Value:** Selecciona **Secret** y luego **Link to productsapi** > **Host**
5. Haz clic en **Create Web Service**

## 🔒 Consideraciones de Seguridad

1. **Nunca commitees credenciales** - Usa siempre variables de entorno en Render
2. **Usa Secrets de Render** - Para valores sensibles, marca las variables como "Secret" en Render
3. **CORS configurado** - Asegúrate de configurar solo los orígenes permitidos
4. **JWT Key fuerte** - Usa una clave de al menos 32 caracteres

## 🐛 Troubleshooting

### El frontend no puede conectar con el backend

- Verifica que `BACKEND_URL` esté configurada correctamente en el servicio frontend
- Asegúrate de que el servicio backend esté funcionando (revisa los logs)
- Verifica que CORS esté configurado con la URL correcta del frontend

### Error 502 Bad Gateway

- Revisa los logs del servicio backend en Render
- Verifica que el health check esté funcionando
- Asegúrate de que todas las variables de entorno estén configuradas

### Error de conexión a la base de datos

- Verifica que Azure SQL Database permita conexiones desde las IPs de Render
- Revisa la cadena de conexión SQL
- Asegúrate de que el firewall de Azure SQL tenga las reglas correctas

## 📝 Comandos Útiles

Puedes ver los logs en tiempo real desde el Dashboard de Render, o usar la CLI de Render:

```bash
# Instalar Render CLI
npm install -g render-cli

# Ver logs
render logs --service productsapi
render logs --service frontend
```

## 💰 Costos

- **Starter Plan:** Gratis (con limitaciones)
- **Standard Plan:** Desde $7/mes por servicio
- Consulta [Render Pricing](https://render.com/pricing) para más detalles

## 🔄 Actualizaciones Automáticas

Render puede configurarse para hacer deploy automático cuando haces push a la rama principal:

1. Ve a tu servicio > **Settings**
2. En **Auto-Deploy**, selecciona la rama (ej: `main` o `master`)
3. Cada push a esa rama desplegará automáticamente

