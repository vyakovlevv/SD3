using FileAnalysisService.Application;
using FileAnalysisService.Infrastructure;
using FileAnalysisService.Infrastructure.Clients;
using FileAnalysisService.Infrastructure.Data;
using FileAnalysisService.Presentation;
using FileAnalysisService.Presentation.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var fileStorageBase = builder.Configuration.GetValue<string>("FileStorage:BaseUrl") ?? "http://localhost:5194";
var conn = "Data Source=/db/reports.db";

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(conn));

builder.Services.AddHandlers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructure();

builder.Services.AddHttpClient<FileStorageClient>(client =>
{
    client.BaseAddress = new Uri(fileStorageBase.TrimEnd('/'));
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FileAnalysisService", Version = "v1" });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapFileAnalysisEndpoints();

app.Run();
