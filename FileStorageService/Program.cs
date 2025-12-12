using FileStorageService.Application;
using FileStorageService.Infrastructure.Clients;
using FileStorageService.Infrastructure.Data;
using FileStorageService.Presentation;
using FileStorageService.Presentation.Handlers;
using Microsoft.EntityFrameworkCore;


public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddDbContext<AppDbContext>(opt =>
            opt.UseSqlite("Data Source=/db/files.db"));

        var analysisBaseUrl = builder.Configuration.GetValue<string>("AnalysisService:BaseUrl")
                              ?? "http://localhost:5140";

        builder.Services.AddHttpClient<AnalysisClient>(client =>
        {
            client.BaseAddress = new Uri(analysisBaseUrl);
        });


        builder.Services.AddApplication();
        builder.Services.AddHandlers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.MapFileStorageEndpoints();

        app.Run();
    }
}
