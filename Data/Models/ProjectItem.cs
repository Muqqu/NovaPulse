using System.ComponentModel.DataAnnotations;

namespace DotNetApp.Data.Models;

public class ProjectItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 100 characters")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required")]
    [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
    public string Description { get; set; } = string.Empty;

    [Required]
    public ProjectCategory Category { get; set; } = ProjectCategory.Fullstack;

    [Required]
    public ProjectPriority Priority { get; set; } = ProjectPriority.Medium;

    [Required]
    public ProjectStatus Status { get; set; } = ProjectStatus.InProgress;

    [Required(ErrorMessage = "Lead Developer / Assignee is required")]
    public string Assignee { get; set; } = "Team Lead";

    [Range(0, 100, ErrorMessage = "Progress must be between 0 and 100%")]
    public int ProgressPercentage { get; set; } = 0;

    [Range(1, 1000, ErrorMessage = "Estimated hours must be at least 1")]
    public int EstimatedHours { get; set; } = 20;

    [Range(0, 1000, ErrorMessage = "Logged hours must be non-negative")]
    public int LoggedHours { get; set; } = 0;

    [Required]
    public DateTime DueDate { get; set; } = DateTime.UtcNow.AddDays(14);

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    public string? Tags { get; set; } = "csharp,dotnet8,blazor";

    public bool IsStarred { get; set; } = false;
}
