using FileStorageService.Domain.Entities;

namespace FileStorageService.Application.Interfaces;

public interface ISubmissionRepository
{
    Task AddAsync(Submission submission);
    Task<Submission?> GetByIdAsync(Guid id);
    Task<List<Submission>> GetAllAsync();
}