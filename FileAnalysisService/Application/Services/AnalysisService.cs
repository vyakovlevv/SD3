using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using FileAnalysisService.Application.DTO;
using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Infrastructure.Clients;
using FileAnalysisService.Domain.Entities;

namespace FileAnalysisService.Application.Services;

public class AnalysisService : IAnalysisService
{
    private readonly FileStorageClient _fileClient;
    private readonly IReportRepository _repo;

    public AnalysisService(FileStorageClient fileClient, IReportRepository repo)
    {
        _fileClient = fileClient;
        _repo = repo;
    }
    
    public async Task<ReportDto> AnalyzeSubmissionAsync(Guid submissionId)
    {
        var fileResult = await _fileClient.GetSubmissionFileAsync(submissionId);
        if (fileResult == null)
            throw new InvalidOperationException("Submission not found in FileStorage");

        var bytes = fileResult.Value.FileBytes;
        string content = Encoding.UTF8.GetString(bytes);

        var hash = ComputeSha256Hex(content);

        var sameHashReports = await _repo.GetByHashAsync(hash);

        var matchedSubmissionIds = sameHashReports.Select(r => r.SubmissionId).ToList();
        var allSubs = await _fileClient.GetAllSubmissionsAsync();
        var mapSubmissionToStudent = allSubs.ToDictionary(s => s.Id, s => s);

        var matchedStudentIds = matchedSubmissionIds
            .Where(id => mapSubmissionToStudent.ContainsKey(id))
            .Select(id => mapSubmissionToStudent[id].Student)
            .Distinct()
            .ToList();

        bool isPlagiarism = matchedSubmissionIds.Count > 0;

        var report = new Report
        {
            SubmissionId = submissionId,
            TaskId = mapSubmissionToStudent[submissionId].TaskId,
            ContentHash = hash,
            IsPlagiarism = isPlagiarism,
            MatchedSubmissionIdsJson = JsonSerializer.Serialize(matchedSubmissionIds),
            MatchedStudentIdsJson = JsonSerializer.Serialize(matchedStudentIds),
            CreatedAt = DateTime.UtcNow
        };

        await _repo.AddAsync(report);

        var dto = new ReportDto
        {
            Id = report.Id,
            SubmissionId = report.SubmissionId,
            TaskId = mapSubmissionToStudent[submissionId].TaskId,
            ContentHash = report.ContentHash,
            IsPlagiarism = report.IsPlagiarism,
            MatchedSubmissionIds = matchedSubmissionIds,
            MatchedStudentIds = matchedStudentIds,
            CreatedAt = report.CreatedAt
        };

        return dto;
    }

    private static string ComputeSha256Hex(string text)
    {
        using var sha = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(text);
        var hash = sha.ComputeHash(bytes);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }
}
