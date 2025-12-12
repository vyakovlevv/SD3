using FileStorageService.Application.Interfaces;
using FileStorageService.Infrastructure.Clients;

namespace FileStorageService.Presentation.Handlers.AddSubmission;

public class AddSubmissionRequestHandler(ISubmissionService service, AnalysisClient analysisClient) : IAddSubmissionRequestHandler
{
    public async Task<AddSubmissionResponse> Handle(AddSubmissionRequest req)
    {
        AddSubmissionResponse resp = new(await service.UploadAsync(req.Student, req.TaskId, req.File));
        await analysisClient.SendForAnalysisAsync(resp.Submission.Id);
        return resp;
    }
}