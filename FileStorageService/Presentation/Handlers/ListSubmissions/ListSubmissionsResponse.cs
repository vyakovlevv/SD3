using FileStorageService.Application.DTO;

namespace FileStorageService.Presentation.Handlers.ListSubmissions;

public record ListSubmissionsResponse(IReadOnlyList<SubmissionDto> Submissions);