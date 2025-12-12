namespace FileStorageService.Application.Interfaces;


public interface IFileRepository
{
    Task<string> SaveFileAsync(IFormFile file);
    Task<byte[]> GetFileBytesAsync(string path);
}