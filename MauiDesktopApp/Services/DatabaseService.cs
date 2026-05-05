using MauiDesktopApp.Data;

namespace MauiDesktopApp.Services;

public class DatabaseService
{
    public Task InitializeAsync()
    {
        return DatabaseInitializer.InitializeAsync();
    }
}
