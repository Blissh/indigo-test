# ProductsAPI - API REST para Gestión de Productos y Ventas

API REST desarrollada con .NET 9 Minimal API para la gestión de productos, ventas y reportes. Incluye autenticación JWT, almacenamiento en Azure Blob Storage y documentación interactiva con Scalar.

## 📋 Tabla de Contenidos

- [Características](#características)
- [Tecnologías](#tecnologías)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Configuración](#configuración)
- [Documentación de la API](#documentación-de-la-api)
- [Ejecución con Docker Compose](#ejecución-con-docker-compose)
- [Ejemplos de Uso](#ejemplos-de-uso)

## ✨ Características

- ✅ Autenticación JWT con tokens seguros
- ✅ CRUD completo de productos
- ✅ Gestión de ventas con validación de stock
- ✅ Reportes de ventas con métricas y análisis
- ✅ Almacenamiento de archivos en Azure Blob Storage
- ✅ Documentación interactiva con Scalar
- ✅ Validación de datos con Data Annotations
- ✅ Manejo de excepciones y logging
- ✅ Arquitectura limpia con separación de responsabilidades

## 🛠 Tecnologías

- **.NET 9** - Framework principal
- **Entity Framework Core 9** - ORM para acceso a datos
- **SQL Server** - Base de datos relacional
- **JWT Bearer** - Autenticación y autorización
- **Azure Blob Storage** - Almacenamiento de archivos
- **Scalar** - Documentación interactiva de API
- **Docker** - Contenedorización
- **Docker Compose** - Orquestación de contenedores

## 📁 Estructura del Proyecto

```
ProductsAPI/
├── API/                          # Endpoints de la API
│   ├── AuthenticationEndpoints.cs
│   ├── ProductEndpoints.cs
│   ├── SalesEndpoints.cs
│   └── ReportsEndpoints.cs
├── Application/                  # Lógica de aplicación
│   ├── Interfaces/              # Contratos de servicios
│   └── Services/                # Implementación de servicios
├── Domain/                       # Entidades y DTOs
│   ├── DTOs/
│   │   ├── Requests/            # DTOs de entrada
│   │   ├── Responses/           # DTOs de salida
│   │   └── Data/                # DTOs de datos/reportes
│   └── Entities/                # Entidades del dominio
├── Infrastructure/               # Infraestructura
│   ├── Persistence/             # DbContext y configuración
│   └── Services/                # Servicios de infraestructura
├── Extensions/                   # Extensiones
│   └── ServiceExtensions.cs     # Configuración de servicios
├── Program.cs                    # Punto de entrada
├── Dockerfile                    # Imagen Docker
├── docker-compose.yml            # Configuración Docker Compose
└── appsettings.json              # Configuración de la aplicación
```

## ⚙️ Configuración

### Variables de Entorno Requeridas

La aplicación requiere las siguientes variables de entorno o configuración en `appsettings.json`:

#### Connection Strings
- `ConnectionStrings:SqlConnection` - Cadena de conexión a SQL Server
- `ConnectionStrings:BlobStorageConnection` - Cadena de conexión a Azure Blob Storage

#### JWT Configuration
- `Jwt:Key` - Clave secreta para firmar tokens JWT (mínimo 32 caracteres)
- `Jwt:Issuer` - Emisor del token
- `Jwt:Audience` - Audiencia del token

#### Ejemplo de appsettings.json
```json
{
  "ConnectionStrings": {
    "SqlConnection": "Server=tu-servidor;Database=tu-db;User Id=tu-usuario;Password=tu-password;",
    "BlobStorageConnection": "tu-cadena-de-conexion-blob-storage"
  },
  "Jwt": {
    "Key": "TuClaveSecretaDeAlMenos32CaracteresParaJWT2024!",
    "Issuer": "ProductsAPI",
    "Audience": "ProductsAPIUsers"
  }
}
```

## 📚 Documentación de la API

### Base URL
```
http://localhost:5000/scalar/v1
http://localhost:5000/api/v1
```

### Autenticación

Todos los endpoints (excepto `/auth/login` y `/auth/register`) requieren autenticación mediante Bearer Token JWT.

**Formato del Header:**
```
Authorization: Bearer {token}
```

---

## 🔐 Endpoints de Autenticación

### POST `/auth/register`
Registra un nuevo usuario en el sistema.

**Request Body:**
```json
{
  "name": "Juan Pérez",
  "email": "juan@example.com",
  "password": "password123",
  "confirmPassword": "password123"
}
```

**Response (200 OK):**
```json
{
  "id": 1,
  "name": "Juan Pérez",
  "email": "juan@example.com",
  "createdAt": "2024-01-15T10:30:00Z",
  "updatedAt": "2024-01-15T10:30:00Z"
}
```

**Errores:**
- `400 Bad Request` - Email duplicado o datos inválidos

---

### POST `/auth/login`
Inicia sesión y obtiene un token JWT.

**Request Body:**
```json
{
  "email": "juan@example.com",
  "password": "password123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "user": {
    "id": 1,
    "name": "Juan Pérez",
    "email": "juan@example.com",
    "createdAt": "2024-01-15T10:30:00Z",
    "updatedAt": "2024-01-15T10:30:00Z"
  },
  "expiresAt": "2024-01-15T11:00:00Z"
}
```

**Errores:**
- `400 Bad Request` - Solicitud inválida
- `401 Unauthorized` - Credenciales inválidas

---

### GET `/auth/health`
Verifica el estado de la API de autenticación (requiere autenticación).

**Response (200 OK):**
```json
{
  "message": "API de autenticación está funcionando correctamente"
}
```

---

## 📦 Endpoints de Productos

Todos los endpoints de productos requieren autenticación.

### GET `/products`
Obtiene todos los productos.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Productos obtenidos exitosamente",
  "data": [
    {
      "id": 1,
      "name": "Laptop Dell",
      "description": "Laptop Dell XPS 15",
      "price": 1500.00,
      "stock": 10,
      "image": "https://..."
    }
  ]
}
```

---

### GET `/products/{id}`
Obtiene un producto por su ID.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Producto obtenido exitosamente",
  "data": {
    "id": 1,
    "name": "Laptop Dell",
    "description": "Laptop Dell XPS 15",
    "price": 1500.00,
    "stock": 10,
    "image": "https://..."
  }
}
```

**Errores:**
- `404 Not Found` - Producto no encontrado

---

### POST `/products`
Crea un nuevo producto.

**Request Body:**
```json
{
  "name": "Laptop Dell",
  "description": "Laptop Dell XPS 15",
  "price": 1500.00,
  "stock": 10,
  "image": "https://example.com/image.jpg"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Producto creado exitosamente",
  "data": {
    "id": 1,
    "name": "Laptop Dell",
    "description": "Laptop Dell XPS 15",
    "price": 1500.00,
    "stock": 10,
    "image": "https://example.com/image.jpg"
  }
}
```

**Errores:**
- `400 Bad Request` - Datos inválidos

---

### PUT `/products/{id}`
Actualiza un producto existente.

**Request Body:**
```json
{
  "name": "Laptop Dell Actualizada",
  "description": "Nueva descripción",
  "price": 1600.00,
  "stock": 15,
  "image": "https://example.com/new-image.jpg"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Producto actualizado exitosamente",
  "data": {
    "id": 1,
    "name": "Laptop Dell Actualizada",
    "description": "Nueva descripción",
    "price": 1600.00,
    "stock": 15,
    "image": "https://example.com/new-image.jpg"
  }
}
```

**Errores:**
- `404 Not Found` - Producto no encontrado
- `400 Bad Request` - Datos inválidos

---

### DELETE `/products/{id}`
Elimina un producto.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Producto eliminado exitosamente",
  "data": {
    "id": 1,
    "name": "Laptop Dell",
    "description": "Laptop Dell XPS 15",
    "price": 1500.00,
    "stock": 10,
    "image": "https://..."
  }
}
```

**Errores:**
- `404 Not Found` - Producto no encontrado

---

## 🛒 Endpoints de Ventas

Todos los endpoints de ventas requieren autenticación.

### POST `/sales`
Crea una nueva venta. Valida existencia de productos y stock disponible.

**Request Body:**
```json
{
  "items": [
    {
      "productId": 1,
      "quantity": 2
    },
    {
      "productId": 2,
      "quantity": 1
    }
  ]
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Venta creada exitosamente",
  "data": {
    "id": 1,
    "date": "2024-01-15T10:30:00Z",
    "total": 3200.00,
    "items": [
      {
        "id": 1,
        "saleId": 1,
        "productId": 1,
        "productName": "Laptop Dell",
        "quantity": 2,
        "price": 1500.00,
        "total": 3000.00,
        "createdAt": "2024-01-15T10:30:00Z",
        "updatedAt": "2024-01-15T10:30:00Z"
      },
      {
        "id": 2,
        "saleId": 1,
        "productId": 2,
        "productName": "Mouse Logitech",
        "quantity": 1,
        "price": 200.00,
        "total": 200.00,
        "createdAt": "2024-01-15T10:30:00Z",
        "updatedAt": "2024-01-15T10:30:00Z"
      }
    ]
  }
}
```

**Errores:**
- `400 Bad Request` - Productos no encontrados o stock insuficiente

---

### GET `/sales`
Obtiene todas las ventas.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Ventas obtenidas exitosamente",
  "data": [
    {
      "id": 1,
      "date": "2024-01-15T10:30:00Z",
      "total": 3200.00,
      "items": [...]
    }
  ]
}
```

---

### GET `/sales/{id}`
Obtiene una venta por su ID.

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Venta obtenida exitosamente",
  "data": {
    "id": 1,
    "date": "2024-01-15T10:30:00Z",
    "total": 3200.00,
    "items": [...]
  }
}
```

**Errores:**
- `404 Not Found` - Venta no encontrada

---

### DELETE `/sales/{id}`
Elimina una venta y todos sus items (eliminación en cascada).

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Venta eliminada exitosamente",
  "data": {
    "id": 1,
    "date": "2024-01-15T10:30:00Z",
    "total": 3200.00,
    "items": [...]
  }
}
```

**Errores:**
- `404 Not Found` - Venta no encontrada

---

## 📊 Endpoints de Reportes

Todos los endpoints de reportes requieren autenticación.

### GET `/reports/sales`
Genera un reporte completo de ventas en un rango de fechas.

**Query Parameters:**
- `startDate` (opcional) - Fecha de inicio (formato: `YYYY-MM-DD`). Por defecto: último mes
- `endDate` (opcional) - Fecha de fin (formato: `YYYY-MM-DD`). Por defecto: fecha actual

**Ejemplo de Request:**
```
GET /api/v1/reports/sales?startDate=2024-01-01&endDate=2024-01-31
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Reporte de ventas generado exitosamente",
  "data": {
    "startDate": "2024-01-01T00:00:00Z",
    "endDate": "2024-01-31T00:00:00Z",
    "totalSales": 150,
    "totalRevenue": 45000.00,
    "averageTicket": 300.00,
    "topProducts": [
      {
        "productId": 1,
        "productName": "Laptop Dell",
        "productImage": "https://...",
        "quantitySold": 50,
        "revenue": 75000.00,
        "ranking": 1
      }
    ],
    "dailySummary": [
      {
        "date": "2024-01-01T00:00:00Z",
        "totalSales": 10,
        "totalRevenue": 3000.00,
        "totalProducts": 5
      }
    ]
  }
}
```

**Errores:**
- `400 Bad Request` - Rango de fechas inválido (startDate > endDate)

---

## 🐳 Ejecución con Docker Compose

### Prerrequisitos

- Docker Desktop instalado y ejecutándose
- Docker Compose v3.8 o superior

### Configuración de Variables de Entorno

El archivo `docker-compose.yml` utiliza variables de entorno del sistema con valores por defecto. Puedes configurarlas de dos formas:

#### Opción 1: Variables de Entorno del Sistema

Crea un archivo `.env` en la raíz del proyecto o configura las variables en tu sistema:

```bash
# Windows PowerShell
$env:SQL_CONNECTION="Server=tu-servidor;Database=tu-db;User Id=tu-usuario;Password=tu-password;"
$env:BLOB_STORAGE_CONNECTION="tu-cadena-de-conexion-blob-storage"
$env:JWT_KEY="TuClaveSecretaDeAlMenos32CaracteresParaJWT2024!"
$env:JWT_ISSUER="ProductsAPI"
$env:JWT_AUDIENCE="ProductsAPIUsers"

# Linux/Mac
export SQL_CONNECTION="Server=tu-servidor;Database=tu-db;User Id=tu-usuario;Password=tu-password;"
export BLOB_STORAGE_CONNECTION="tu-cadena-de-conexion-blob-storage"
export JWT_KEY="TuClaveSecretaDeAlMenos32CaracteresParaJWT2024!"
export JWT_ISSUER="ProductsAPI"
export JWT_AUDIENCE="ProductsAPIUsers"
```

#### Opción 2: Modificar docker-compose.yml

Edita directamente el archivo `docker-compose.yml` y reemplaza los valores por defecto en las variables de entorno.

### Ejecución

#### 1. Construir y Ejecutar el Contenedor

Desde la raíz del proyecto (donde está el `docker-compose.yml`):

```bash
docker-compose up --build
```

Este comando:
- Construye la imagen Docker si no existe
- Crea y ejecuta el contenedor `productsapi`
- Expone el puerto `5000` en el host

#### 2. Ejecutar en Segundo Plano

Para ejecutar el contenedor en modo detached (segundo plano):

```bash
docker-compose up -d --build
```

#### 3. Ver Logs

```bash
# Ver todos los logs
docker-compose logs

# Seguir los logs en tiempo real
docker-compose logs -f

# Ver logs del servicio específico
docker-compose logs productsapi
```

#### 4. Detener el Contenedor

```bash
# Detener sin eliminar
docker-compose stop

# Detener y eliminar contenedores
docker-compose down

# Detener, eliminar contenedores y volúmenes
docker-compose down -v
```

### Verificación del Estado

#### Verificar que el Contenedor Está Ejecutándose

```bash
docker-compose ps
```

Deberías ver algo como:
```
NAME          IMAGE          STATUS          PORTS
productsapi   productsapi    Up 2 minutes    0.0.0.0:5000->5000/tcp
```

#### Verificar Health Check

El contenedor incluye un health check que verifica cada 30 segundos si la API responde:

```bash
docker inspect productsapi | grep -A 10 Health
```

#### Probar la API

Una vez que el contenedor esté ejecutándose, puedes probar la API:

```bash
# Verificar que la API responde
curl http://localhost:5000/api/v1/auth/login

# O abrir en el navegador
http://localhost:5000/api/v1/auth/login
```

### Acceso a la Documentación

En modo desarrollo, la documentación interactiva está disponible en:

```
http://localhost:5000/scalar/v1
```

### Configuración del Health Check

El health check está configurado en `docker-compose.yml`:

```yaml
healthcheck:
  test: ["CMD-SHELL", "curl -f http://localhost:5000/api/v1/auth/login || exit 1"]
  interval: 30s      # Verifica cada 30 segundos
  timeout: 10s       # Timeout de 10 segundos
  retries: 3         # Reintenta 3 veces antes de marcar como no saludable
  start_period: 40s  # Período de gracia de 40 segundos al iniciar
```

### Solución de Problemas

#### El contenedor no inicia

1. Verifica que Docker Desktop esté ejecutándose
2. Revisa los logs: `docker-compose logs productsapi`
3. Verifica que el puerto 5000 no esté en uso: `netstat -an | findstr :5000` (Windows) o `lsof -i :5000` (Linux/Mac)

#### Error de conexión a la base de datos

1. Verifica que la cadena de conexión SQL sea correcta
2. Asegúrate de que la base de datos sea accesible desde el contenedor
3. Revisa los logs para ver el error específico

#### Error de autenticación JWT

1. Verifica que `JWT_KEY` tenga al menos 32 caracteres
2. Asegúrate de que `JWT_ISSUER` y `JWT_AUDIENCE` estén configurados

#### Reconstruir la imagen desde cero

```bash
docker-compose build --no-cache
docker-compose up
```

---

## 💡 Ejemplos de Uso

### Flujo Completo de Uso

#### 1. Registrar un Usuario

```bash
curl -X POST http://localhost:5000/api/v1/auth/register \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Juan Pérez",
    "email": "juan@example.com",
    "password": "password123",
    "confirmPassword": "password123"
  }'
```

#### 2. Iniciar Sesión y Obtener Token

```bash
curl -X POST http://localhost:5000/api/v1/auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "juan@example.com",
    "password": "password123"
  }'
```

Guarda el token de la respuesta.

#### 3. Crear un Producto

```bash
curl -X POST http://localhost:5000/api/v1/products \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {tu-token}" \
  -d '{
    "name": "Laptop Dell",
    "description": "Laptop Dell XPS 15",
    "price": 1500.00,
    "stock": 10,
    "image": "https://example.com/image.jpg"
  }'
```

#### 4. Crear una Venta

```bash
curl -X POST http://localhost:5000/api/v1/sales \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer {tu-token}" \
  -d '{
    "items": [
      {
        "productId": 1,
        "quantity": 2
      }
    ]
  }'
```

#### 5. Generar Reporte de Ventas

```bash
curl -X GET "http://localhost:5000/api/v1/reports/sales?startDate=2024-01-01&endDate=2024-01-31" \
  -H "Authorization: Bearer {tu-token}"
```

---

## 📝 Notas Adicionales

- Los tokens JWT expiran después de 30 minutos
- La eliminación de ventas es en cascada (elimina automáticamente los items asociados)
- Las ventas actualizan automáticamente el stock de los productos
- El health check verifica que la API esté respondiendo correctamente
- La documentación interactiva está disponible solo en modo desarrollo

---

## 🔒 Seguridad

- Las contraseñas se almacenan con hash usando `PasswordHasher`
- Los tokens JWT se validan en cada solicitud autenticada
- Todas las rutas (excepto login y register) requieren autenticación
- Las validaciones de datos se realizan con Data Annotations

---

## 📄 Licencia

Este proyecto es privado, de prueba y está destinado para uso interno.

