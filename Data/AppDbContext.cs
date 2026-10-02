using Microsoft.EntityFrameworkCore;
using DotNetApp.Data.Models;

namespace DotNetApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<ProjectItem> Projects => Set<ProjectItem>();
    public DbSet<ActivityLog> ActivityLogs => Set<ActivityLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ProjectItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Assignee).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Category).HasConversion<string>();
            entity.Property(e => e.Priority).HasConversion<string>();
            entity.Property(e => e.Status).HasConversion<string>();
        });

        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Action).IsRequired().HasMaxLength(50);
            entity.Property(e => e.EntityTitle).IsRequired().HasMaxLength(150);
        });
    }
}
