namespace MauiDesktopApp.Services;

public class LogService
{
    private readonly string _logFilePath;

    public LogService()
    {
        var logDirectory = Path.Combine(FileSystem.AppDataDirectory, "logs");
        Directory.CreateDirectory(logDirectory);
        _logFilePath = Path.Combine(logDirectory, "annuai.log");
    }

    public Task WriteInfoAsync(string message)
    {
        return WriteAsync("INFO", message);
    }

    public Task WriteErrorAsync(string message)
    {
        return WriteAsync("ERROR", message);
    }

    private async Task WriteAsync(string level, string message)
    {
        var line = $"{DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} [{level}] {message}{Environment.NewLine}";
        await File.AppendAllTextAsync(_logFilePath, line);
    }
}
