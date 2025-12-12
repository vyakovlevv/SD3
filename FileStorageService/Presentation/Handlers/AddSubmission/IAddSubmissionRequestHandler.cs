using FileStorageService.Application.DTO;

namespace FileStorageService.Presentation.Handlers.AddSubmission;

public interface IAddSubmissionRequestHandler
{
    Task<AddSubmissionResponse> Handle(AddSubmissionRequest request);
}