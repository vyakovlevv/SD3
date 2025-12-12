namespace FileStorageService.Presentation.Handlers.ListSubmissions;

public interface IListSubmissionsRequestHandler
{
    Task<ListSubmissionsResponse> Handle();
}