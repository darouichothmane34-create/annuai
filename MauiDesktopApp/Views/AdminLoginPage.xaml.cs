using Microsoft.Maui.Controls;
using MauiDesktopApp.ViewModels;

namespace MauiDesktopApp.Views;

public partial class AdminLoginPage : ContentPage
{
    public AdminLoginPage(AdminLoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
