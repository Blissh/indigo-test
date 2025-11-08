# Docker - ProductsAPI

## 🐳 Construcción y Ejecución

### Construir la imagen
```bash
docker build -t productsapi:latest .
```

### Ejecutar el contenedor
```bash
docker run -d \
  --name productsapi \
  -p 5000:5000 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ConnectionStrings__SqlConnection="tu-connection-string" \
  -e ConnectionStrings__BlobStorageConnection="tu-blob-connection" \
  -e Jwt__Key="tu-jwt-key" \
  -e Jwt__Issuer="ProductsAPI" \
  -e Jwt__Audience="ProductsAPIUsers" \
  productsapi:latest
```

### Usar Docker Compose
```bash
docker-compose up -d
```

## 📋 Variables de Entorno

Las siguientes variables de entorno pueden ser configuradas:

| Variable | Descripción | Ejemplo |
|----------|-------------|---------|
| `ASPNETCORE_ENVIRONMENT` | Entorno de ejecución | `Development` |
| `ASPNETCORE_URLS` | URLs donde escucha la API | `http://+:5000` |
| `ConnectionStrings__SqlConnection` | Cadena de conexión SQL Server | `Server=...;Database=...` |
| `ConnectionStrings__BlobStorageConnection` | Cadena de conexión Blob Storage | `sp=...` |
| `Jwt__Key` | Clave secreta JWT | `tu-clave-secreta` |
| `Jwt__Issuer` | Emisor del token JWT | `ProductsAPI` |
| `Jwt__Audience` | Audiencia del token JWT | `ProductsAPIUsers` |

**Nota:** En .NET, las variables de entorno con doble guion bajo (`__`) se mapean a secciones anidadas en `appsettings.json`.

## 🔍 Verificar el contenedor

```bash
# Ver logs
docker logs productsapi

# Verificar salud del contenedor
docker ps

# Acceder al contenedor
docker exec -it productsapi /bin/bash
```

## 🌐 Acceder a la API

Una vez ejecutado el contenedor, la API estará disponible en:
- **API**: http://localhost:5000/api/v1
- **Swagger/Scalar**: http://localhost:5000/scalar/v1 (en Development)

## 🔐 Seguridad

- El contenedor ejecuta como usuario no-root (`appuser`)
- Healthcheck configurado para monitoreo
- Variables de entorno para configuración sensible

