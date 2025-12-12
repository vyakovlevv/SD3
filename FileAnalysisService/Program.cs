using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Application.Services;
using FileAnalysisService.Infrastructure.Clients;
using FileAnalysisService.Infrastructure.Data;
using FileAnalysisService.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Configuration
var fileStorageBase = builder.Configuration.GetValue<string>("FileStorage:BaseUrl") ?? "http://localhost:5194";
var conn = "Data Source=/db/reports.db";

// Db
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite(conn));

// DI
builder.Services.AddScoped<IReportRepository, ReportRepository>();
builder.Services.AddScoped<AnalysisService>();

// Http client to FileStorage
builder.Services.AddHttpClient<IFileStorageClient, FileStorageClient>(client =>
{
    client.BaseAddress = new Uri(fileStorageBase.TrimEnd('/'));
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "FileAnalysisService", Version = "v1" });
});

var app = builder.Build();

// Ensure DB folder and DB created
var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "DB");
Directory.CreateDirectory(dbPath);
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

// Endpoints

// 1. POST /analyze/{submissionId} - запуск анализа для submissionId
app.MapPost("/analyze/{submissionId:guid}", async (Guid submissionId, AnalysisService svc) =>
{
    try
    {
        var dto = await svc.AnalyzeSubmissionAsync(submissionId);
        return Results.Ok(dto);
    }
    catch (InvalidOperationException)
    {
        return Results.NotFound(new { message = "Submission not found in FileStorage" });
    }
})
.WithName("AnalyzeSubmission")
.WithOpenApi()
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// 2. GET /reports/{submissionId} - получить отчёт для submissionId
app.MapGet("/reports/{submissionId:guid}", async (Guid submissionId, IReportRepository repo) =>
{
    var r = await repo.GetBySubmissionIdAsync(submissionId);
    if (r == null) return Results.NotFound();
    var dto = new FileAnalysisService.Application.DTO.ReportDto
    {
        Id = r.Id,
        SubmissionId = r.SubmissionId,
        ContentHash = r.ContentHash,
        IsPlagiarism = r.IsPlagiarism,
        MatchedSubmissionIds = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(r.MatchedSubmissionIdsJson) ?? new(),
        MatchedStudentIds = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(r.MatchedStudentIdsJson) ?? new(),
        CreatedAt = r.CreatedAt
    };
    return Results.Ok(dto);
})
.WithName("GetReportBySubmission")
.WithOpenApi()
.Produces(StatusCodes.Status200OK)
.Produces(StatusCodes.Status404NotFound);

// 3. GET /reports - список всех отчётов
app.MapGet("/reports", async (IReportRepository repo) =>
{
    var list = await repo.GetAllAsync();
    var dtoList = list.Select(r => new FileAnalysisService.Application.DTO.ReportDto
    {
        Id = r.Id,
        SubmissionId = r.SubmissionId,
        ContentHash = r.ContentHash,
        IsPlagiarism = r.IsPlagiarism,
        MatchedSubmissionIds = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(r.MatchedSubmissionIdsJson) ?? new(),
        MatchedStudentIds = System.Text.Json.JsonSerializer.Deserialize<List<Guid>>(r.MatchedStudentIdsJson) ?? new(),
        CreatedAt = r.CreatedAt
    }).ToList();
    return Results.Ok(dtoList);
})
.WithName("ListReports")
.WithOpenApi();

// Run
app.Run();
