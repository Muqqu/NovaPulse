using DotNetApp.Data;
using DotNetApp.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace DotNetApp.Services;

public class ProjectService : IProjectService
{
    private readonly IDbContextFactory<AppDbContext> _contextFactory;

    public ProjectService(IDbContextFactory<AppDbContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<List<ProjectItem>> GetAllProjectsAsync(
        string? search = null,
        ProjectCategory? category = null,
        ProjectPriority? priority = null,
        ProjectStatus? status = null,
        string? sortBy = null)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var query = context.Projects.AsQueryable();

        if (!string.IsNullOrWhiteSpace(search))
        {
            var s = search.Trim().ToLower();
            query = query.Where(p =>
                p.Title.ToLower().Contains(s) ||
                p.Description.ToLower().Contains(s) ||
                p.Assignee.ToLower().Contains(s) ||
                (p.Tags != null && p.Tags.ToLower().Contains(s)));
        }

        if (category.HasValue)
        {
            query = query.Where(p => p.Category == category.Value);
        }

        if (priority.HasValue)
        {
            query = query.Where(p => p.Priority == priority.Value);
        }

        if (status.HasValue)
        {
            query = query.Where(p => p.Status == status.Value);
        }

        query = sortBy switch
        {
            "duedate_asc" => query.OrderBy(p => p.DueDate),
            "duedate_desc" => query.OrderByDescending(p => p.DueDate),
            "priority_desc" => query.OrderByDescending(p => p.Priority),
            "progress_desc" => query.OrderByDescending(p => p.ProgressPercentage),
            "progress_asc" => query.OrderBy(p => p.ProgressPercentage),
            "title_asc" => query.OrderBy(p => p.Title),
            "newest" => query.OrderByDescending(p => p.CreatedAt),
            _ => query.OrderByDescending(p => p.IsStarred).ThenByDescending(p => p.CreatedAt)
        };

        return await query.ToListAsync();
    }

    public async Task<ProjectItem?> GetProjectByIdAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.Projects.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<ProjectItem> CreateProjectAsync(ProjectItem item)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        item.CreatedAt = DateTime.UtcNow;
        if (item.Status == ProjectStatus.Completed && item.ProgressPercentage < 100)
        {
            item.ProgressPercentage = 100;
        }

        context.Projects.Add(item);

        context.ActivityLogs.Add(new ActivityLog
        {
            Action = "Created",
            EntityTitle = item.Title,
            Details = $"Added project under {item.Category} with {item.Priority} priority",
            IconType = "add",
            Timestamp = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
        return item;
    }

    public async Task<ProjectItem> UpdateProjectAsync(ProjectItem item)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        item.UpdatedAt = DateTime.UtcNow;
        if (item.Status == ProjectStatus.Completed && item.ProgressPercentage < 100)
        {
            item.ProgressPercentage = 100;
        }

        context.Projects.Update(item);

        context.ActivityLogs.Add(new ActivityLog
        {
            Action = "Updated",
            EntityTitle = item.Title,
            Details = $"Updated project details & metrics",
            IconType = "edit",
            Timestamp = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> DeleteProjectAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var item = await context.Projects.FindAsync(id);
        if (item == null) return false;

        context.Projects.Remove(item);

        context.ActivityLogs.Add(new ActivityLog
        {
            Action = "Deleted",
            EntityTitle = item.Title,
            Details = $"Deleted project (#{id})",
            IconType = "delete",
            Timestamp = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ToggleStarAsync(int id)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var item = await context.Projects.FindAsync(id);
        if (item == null) return false;

        item.IsStarred = !item.IsStarred;
        item.UpdatedAt = DateTime.UtcNow;

        context.ActivityLogs.Add(new ActivityLog
        {
            Action = item.IsStarred ? "Starred" : "Unstarred",
            EntityTitle = item.Title,
            Details = item.IsStarred ? "Marked as favorite" : "Removed from favorites",
            IconType = "star",
            Timestamp = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateStatusAsync(int id, ProjectStatus status)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var item = await context.Projects.FindAsync(id);
        if (item == null) return false;

        var prevStatus = item.Status;
        item.Status = status;
        if (status == ProjectStatus.Completed)
        {
            item.ProgressPercentage = 100;
        }
        item.UpdatedAt = DateTime.UtcNow;

        context.ActivityLogs.Add(new ActivityLog
        {
            Action = "Status Changed",
            EntityTitle = item.Title,
            Details = $"Transitioned from {prevStatus} to {status}",
            IconType = "check",
            Timestamp = DateTime.UtcNow
        });

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateProgressAsync(int id, int progress)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var item = await context.Projects.FindAsync(id);
        if (item == null) return false;

        item.ProgressPercentage = Math.Clamp(progress, 0, 100);
        if (item.ProgressPercentage == 100 && item.Status != ProjectStatus.Completed)
        {
            item.Status = ProjectStatus.Completed;
        }
        item.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
        return true;
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        var projects = await context.Projects.ToListAsync();

        if (projects.Count == 0)
        {
            return new DashboardStats();
        }

        return new DashboardStats
        {
            TotalProjects = projects.Count,
            InProgressCount = projects.Count(p => p.Status == ProjectStatus.InProgress),
            CompletedCount = projects.Count(p => p.Status == ProjectStatus.Completed),
            BacklogCount = projects.Count(p => p.Status == ProjectStatus.Backlog),
            InReviewCount = projects.Count(p => p.Status == ProjectStatus.InReview),
            CriticalPriorityCount = projects.Count(p => p.Priority == ProjectPriority.Critical),
            HighPriorityCount = projects.Count(p => p.Priority == ProjectPriority.High),
            OverallProgressPercentage = (int)Math.Round(projects.Average(p => p.ProgressPercentage)),
            TotalEstimatedHours = projects.Sum(p => p.EstimatedHours),
            TotalLoggedHours = projects.Sum(p => p.LoggedHours),
            StarredCount = projects.Count(p => p.IsStarred)
        };
    }

    public async Task<List<ActivityLog>> GetRecentActivitiesAsync(int count = 10)
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        return await context.ActivityLogs
            .OrderByDescending(a => a.Timestamp)
            .Take(count)
            .ToListAsync();
    }

    public async Task ResetDatabaseAsync()
    {
        await using var context = await _contextFactory.CreateDbContextAsync();
        await context.Database.EnsureDeletedAsync();
        await DbInitializer.InitializeAsync(context);
    }
}
