using Microsoft.Maui.Controls;
using MauiDesktopApp.ViewModels;

#if WINDOWS
using Microsoft.UI.Xaml;
using Windows.System;
#endif

namespace MauiDesktopApp.Views;

public partial class VisitorPage : ContentPage
{
    private readonly VisitorViewModel _viewModel;

    public VisitorPage(VisitorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;

#if WINDOWS
        Loaded += OnLoaded;
#endif
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.InitializeAsync();
    }

#if WINDOWS
    private void OnLoaded(object? sender, EventArgs e)
    {
        if (Handler?.PlatformView is not FrameworkElement platformView)
        {
            return;
        }

        var accelerator = new KeyboardAccelerator
        {
            Key = VirtualKey.A,
            Modifiers = VirtualKeyModifiers.Control | VirtualKeyModifiers.Shift
        };

        accelerator.Invoked += async (_, args) =>
        {
            args.Handled = true;
            await Shell.Current.GoToAsync(nameof(AdminLoginPage));
        };

        platformView.KeyboardAccelerators.Add(accelerator);
    }
#endif
}
