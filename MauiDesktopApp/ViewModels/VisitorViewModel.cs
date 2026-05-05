using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiDesktopApp.Models;
using MauiDesktopApp.Services;

namespace MauiDesktopApp.ViewModels;

public partial class VisitorViewModel : ObservableObject
{
    private readonly SalarieService _salarieService;
    private readonly SiteService _siteService;
    private readonly ServiceEntrepriseService _serviceEntrepriseService;
    private readonly PdfService _pdfService;
    private readonly LogService _logService;

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private Site? _selectedSite;

    [ObservableProperty]
    private Service? _selectedService;

    [ObservableProperty]
    private Salarie? _selectedSalarie;

    [ObservableProperty]
    private bool _isBusy;

    public ObservableCollection<Site> Sites { get; } = new();
    public ObservableCollection<Service> Services { get; } = new();
    public ObservableCollection<Salarie> SearchResults { get; } = new();

    public VisitorViewModel(
        SalarieService salarieService,
        SiteService siteService,
        ServiceEntrepriseService serviceEntrepriseService,
        PdfService pdfService,
        LogService logService)
    {
        _salarieService = salarieService;
        _siteService = siteService;
        _serviceEntrepriseService = serviceEntrepriseService;
        _pdfService = pdfService;
        _logService = logService;
    }


    [ObservableProperty]
    private string _exportStatus = string.Empty;

    [RelayCommand]
    public async Task ExportPdfAsync()
    {
        if (SelectedSalarie is null)
        {
            ExportStatus = "Veuillez sélectionner un salarié.";
            return;
        }

        var path = await _pdfService.ExportSalarieAsync(SelectedSalarie);
        if (string.IsNullOrWhiteSpace(path))
        {
            ExportStatus = "Échec export PDF (voir logs).";
            await _logService.WriteErrorAsync("Export PDF échoué depuis VisitorViewModel.");
            return;
        }

        ExportStatus = $"PDF exporté : {path}";
    }
    [RelayCommand]
    public async Task InitializeAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        try
        {
            Sites.Clear();
            Services.Clear();

            var sites = await _siteService.GetAllAsync();
            var services = await _serviceEntrepriseService.GetAllAsync();

            foreach (var site in sites) Sites.Add(site);
            foreach (var service in services) Services.Add(service);

            await SearchAsync();
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task SearchAsync()
    {
        if (IsBusy) return;

        IsBusy = true;
        try
        {
            var query = string.IsNullOrWhiteSpace(SearchText)
                ? await _salarieService.GetAllAsync()
                : await _salarieService.SearchAsync(SearchText);

            if (SelectedSite is not null)
            {
                query = query.Where(s => s.SiteId == SelectedSite.Id).ToList();
            }

            if (SelectedService is not null)
            {
                query = query.Where(s => s.ServiceId == SelectedService.Id).ToList();
            }

            SearchResults.Clear();
            foreach (var salarie in query)
            {
                SearchResults.Add(salarie);
            }

            SelectedSalarie = SearchResults.FirstOrDefault();
        }
        finally
        {
            IsBusy = false;
        }
    }
}
