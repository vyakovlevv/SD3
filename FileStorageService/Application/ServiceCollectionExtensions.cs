using FileStorageService.Application.Interfaces;
using FileStorageService.Application.Services;
using FileStorageService.Infrastructure.Clients;
using FileStorageService.Infrastructure.Repositories;
using FileStorageService.Infrastructure.Storage;

namespace FileStorageService.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IFileRepository, FileStorage>();
        services.AddScoped<ISubmissionService, SubmissionService>();
        services.AddScoped<ISubmissionRepository, SubmissionRepository>();
        services.AddScoped<IAnalysisClient, AnalysisClient>();
        return services;
    }
}