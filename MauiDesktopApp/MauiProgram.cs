using MauiDesktopApp.Data;
using MauiDesktopApp.Services;
using MauiDesktopApp.ViewModels;
using MauiDesktopApp.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MauiDesktopApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=annuai.db"));

        builder.Services.AddSingleton<DatabaseService>();
        builder.Services.AddScoped<SalarieService>();
        builder.Services.AddScoped<SiteService>();
        builder.Services.AddScoped<ServiceEntrepriseService>();
        builder.Services.AddSingleton<LogService>();
        builder.Services.AddSingleton<AuthService>();
        builder.Services.AddSingleton<PdfService>();
        builder.Services.AddHttpClient<RandomUserApiService>();

        builder.Services.AddTransient<VisitorViewModel>();
        builder.Services.AddTransient<AdminLoginViewModel>();
        builder.Services.AddTransient<AdminDashboardViewModel>();
        builder.Services.AddTransient<VisitorPage>();
        builder.Services.AddTransient<AdminLoginPage>();
        builder.Services.AddTransient<AdminDashboardPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        Routing.RegisterRoute(nameof(AdminLoginPage), typeof(AdminLoginPage));
        Routing.RegisterRoute(nameof(AdminDashboardPage), typeof(AdminDashboardPage));

        return app;
    }
}
