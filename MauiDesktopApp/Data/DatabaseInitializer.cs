namespace MauiDesktopApp.Data;

public static class DatabaseInitializer
{
    public static async Task InitializeAsync()
    {
        await using var db = new AppDbContext();
        await db.Database.EnsureCreatedAsync();
    }
}
