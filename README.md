# Indigo Test - Sistema de Gestión de Productos y Ventas

Sistema completo de gestión de productos y ventas desarrollado con .NET 9 (backend) y React + TypeScript (frontend). Incluye autenticación JWT, gestión de productos, ventas y reportes con visualizaciones.

## 📋 Tabla de Contenidos

- [Características](#características)
- [Tecnologías](#tecnologías)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Configuración](#configuración)
- [Ejecución con Docker Compose](#ejecución-con-docker-compose)
- [Desarrollo Local](#desarrollo-local)
- [Documentación de la API](#documentación-de-la-api)

## ✨ Características

### Backend (ProductsAPI)
- ✅ Autenticación JWT con tokens seguros
- ✅ CRUD completo de productos
- ✅ Gestión de ventas con validación de stock
- ✅ Reportes de ventas con métricas y análisis
- ✅ Almacenamiento de archivos en Azure Blob Storage
- ✅ Documentación interactiva con Scalar
- ✅ Validación de datos con Data Annotations
- ✅ Arquitectura limpia con separación de responsabilidades

### Frontend (React)
- ✅ Interfaz de usuario moderna y responsive
- ✅ Autenticación y registro de usuarios
- ✅ Gestión completa de productos (CRUD)
- ✅ Creación y visualización de ventas
- ✅ Reportes con gráficos interactivos (Recharts)
- ✅ Carga de imágenes para productos
- ✅ Rutas protegidas con autenticación
- ✅ Manejo de errores y estados de carga

## 🛠 Tecnologías

### Backend
- **.NET 9** - Framework principal
- **Entity Framework Core 9** - ORM para acceso a datos
- **SQL Server** - Base de datos relacional
- **JWT Bearer** - Autenticación y autorización
- **Azure Blob Storage** - Almacenamiento de archivos
- **Scalar** - Documentación interactiva de API

### Frontend
- **React 19** - Biblioteca de UI
- **TypeScript** - Tipado estático
- **Vite** - Build tool y dev server
- **React Router** - Enrutamiento
- **Axios** - Cliente HTTP
- **Recharts** - Gráficos y visualizaciones
- **CSS Modules** - Estilos modulares

### Infraestructura
- **Docker** - Contenedorización
- **Docker Compose** - Orquestación de contenedores
- **Nginx** - Servidor web para frontend

## 📁 Estructura del Proyecto

```
indigo-test/
├── ProductsAPI/              # Backend API (.NET 9)
│   ├── API/                  # Endpoints de la API
│   ├── Application/         # Lógica de aplicación
│   ├── Domain/              # Entidades y DTOs
│   ├── Infrastructure/      # Persistencia y servicios
│   ├── Dockerfile           # Imagen Docker del backend
│   └── docker-compose.yml   # Docker Compose del backend (legacy)
│
├── frontend/                # Frontend (React + TypeScript)
│   ├── src/
│   │   ├── api/            # Servicios de API
│   │   ├── components/     # Componentes reutilizables
│   │   ├── context/        # Context API (Auth)
│   │   ├── hooks/          # Custom hooks
│   │   ├── pages/          # Páginas de la aplicación
│   │   └── utils/          # Utilidades
│   ├── Dockerfile          # Imagen Docker del frontend
│   └── nginx.conf          # Configuración de Nginx
│
└── docker-compose.yml       # Docker Compose principal (backend + frontend)
```

## ⚙️ Configuración

### Variables de Entorno

El proyecto utiliza variables de entorno para la configuración. Puedes configurarlas de dos formas:

#### Opción 1: Variables de Entorno del Sistema

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

### Variables Requeridas

- `SQL_CONNECTION` - Cadena de conexión a SQL Server
- `BLOB_STORAGE_CONNECTION` - Cadena de conexión a Azure Blob Storage
- `JWT_KEY` - Clave secreta para firmar tokens JWT (mínimo 32 caracteres)
- `JWT_ISSUER` - Emisor del token
- `JWT_AUDIENCE` - Audiencia del token

## 🐳 Ejecución con Docker Compose

### Prerrequisitos

- Docker Desktop instalado y ejecutándose
- Docker Compose v3.8 o superior

### Ejecución

Desde la raíz del proyecto:

```bash
# Construir y ejecutar ambos servicios
docker-compose up --build

# Ejecutar en segundo plano
docker-compose up -d --build

# Ver logs
docker-compose logs -f

# Detener servicios
docker-compose down
```

### Acceso a la Aplicación

Una vez que los contenedores estén ejecutándose:

- **Frontend**: http://localhost:3000
- **Backend API**: http://localhost:5000/api/v1
- **Documentación API**: http://localhost:5000/scalar/v1

### Verificación del Estado

```bash
# Ver estado de los contenedores
docker-compose ps

# Ver logs de un servicio específico
docker-compose logs productsapi
docker-compose logs frontend
```

## 💻 Desarrollo Local

### Backend

```bash
cd ProductsAPI

# Restaurar dependencias
dotnet restore

# Ejecutar migraciones (si es necesario)
dotnet ef database update

# Ejecutar en modo desarrollo
dotnet run
```

El backend estará disponible en `http://localhost:5000`

### Frontend

```bash
cd frontend

# Instalar dependencias
npm install

# Ejecutar en modo desarrollo
npm run dev
```

El frontend estará disponible en `http://localhost:5173`

**Nota**: Asegúrate de que el backend esté ejecutándose antes de iniciar el frontend.

## 📚 Documentación de la API

### Base URL

```
http://localhost:5000/api/v1
```

### Autenticación

Todos los endpoints (excepto `/auth/login` y `/auth/register`) requieren autenticación mediante Bearer Token JWT.

**Formato del Header:**
```
Authorization: Bearer {token}
```

### Endpoints Principales

#### Autenticación
- `POST /auth/register` - Registrar nuevo usuario
- `POST /auth/login` - Iniciar sesión y obtener token

#### Productos
- `GET /products` - Obtener todos los productos
- `GET /products/{id}` - Obtener producto por ID
- `POST /products` - Crear nuevo producto
- `PUT /products/{id}` - Actualizar producto
- `DELETE /products/{id}` - Eliminar producto

#### Ventas
- `GET /sales` - Obtener todas las ventas
- `GET /sales/{id}` - Obtener venta por ID
- `POST /sales` - Crear nueva venta
- `DELETE /sales/{id}` - Eliminar venta

#### Reportes
- `GET /reports/sales` - Generar reporte de ventas (con parámetros opcionales `startDate` y `endDate`)

### Documentación Interactiva

En modo desarrollo, la documentación interactiva está disponible en:
```
http://localhost:5000/scalar/v1
```

## 🚀 Flujo de Uso

1. **Registro/Login**: Accede a la aplicación y crea una cuenta o inicia sesión
2. **Productos**: Gestiona tu catálogo de productos (crear, editar, eliminar)
3. **Ventas**: Crea ventas seleccionando productos y cantidades
4. **Reportes**: Visualiza métricas y análisis de ventas con gráficos interactivos

## 📝 Notas Adicionales

- Los tokens JWT expiran después de 30 minutos
- La eliminación de ventas es en cascada (elimina automáticamente los items asociados)
- Las ventas actualizan automáticamente el stock de los productos
- El frontend usa un proxy de Nginx para comunicarse con el backend en producción
- En desarrollo, el frontend se comunica directamente con el backend en `http://localhost:5000`

## 🔒 Seguridad

- Las contraseñas se almacenan con hash usando `PasswordHasher`
- Los tokens JWT se validan en cada solicitud autenticada
- Todas las rutas (excepto login y register) requieren autenticación
- Las validaciones de datos se realizan con Data Annotations
- CORS configurado para permitir comunicación entre frontend y backend

## 🐛 Solución de Problemas

### El contenedor no inicia
1. Verifica que Docker Desktop esté ejecutándose
2. Revisa los logs: `docker-compose logs`
3. Verifica que los puertos 3000 y 5000 no estén en uso

### Error de conexión a la base de datos
1. Verifica que la cadena de conexión SQL sea correcta
2. Asegúrate de que la base de datos sea accesible
3. Revisa los logs del backend

### El frontend no se comunica con el backend
1. Verifica que ambos contenedores estén ejecutándose
2. Revisa la configuración de CORS en el backend
3. Verifica los logs de ambos servicios

### Reconstruir las imágenes desde cero
```bash
docker-compose build --no-cache
docker-compose up
```

## 📄 Licencia

Este proyecto es privado, de prueba y está destinado para uso interno.

