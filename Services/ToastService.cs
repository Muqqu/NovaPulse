namespace DotNetApp.Services;

public enum ToastLevel
{
    Info,
    Success,
    Warning,
    Error
}

public class ToastMessage
{
    public Guid Id { get; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public ToastLevel Level { get; set; } = ToastLevel.Info;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
}

public class ToastService
{
    public event Action<ToastMessage>? OnShow;

    public void ShowToast(string message, ToastLevel level = ToastLevel.Info, string title = "")
    {
        OnShow?.Invoke(new ToastMessage
        {
            Title = string.IsNullOrWhiteSpace(title) ? level.ToString() : title,
            Message = message,
            Level = level
        });
    }

    public void Success(string message, string title = "Success") => ShowToast(message, ToastLevel.Success, title);
    public void Info(string message, string title = "Information") => ShowToast(message, ToastLevel.Info, title);
    public void Warning(string message, string title = "Notice") => ShowToast(message, ToastLevel.Warning, title);
    public void Error(string message, string title = "Error") => ShowToast(message, ToastLevel.Error, title);
}
