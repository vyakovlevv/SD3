using FileStorageService.Application.Interfaces;
using FileStorageService.Domain.Entities;
using FileStorageService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FileStorageService.Infrastructure.Repositories;

public class SubmissionRepository : ISubmissionRepository
{
    private readonly AppDbContext _db;

    public SubmissionRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Submission submission)
    {
        _db.Submissions.Add(submission);
        await _db.SaveChangesAsync();
    }

    public Task<Submission?> GetByIdAsync(Guid id)
    {
        return _db.Submissions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public Task<List<Submission>> GetAllAsync()
    {
        return _db.Submissions.ToListAsync();
    }
}