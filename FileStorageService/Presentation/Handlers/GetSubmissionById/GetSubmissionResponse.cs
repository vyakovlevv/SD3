using FileStorageService.Application.DTO;

namespace FileStorageService.Presentation.Handlers.GetSubmissionById;

public record GetSubmissionResponse(byte[]? SubmissionData, string Filename);