using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Infrastructure.Clients;
using FileAnalysisService.Infrastructure.Repositories;

namespace FileAnalysisService.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IReportRepository, ReportRepository>();
        return services;
    }
}