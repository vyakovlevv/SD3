using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Application.Services;

namespace FileAnalysisService.Application;


public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAnalysisService, AnalysisService>();
        return services;
    }
}