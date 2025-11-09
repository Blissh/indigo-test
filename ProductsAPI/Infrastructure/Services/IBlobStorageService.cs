namespace ProductsAPI.Infrastructure.Services
{
    /// <summary>
    /// Servicio para operaciones con Azure Blob Storage.
    /// </summary>
    public interface IBlobStorageService
    {
        /// <summary>
        /// Sube un archivo al contenedor especificado.
        /// </summary>
        Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream, string? contentType = null);
        
        /// <summary>
        /// Elimina un archivo del contenedor.
        /// </summary>
        Task<bool> DeleteFileAsync(string containerName, string fileName);
        
        /// <summary>
        /// Verifica si un archivo existe en el contenedor.
        /// </summary>
        Task<bool> FileExistsAsync(string containerName, string fileName);
        
        /// <summary>
        /// Obtiene la URL pública de un archivo.
        /// </summary>
        Task<string> GetFileUrlAsync(string containerName, string fileName);
        
        /// <summary>
        /// Descarga un archivo del contenedor.
        /// </summary>
        Task<Stream> DownloadFileAsync(string containerName, string fileName);
        
        /// <summary>
        /// Lista todos los archivos del contenedor con el prefijo opcional.
        /// </summary>
        Task<List<string>> ListFilesAsync(string containerName, string? prefix = null);
    }
}

