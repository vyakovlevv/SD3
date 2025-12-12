using FileStorageService.Application.DTO;
using FileStorageService.Presentation.Handlers.AddSubmission;
using FileStorageService.Presentation.Handlers.GetSubmissionById;
using FileStorageService.Presentation.Handlers.ListSubmissions;
using Microsoft.AspNetCore.Mvc;

namespace FileStorageService.Presentation;

public static class  FileStorageEndpoints
{
    public static WebApplication MapFileStorageEndpoints(this WebApplication app)
    {
        app.MapGroup("/submissions")
            .WithTags("Submissions")
            .MapGetSubmissions()
            .MapAddSubmission()
            .MapGetSubmissionById();
        return app;
    }
    private static RouteGroupBuilder MapGetSubmissions(this RouteGroupBuilder group)
    {
        group.MapGet("", async (IListSubmissionsRequestHandler handler) =>
        {
            var response = await handler.Handle();
            return Results.Ok(response);
        })
        .WithName("GetSubmissions")
        .WithSummary("Get all submissions")
        .WithDescription("Get all submissions from the database")
        .WithOpenApi();
        return group;
    }
    private static RouteGroupBuilder MapAddSubmission(this RouteGroupBuilder group)
    {
        group.MapPost("", async ([FromForm] AddSubmissionRequest request, IAddSubmissionRequestHandler handler) =>
        {
            var response = await handler.Handle(request);
            return Results.Ok(response);
        })
        .DisableAntiforgery()
        .Accepts<AddSubmissionRequest>("multipart/form-data")
        .WithName("AddSubmission")
        .WithSummary("Add a new submission")
        .WithDescription("Add a new car to the database")
        .WithOpenApi();
        return group;
    }
    private static RouteGroupBuilder MapGetSubmissionById(this RouteGroupBuilder group)
    {
        group.MapGet("/{id:guid}", async (Guid id, IGetSubmissionByIdRequestHandler handler) =>
        {
            var response = await handler.Handle(id);
            if (response.SubmissionData == null) return Results.NotFound();
            return Results.File(response.SubmissionData, "text/plain", response.Filename);
        })
        .WithName("GetSubmissionById")
        .WithSummary("Get submission by id")
        .WithDescription("Get submission by id")
        .WithOpenApi();
        return group;
    }
}