using FileStorageService.Presentation.Handlers.AddSubmission;
using FileStorageService.Presentation.Handlers.GetSubmissionById;
using FileStorageService.Presentation.Handlers.ListSubmissions;

namespace FileStorageService.Presentation.Handlers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<IAddSubmissionRequestHandler, AddSubmissionRequestHandler>();
        services.AddScoped<IGetSubmissionByIdRequestHandler, GetSubmissionByIdRequestHandler>();
        services.AddScoped<IListSubmissionsRequestHandler, ListSubmissionsRequestHandler>();
        return services;
    }
}