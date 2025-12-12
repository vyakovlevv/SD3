using System.Text.Json;
using FileAnalysisService.Application.Interfaces;
using FileAnalysisService.Domain.Entities;
using FileAnalysisService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Infrastructure.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _db;

    public ReportRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Report report)
    {
        _db.Reports.Add(report);
        await _db.SaveChangesAsync();
    }

    public Task<Report?> GetBySubmissionIdAsync(Guid submissionId)
    {
        return _db.Reports.FirstOrDefaultAsync(r => r.SubmissionId == submissionId);
    }

    public Task<List<Report>> GetAllAsync()
    {
        return _db.Reports.OrderByDescending(r => r.CreatedAt).ToListAsync();
    }

    public Task<List<Report>> GetByHashAsync(string hash)
    {
        return _db.Reports.Where(r => r.ContentHash == hash).ToListAsync();
    }
}