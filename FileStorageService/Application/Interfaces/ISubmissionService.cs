using FileStorageService.Application.DTO;

namespace FileStorageService.Application.Interfaces;

public interface ISubmissionService
{ 
    Task<SubmissionDto> UploadAsync(Guid student, Guid taskId, IFormFile file);
    Task<(byte[]?, string? path)> GetFileAsync(Guid id);
    Task<List<SubmissionDto>> GetAllAsync();
}