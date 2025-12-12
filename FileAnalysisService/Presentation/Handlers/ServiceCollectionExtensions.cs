using FileAnalysisService.Presentation.Handlers.AddReport;
using FileAnalysisService.Presentation.Handlers.GetReportBySubmissionId;
using FileAnalysisService.Presentation.Handlers.GetReportsByTaskId;

namespace FileAnalysisService.Presentation.Handlers;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<IAddReportRequestHandler, AddReportRequestHandler>();
        services.AddScoped<IGetReportBySubmissionIdHandler, GetReportBySubmissionIdHandler>();
        services.AddScoped<IGetReportsByTaskIdHandler, GetReportsByTaskIdHandler>();
        return services;
    }
}