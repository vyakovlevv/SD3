namespace FileAnalysisService.Application.DTO;

public record FileStorageSubmissionDto(Guid Id, Guid Student, Guid TaskId, string FileName, string StoragePath, DateTime UploadedAt);
