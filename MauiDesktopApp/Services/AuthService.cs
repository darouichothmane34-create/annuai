namespace MauiDesktopApp.Services;

public class AuthService
{
    private const string AdminPassword = "Admin@2026";

    public bool VerifyAdminPassword(string password)
    {
        return string.Equals(password, AdminPassword, StringComparison.Ordinal);
    }
}
