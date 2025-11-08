namespace ProductsAPI.Infrastructure.Services
{
    public interface IBlobStorageService
    {
        Task<string> UploadFileAsync(string containerName, string fileName, Stream fileStream, string? contentType = null);
        Task<bool> DeleteFileAsync(string containerName, string fileName);
        Task<bool> FileExistsAsync(string containerName, string fileName);
        Task<string> GetFileUrlAsync(string containerName, string fileName);
        Task<Stream> DownloadFileAsync(string containerName, string fileName);
        Task<List<string>> ListFilesAsync(string containerName, string? prefix = null);
    }
}

