using ApiGateway.DTO;
using ApiGateway.Services;
using ApiGateway.Gateways;

var builder = WebApplication.CreateBuilder(args);

var fileStorageUrl = builder.Configuration["Services:FileStorage"]!;
var fileAnalysisUrl = builder.Configuration["Services:FileAnalysis"]!;

// HttpClient для FileStorage
builder.Services.AddHttpClient<ProxyService>("FileStorage", client =>
{
    client.BaseAddress = new Uri(fileStorageUrl);
});

// HttpClient для FileAnalysis
builder.Services.AddHttpClient<ProxyService>("FileAnalysis", client =>
{
    client.BaseAddress = new Uri(fileAnalysisUrl);
});

builder.Services.AddScoped<FileStorageGateway>(sp =>
{
    var proxy = sp.GetRequiredService<IHttpClientFactory>().CreateClient("FileStorage");
    return new FileStorageGateway(new ProxyService(proxy));
});

builder.Services.AddScoped<FileAnalysisGateway>(sp =>
{
    var proxy = sp.GetRequiredService<IHttpClientFactory>().CreateClient("FileAnalysis");
    return new FileAnalysisGateway(new ProxyService(proxy));
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("gateway", new() { Title = "API Gateway", Version = "v1" });

    // 🔥 подключаем другие OpenAPI документы
    c.SwaggerDoc("filestorage", new() { Title = "FileStorageService", Version = "v1" });
    c.SwaggerDoc("fileanalysis", new() { Title = "FileAnalysisService", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/gateway/swagger.json", "API Gateway");

    // 🔥 внешние свэггеры (объединение)
    options.SwaggerEndpoint($"{fileStorageUrl}/swagger/v1/swagger.json", "FileStorage API");
    options.SwaggerEndpoint($"{fileAnalysisUrl}/swagger/v1/swagger.json", "FileAnalysis API");
});




app.MapPost("/submissions", async (HttpRequest req, FileStorageGateway gw)
        => await gw.Upload(req))
    .DisableAntiforgery()
    .Accepts<AddSubmissionRequestDto>("multipart/form-data") 
    .Produces(StatusCodes.Status200OK)
    .WithOpenApi(op =>
    {
        op.Summary = "Upload submission (file + metadata)";
        op.Description = "Uploads a file and metadata. Parameters: studentId, taskId, file.";
        return op;
    });

app.MapGet("/submissions", async (FileStorageGateway gw)
        => await gw.GetAllSubmissions());

app.MapGet("/submissions/{id:guid}", async (Guid id, FileStorageGateway gw)
        => await gw.GetSubmission(id));

app.MapPost("/analyze/{submissionId:guid}",
    async (Guid submissionId, FileAnalysisGateway gw)
        => await gw.Analyze(submissionId));

app.MapGet("/reports/{submissionId:guid}",
    async (Guid submissionId, FileAnalysisGateway gw)
        => await gw.GetReport(submissionId));

app.MapGet("/reports", async (FileAnalysisGateway gw)
        => await gw.GetReports());


app.Run();
