using FileStorageService.Application.Interfaces;

namespace FileStorageService.Presentation.Handlers.ListSubmissions;

public class ListSubmissionsRequestHandler(ISubmissionService service) : IListSubmissionsRequestHandler
{
    public async Task<ListSubmissionsResponse> Handle()
    {
        return new ListSubmissionsResponse(await service.GetAllAsync());
    }
}