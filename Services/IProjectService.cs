using DotNetApp.Data.Models;

namespace DotNetApp.Services;

public class DashboardStats
{
    public int TotalProjects { get; set; }
    public int InProgressCount { get; set; }
    public int CompletedCount { get; set; }
    public int BacklogCount { get; set; }
    public int InReviewCount { get; set; }
    public int CriticalPriorityCount { get; set; }
    public int HighPriorityCount { get; set; }
    public int OverallProgressPercentage { get; set; }
    public int TotalEstimatedHours { get; set; }
    public int TotalLoggedHours { get; set; }
    public int StarredCount { get; set; }
}

public interface IProjectService
{
    Task<List<ProjectItem>> GetAllProjectsAsync(
        string? search = null,
        ProjectCategory? category = null,
        ProjectPriority? priority = null,
        ProjectStatus? status = null,
        string? sortBy = null);

    Task<ProjectItem?> GetProjectByIdAsync(int id);
    Task<ProjectItem> CreateProjectAsync(ProjectItem item);
    Task<ProjectItem> UpdateProjectAsync(ProjectItem item);
    Task<bool> DeleteProjectAsync(int id);
    Task<bool> ToggleStarAsync(int id);
    Task<bool> UpdateStatusAsync(int id, ProjectStatus status);
    Task<bool> UpdateProgressAsync(int id, int progress);
    Task<DashboardStats> GetDashboardStatsAsync();
    Task<List<ActivityLog>> GetRecentActivitiesAsync(int count = 10);
    Task ResetDatabaseAsync();
}
