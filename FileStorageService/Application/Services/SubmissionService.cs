namespace FileStorageService.Application.Services;

using Application.DTO;
using Application.Interfaces;
using Domain.Entities;

public class SubmissionService : ISubmissionService
{
    private readonly IFileRepository _fileRepo;
    private readonly ISubmissionRepository _submissionRepo;

    public SubmissionService(
        IFileRepository fileRepo,
        ISubmissionRepository submissionRepo)
    {
        _fileRepo = fileRepo;
        _submissionRepo = submissionRepo;
    }

    public async Task<SubmissionDto> UploadAsync(Guid student, Guid taskId, IFormFile file)
    {
        var savedPath = await _fileRepo.SaveFileAsync(file);

        var submission = new Submission
        {
            Student = student,
            TaskId = taskId,
            FileName = file.FileName,
            StoragePath = savedPath,
        };

        await _submissionRepo.AddAsync(submission);

        return new SubmissionDto
        {
            Id = submission.Id,
            Student = submission.Student,
            TaskId = submission.TaskId,
            FileName = submission.FileName,
            StoragePath = submission.StoragePath,
            UploadedAt = submission.UploadedAt
        };
    }

    public async Task<(byte[]?, string?)> GetFileAsync(Guid id)
    {
        var sub = await _submissionRepo.GetByIdAsync(id);
        if (sub == null) return (null, null);
        return (await _fileRepo.GetFileBytesAsync(sub.StoragePath), sub.FileName);
    }

    public async Task<List<SubmissionDto>> GetAllAsync()
    {
        var subs = await _submissionRepo.GetAllAsync();
        return subs.Select(x => new SubmissionDto
        {
            Id = x.Id,
            Student = x.Student,
            TaskId = x.TaskId,
            FileName = x.FileName,
            StoragePath = x.StoragePath,
            UploadedAt = x.UploadedAt
        }).ToList();
    }
}
