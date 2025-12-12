using FileAnalysisService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileAnalysisService.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<Report> Reports => Set<Report>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}