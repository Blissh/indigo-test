using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Logging;

namespace ProductsAPI.Infrastructure.Services
{
    public class BlobStorageService : IBlobStorageService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<BlobStorageService> _logger;

        public BlobStorageService(IConfiguration configuration, ILogger<BlobStorageService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        private BlobServiceClient GetBlobServiceClient()
        {
            var connectionString = _configuration.GetConnectionString("BlobStorageConnection");
            if (string.IsNullOrEmpty(connectionString))
            {
                _logger.LogError("La cadena de conexión de Blob Storage no está configurada");
                throw new InvalidOperationException("La cadena de conexión de Blob Storage no está configurada");
            }

            return new BlobServiceClient(connectionString);
        }

        private async Task<BlobContainerClient> GetContainerClientAsync(string containerName, bool createIfNotExists = false)
        {
            try
            {
                var blobServiceClient = GetBlobServiceClient();
                var containerClient = blobServiceClient.GetBlobContainerClient(containerName);

                if (createIfNotExists)
                {
                    await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                    _logger.LogInformation("Contenedor '{ContainerName}' verificado/creado", containerName);
                }

                return containerClient;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener el cliente del contenedor: {ContainerName}", containerName);
                throw new ApplicationException($"Error al acceder al contenedor '{containerName}'", ex);
            }
        }

        public async Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream, string? contentType = null)
        {
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentException("El nombre del contenedor no puede estar vacío", nameof(containerName));
            
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("El nombre del archivo no puede estar vacío", nameof(fileName));
            
            if (fileStream == null)
                throw new ArgumentNullException(nameof(fileStream));

            try
            {
                _logger.LogInformation("Iniciando subida de archivo: {FileName} al contenedor: {ContainerName}", fileName, containerName);

                var containerClient = await GetContainerClientAsync(containerName, createIfNotExists: true);
                var blobClient = containerClient.GetBlobClient(fileName);

                var uploadOptions = new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = contentType ?? "application/octet-stream"
                    }
                };

                await blobClient.UploadAsync(fileStream, uploadOptions);
                
                var url = blobClient.Uri.ToString();
                _logger.LogInformation("Archivo subido exitosamente: {FileName}, URL: {Url}", fileName, url);
                
                return url;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir el archivo {FileName} al contenedor {ContainerName}", fileName, containerName);
                throw new ApplicationException($"Error al subir el archivo '{fileName}' al blob storage", ex);
            }
            finally
            {
                if (fileStream.CanSeek)
                {
                    fileStream.Position = 0;
                }
            }
        }

        public async Task<bool> DeleteFileAsync(string containerName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentException("El nombre del contenedor no puede estar vacío", nameof(containerName));
            
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("El nombre del archivo no puede estar vacío", nameof(fileName));

            try
            {
                _logger.LogInformation("Intentando eliminar archivo: {FileName} del contenedor: {ContainerName}", fileName, containerName);

                var containerClient = await GetContainerClientAsync(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                var result = await blobClient.DeleteIfExistsAsync();
                
                if (result.Value)
                {
                    _logger.LogInformation("Archivo eliminado exitosamente: {FileName}", fileName);
                }
                else
                {
                    _logger.LogWarning("El archivo no existe: {FileName}", fileName);
                }

                return result.Value;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar el archivo {FileName} del contenedor {ContainerName}", fileName, containerName);
                throw new ApplicationException($"Error al eliminar el archivo '{fileName}' del blob storage", ex);
            }
        }

        public async Task<bool> FileExistsAsync(string containerName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentException("El nombre del contenedor no puede estar vacío", nameof(containerName));
            
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("El nombre del archivo no puede estar vacío", nameof(fileName));

            try
            {
                var containerClient = await GetContainerClientAsync(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);
                
                return await blobClient.ExistsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar existencia del archivo {FileName} en el contenedor {ContainerName}", fileName, containerName);
                throw new ApplicationException($"Error al verificar la existencia del archivo '{fileName}'", ex);
            }
        }

        public async Task<string> GetFileUrlAsync(string containerName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentException("El nombre del contenedor no puede estar vacío", nameof(containerName));
            
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("El nombre del archivo no puede estar vacío", nameof(fileName));

            try
            {
                var containerClient = await GetContainerClientAsync(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                var exists = await blobClient.ExistsAsync();
                if (!exists)
                {
                    _logger.LogWarning("El archivo no existe: {FileName}", fileName);
                    throw new FileNotFoundException($"El archivo '{fileName}' no existe en el contenedor '{containerName}'");
                }

                return blobClient.Uri.ToString();
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la URL del archivo {FileName} del contenedor {ContainerName}", fileName, containerName);
                throw new ApplicationException($"Error al obtener la URL del archivo '{fileName}'", ex);
            }
        }

        public async Task<Stream> DownloadFileAsync(string containerName, string fileName)
        {
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentException("El nombre del contenedor no puede estar vacío", nameof(containerName));
            
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("El nombre del archivo no puede estar vacío", nameof(fileName));

            try
            {
                _logger.LogInformation("Descargando archivo: {FileName} del contenedor: {ContainerName}", fileName, containerName);

                var containerClient = await GetContainerClientAsync(containerName);
                var blobClient = containerClient.GetBlobClient(fileName);

                var exists = await blobClient.ExistsAsync();
                if (!exists)
                {
                    _logger.LogWarning("El archivo no existe: {FileName}", fileName);
                    throw new FileNotFoundException($"El archivo '{fileName}' no existe en el contenedor '{containerName}'");
                }

                var memoryStream = new MemoryStream();
                await blobClient.DownloadToAsync(memoryStream);
                memoryStream.Position = 0;

                _logger.LogInformation("Archivo descargado exitosamente: {FileName}", fileName);
                return memoryStream;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al descargar el archivo {FileName} del contenedor {ContainerName}", fileName, containerName);
                throw new ApplicationException($"Error al descargar el archivo '{fileName}'", ex);
            }
        }

        public async Task<List<string>> ListFilesAsync(string containerName, string? prefix = null)
        {
            if (string.IsNullOrWhiteSpace(containerName))
                throw new ArgumentException("El nombre del contenedor no puede estar vacío", nameof(containerName));

            try
            {
                _logger.LogInformation("Listando archivos del contenedor: {ContainerName} con prefijo: {Prefix}", containerName, prefix ?? "ninguno");

                var containerClient = await GetContainerClientAsync(containerName);
                var files = new List<string>();

                await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: prefix))
                {
                    files.Add(blobItem.Name);
                }

                _logger.LogInformation("Se encontraron {Count} archivos en el contenedor {ContainerName}", files.Count, containerName);
                return files;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar archivos del contenedor {ContainerName}", containerName);
                throw new ApplicationException($"Error al listar archivos del contenedor '{containerName}'", ex);
            }
        }
    }
}
