using MauiDesktopApp.Views;

namespace MauiDesktopApp;

public partial class App : Application
{
    public App(VisitorPage visitorPage)
    {
        InitializeComponent();
        MainPage = new NavigationPage(visitorPage);
    }
}
