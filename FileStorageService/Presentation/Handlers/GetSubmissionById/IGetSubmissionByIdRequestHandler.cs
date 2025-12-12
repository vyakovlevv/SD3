namespace FileStorageService.Presentation.Handlers.GetSubmissionById;

public interface IGetSubmissionByIdRequestHandler
{
    Task<GetSubmissionResponse> Handle(Guid id);
}