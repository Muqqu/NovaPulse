namespace DotNetApp.Data.Models;

public class ActivityLog
{
    public int Id { get; set; }
    public string Action { get; set; } = string.Empty; // "Created", "Updated", "Status Changed", "Deleted"
    public string EntityTitle { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string IconType { get; set; } = "info"; // "add", "edit", "check", "delete", "star"
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
