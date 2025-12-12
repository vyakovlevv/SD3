using FileStorageService.Application.Interfaces;

namespace FileStorageService.Infrastructure.Storage;

public class FileStorage : IFileRepository
{
    private readonly string _root;

    public FileStorage()
    {
        _root = "/storage";
        Directory.CreateDirectory(_root);
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var filePath = Path.Combine(_root, $"{Guid.NewGuid()}_{file.FileName}");

        using var stream = File.OpenWrite(filePath);
        await file.CopyToAsync(stream);

        return filePath;
    }

    public Task<byte[]> GetFileBytesAsync(string path)
    {
        return File.ReadAllBytesAsync(path);
    }
}