# Resumen de Configuración para Render

## Archivos Creados/Modificados

### ✅ Archivos Nuevos:
- `render.yaml` - Configuración de servicios para Render
- `frontend/nginx.conf.template` - Template de configuración de nginx con variables de entorno
- `frontend/docker-entrypoint.sh` - Script para generar nginx.conf desde template
- `RENDER_DEPLOYMENT.md` - Guía completa de despliegue

### ✅ Archivos Modificados:
- `ProductsAPI/Program.cs` - CORS ahora es configurable mediante variables de entorno
- `frontend/Dockerfile` - Actualizado para usar template de nginx con variables de entorno
- `docker-compose.yml` - Agregada variable BACKEND_URL para desarrollo local

## Cómo Funciona

### En Render:
1. Render lee `render.yaml` y crea dos servicios web
2. El servicio `productsapi` se despliega desde `ProductsAPI/Dockerfile`
3. El servicio `frontend` se despliega desde `frontend/Dockerfile`
4. Render automáticamente configura `BACKEND_URL` en el frontend apuntando a la URL del backend
5. El script `docker-entrypoint.sh` genera `nginx.conf` usando la variable `BACKEND_URL`
6. Nginx hace proxy de `/api` al backend usando la URL configurada

### En Desarrollo Local (Docker Compose):
1. `docker-compose.yml` pasa `BACKEND_URL=http://productsapi:5000` al frontend
2. El mismo proceso genera `nginx.conf` con la URL del backend
3. Todo funciona igual que antes

## Variables de Entorno Necesarias en Render

### Backend (productsapi):
- `ConnectionStrings__SqlConnection` - Cadena de conexión a Azure SQL
- `ConnectionStrings__BlobStorageConnection` - Cadena de conexión a Azure Blob Storage
- `Jwt__Key` - Clave secreta para JWT (mínimo 32 caracteres)
- `Cors__AllowedOrigins` - URLs permitidas para CORS (ej: `https://frontend-xxxx.onrender.com`)

### Frontend:
- `BACKEND_URL` - Se configura automáticamente por Render (no necesitas configurarla manualmente)

## Próximos Pasos

1. Sube el código a tu repositorio Git
2. Conecta el repositorio a Render usando Blueprint
3. Configura las variables de entorno en el servicio `productsapi`
4. Espera a que ambos servicios se desplieguen
5. Obtén la URL del frontend y actualiza `Cors__AllowedOrigins` en el backend
6. ¡Listo! Tu aplicación estará en producción

## Notas Importantes

- El archivo `nginx.conf` original se mantiene pero ya no se usa (el Dockerfile usa el template)
- El CORS ahora es configurable, asegúrate de configurarlo con la URL correcta del frontend
- Render usa HTTPS automáticamente, así que las URLs serán `https://...`
- El health check del backend está configurado en `/api/v1/auth/login`

