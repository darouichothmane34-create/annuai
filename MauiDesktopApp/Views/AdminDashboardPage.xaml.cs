using Microsoft.Maui.Controls;
using MauiDesktopApp.ViewModels;

namespace MauiDesktopApp.Views;

public partial class AdminDashboardPage : TabbedPage
{
    private readonly AdminDashboardViewModel _viewModel;

    public AdminDashboardPage(AdminDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }
}
