using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiDesktopApp.Services;

namespace MauiDesktopApp.ViewModels;

public partial class AdminLoginViewModel : ObservableObject
{
    private readonly AuthService _authService;
    private readonly LogService _logService;

    [ObservableProperty]
    private string _password = string.Empty;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    public AdminLoginViewModel(AuthService authService, LogService logService)
    {
        _authService = authService;
        _logService = logService;
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        var isValid = _authService.VerifyAdminPassword(Password);
        if (!isValid)
        {
            await _logService.WriteErrorAsync("Échec authentification administrateur.");
            ErrorMessage = "Mot de passe invalide.";
            return;
        }

        await _logService.WriteInfoAsync("Authentification administrateur réussie.");
        Password = string.Empty;
        await Shell.Current.GoToAsync(nameof(Views.AdminDashboardPage));
    }
}
