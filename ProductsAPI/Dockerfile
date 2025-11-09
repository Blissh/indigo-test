# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiar archivos del proyecto
COPY ["ProductsAPI.csproj", "./"]
RUN dotnet restore "ProductsAPI.csproj"

COPY . .
WORKDIR "/src"
RUN dotnet build "ProductsAPI.csproj" -c Release -o /app/build

# Stage 2: Publish
FROM build AS publish
RUN dotnet publish "ProductsAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 3: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

# Instalar curl para healthcheck
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Crear usuario no-root para seguridad
RUN groupadd -r appuser && useradd -r -g appuser appuser

# Copiar archivos publicados
COPY --from=publish /app/publish .

# Cambiar ownership al usuario no-root
RUN chown -R appuser:appuser /app
USER appuser

# Exponer puerto (Render usa PORT variable, por defecto 10000)
# Render detectará automáticamente el puerto si se vincula correctamente
EXPOSE 10000

# Variables de entorno por defecto para desarrollo local
# En Render, PORT se proporciona automáticamente y ASPNETCORE_URLS se configura en render.yaml
ENV ASPNETCORE_ENVIRONMENT=Development

# Healthcheck - Render detectará el puerto automáticamente
# Usamos un puerto por defecto para desarrollo local
# Usar /api/v1/auth/health que es un GET endpoint
HEALTHCHECK --interval=30s --timeout=10s --start-period=40s --retries=3 \
  CMD curl -f http://localhost:${PORT:-10000}/api/v1/auth/health || curl -f http://localhost:5000/api/v1/auth/health || exit 1

# Punto de entrada
ENTRYPOINT ["dotnet", "ProductsAPI.dll"]
