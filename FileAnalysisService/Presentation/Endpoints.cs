using FileAnalysisService.Presentation.Handlers.AddReport;
using FileAnalysisService.Presentation.Handlers.GetReportBySubmissionId;
using FileAnalysisService.Presentation.Handlers.GetReportsByTaskId;

namespace FileAnalysisService.Presentation;

public static class FileAnalysisEndpoints
{
    public static WebApplication MapFileAnalysisEndpoints(this WebApplication app)
    {
        app.MapGroup("/reports")
            .WithTags("Reports")
            .MapGetReportBySubmissionId()
            .MapAddReport()
            .MapGetReportsByTaskId();
        return app;
    }
    private static RouteGroupBuilder MapGetReportBySubmissionId(this RouteGroupBuilder group)
    {
        group.MapGet("/submission/{submissionId:guid}", async (Guid submissionId, IGetReportBySubmissionIdHandler handler) =>
        {
            var response = await handler.Handle(submissionId);
            if (response == null) return Results.NotFound();
            return Results.Ok(response);
        })
        .WithName("GetReportBySubmissionId")
        .WithSummary("Get report by submission id")
        .WithDescription("Get report by submission id")
        .WithOpenApi()
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);
        return group;
    }
    private static RouteGroupBuilder MapAddReport(this RouteGroupBuilder group)
    {
        group.MapPost("/upload", async (AddReportRequest request, IAddReportRequestHandler handler) =>
        {
            try
            {
                var response = await handler.Handle(request);
                return Results.Ok(response);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.NotFound(new { message = "Submission not found in FileStorage" });
            }
            
        })
        .DisableAntiforgery()
        .WithName("AddReport")
        .WithSummary("Add a new report")
        .WithDescription("Add a new report")
        .WithOpenApi();
        return group;
    }
    private static RouteGroupBuilder MapGetReportsByTaskId(this RouteGroupBuilder group)
    {
        group.MapGet("/task/{taskId:guid}", async (Guid taskId, IGetReportsByTaskIdHandler handler) =>
        {
            var response = await handler.Handle(taskId);
            return Results.Ok(response);
        })
        .WithName("GetReportsByTaskId")
        .WithSummary("Get reports by task id")
        .WithDescription("Get reports by task id")
        .WithOpenApi();
        return group;
    }
}