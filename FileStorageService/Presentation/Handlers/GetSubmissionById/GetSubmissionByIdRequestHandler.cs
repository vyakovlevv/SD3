using FileStorageService.Application.Interfaces;

namespace FileStorageService.Presentation.Handlers.GetSubmissionById;

public class GetSubmissionByIdRequestHandler(ISubmissionService service) : IGetSubmissionByIdRequestHandler
{
    public async Task<GetSubmissionResponse> Handle(Guid id)
    {
        var (bytes, filename) = await service.GetFileAsync(id);
        return new (bytes, filename);
    }
}