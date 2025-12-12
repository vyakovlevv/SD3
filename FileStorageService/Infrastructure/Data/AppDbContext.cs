using FileStorageService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileStorageService.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Submission> Submissions => Set<Submission>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}