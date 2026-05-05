using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiDesktopApp.Models;
using MauiDesktopApp.Services;

namespace MauiDesktopApp.ViewModels;

public partial class AdminDashboardViewModel : ObservableObject
{
    private readonly SiteService _siteService;
    private readonly ServiceEntrepriseService _serviceService;
    private readonly SalarieService _salarieService;
    private readonly LogService _logService;
    private readonly RandomUserApiService _randomUserApiService;

    public ObservableCollection<Site> Sites { get; } = new();
    public ObservableCollection<Service> Services { get; } = new();
    public ObservableCollection<Salarie> Salaries { get; } = new();

    [ObservableProperty] private Site? _selectedSite;
    [ObservableProperty] private Service? _selectedService;
    [ObservableProperty] private Salarie? _selectedSalarie;

    [ObservableProperty] private string _siteNom = string.Empty;
    [ObservableProperty] private string _siteVille = string.Empty;

    [ObservableProperty] private string _serviceNom = string.Empty;

    [ObservableProperty] private string _salariePrenom = string.Empty;
    [ObservableProperty] private string _salarieNom = string.Empty;
    [ObservableProperty] private string _salarieFixe = string.Empty;
    [ObservableProperty] private string _salariePortable = string.Empty;
    [ObservableProperty] private string _salarieEmail = string.Empty;
    [ObservableProperty] private Site? _salarieSite;
    [ObservableProperty] private Service? _selectedSalarieService;

    [ObservableProperty] private string _statusMessage = string.Empty;

    public AdminDashboardViewModel(SiteService siteService, ServiceEntrepriseService serviceService, SalarieService salarieService, LogService logService, RandomUserApiService randomUserApiService)
    {
        _siteService = siteService;
        _serviceService = serviceService;
        _salarieService = salarieService;
        _logService = logService;
        _randomUserApiService = randomUserApiService;
    }

    [RelayCommand]
    public async Task InitializeAsync()
    {
        await RefreshSitesAsync();
        await RefreshServicesAsync();
        await RefreshSalariesAsync();
    }

    [RelayCommand]
    public async Task AddSiteAsync()
    {
        if (string.IsNullOrWhiteSpace(SiteNom) || string.IsNullOrWhiteSpace(SiteVille))
        {
            StatusMessage = "Nom et ville du site sont obligatoires.";
            return;
        }
        try
        {
            await _siteService.AddAsync(new Site { Nom = SiteNom.Trim(), Ville = SiteVille.Trim() });
            SiteNom = SiteVille = string.Empty;
            await RefreshSitesAsync();
        }
        catch (Exception ex) { await LogErrorAsync("AddSite", ex); }
    }

    [RelayCommand]
    public async Task UpdateSiteAsync()
    {
        if (SelectedSite is null || string.IsNullOrWhiteSpace(SiteNom) || string.IsNullOrWhiteSpace(SiteVille)) return;
        try
        {
            SelectedSite.Nom = SiteNom.Trim();
            SelectedSite.Ville = SiteVille.Trim();
            await _siteService.UpdateAsync(SelectedSite);
            await RefreshSitesAsync();
        }
        catch (Exception ex) { await LogErrorAsync("UpdateSite", ex); }
    }

    [RelayCommand]
    public async Task DeleteSiteAsync()
    {
        if (SelectedSite is null) return;
        try
        {
            await _siteService.DeleteAsync(SelectedSite.Id);
            SiteNom = SiteVille = string.Empty;
            await RefreshSitesAsync();
        }
        catch (Exception ex) { await LogErrorAsync("DeleteSite", ex); }
    }

    [RelayCommand]
    public async Task AddServiceAsync()
    {
        if (string.IsNullOrWhiteSpace(ServiceNom)) { StatusMessage = "Nom du service obligatoire."; return; }
        try
        {
            await _serviceService.AddAsync(new Service { Nom = ServiceNom.Trim() });
            ServiceNom = string.Empty;
            await RefreshServicesAsync();
        }
        catch (Exception ex) { await LogErrorAsync("AddService", ex); }
    }

    [RelayCommand]
    public async Task UpdateServiceAsync()
    {
        if (SelectedService is null || string.IsNullOrWhiteSpace(ServiceNom)) return;
        try
        {
            SelectedService.Nom = ServiceNom.Trim();
            await _serviceService.UpdateAsync(SelectedService);
            await RefreshServicesAsync();
        }
        catch (Exception ex) { await LogErrorAsync("UpdateService", ex); }
    }

    [RelayCommand]
    public async Task DeleteServiceAsync()
    {
        if (SelectedService is null) return;
        try
        {
            await _serviceService.DeleteAsync(SelectedService.Id);
            ServiceNom = string.Empty;
            await RefreshServicesAsync();
        }
        catch (Exception ex) { await LogErrorAsync("DeleteService", ex); }
    }

    [RelayCommand]
    public async Task AddSalarieAsync()
    {
        if (!ValidateSalarie()) return;
        try
        {
            await _salarieService.AddAsync(new Salarie { Prenom = SalariePrenom.Trim(), Nom = SalarieNom.Trim(), TelephoneFixe = SalarieFixe.Trim(), TelephonePortable = SalariePortable.Trim(), Email = SalarieEmail.Trim(), SiteId = SalarieSite!.Id, ServiceId = SelectedSalarieService!.Id });
            ClearSalarieForm();
            await RefreshSalariesAsync();
        }
        catch (Exception ex) { await LogErrorAsync("AddSalarie", ex); }
    }

    [RelayCommand]
    public async Task UpdateSalarieAsync()
    {
        if (SelectedSalarie is null || !ValidateSalarie()) return;
        try
        {
            SelectedSalarie.Prenom = SalariePrenom.Trim();
            SelectedSalarie.Nom = SalarieNom.Trim();
            SelectedSalarie.TelephoneFixe = SalarieFixe.Trim();
            SelectedSalarie.TelephonePortable = SalariePortable.Trim();
            SelectedSalarie.Email = SalarieEmail.Trim();
            SelectedSalarie.SiteId = SalarieSite!.Id;
            SelectedSalarie.ServiceId = SelectedSalarieService!.Id;
            await _salarieService.UpdateAsync(SelectedSalarie);
            await RefreshSalariesAsync();
        }
        catch (Exception ex) { await LogErrorAsync("UpdateSalarie", ex); }
    }

    [RelayCommand]
    public async Task DeleteSalarieAsync()
    {
        if (SelectedSalarie is null) return;
        try
        {
            await _salarieService.DeleteAsync(SelectedSalarie.Id);
            ClearSalarieForm();
            await RefreshSalariesAsync();
        }
        catch (Exception ex) { await LogErrorAsync("DeleteSalarie", ex); }
    }


    [RelayCommand]
    public async Task ImportRandomUserAsync()
    {
        var imported = await _randomUserApiService.ImportFrenchUsersAsync(10);
        StatusMessage = imported > 0
            ? $"{imported} salariés importés depuis RandomUser."
            : "Aucun salarié importé (voir logs).";
        await RefreshSalariesAsync();
    }
    partial void OnSelectedSiteChanged(Site? value)
    {
        if (value is null) return;
        SiteNom = value.Nom; SiteVille = value.Ville;
    }
    partial void OnSelectedServiceChanged(Service? value)
    {
        if (value is null) return;
        ServiceNom = value.Nom;
    }
    partial void OnSelectedSalarieChanged(Salarie? value)
    {
        if (value is null) return;
        SalariePrenom = value.Prenom; SalarieNom = value.Nom; SalarieFixe = value.TelephoneFixe; SalariePortable = value.TelephonePortable; SalarieEmail = value.Email;
        SalarieSite = Sites.FirstOrDefault(s => s.Id == value.SiteId);
        SelectedSalarieService = Services.FirstOrDefault(s => s.Id == value.ServiceId);
    }

    private async Task RefreshSitesAsync() { Sites.Clear(); foreach (var i in await _siteService.GetAllAsync()) Sites.Add(i); }
    private async Task RefreshServicesAsync() { Services.Clear(); foreach (var i in await _serviceService.GetAllAsync()) Services.Add(i); }
    private async Task RefreshSalariesAsync() { Salaries.Clear(); foreach (var i in await _salarieService.GetAllAsync()) Salaries.Add(i); }
    private bool ValidateSalarie()
    {
        if (string.IsNullOrWhiteSpace(SalariePrenom) || string.IsNullOrWhiteSpace(SalarieNom) || string.IsNullOrWhiteSpace(SalarieEmail) || SalarieSite is null || SelectedSalarieService is null)
        { StatusMessage = "Prénom, nom, email, site et service sont obligatoires."; return false; }
        return true;
    }
    private void ClearSalarieForm() { SalariePrenom = SalarieNom = SalarieFixe = SalariePortable = SalarieEmail = string.Empty; SalarieSite = null; SelectedSalarieService = null; }
    private Task LogErrorAsync(string action, Exception ex) => _logService.WriteErrorAsync($"{action} failed: {ex.Message}");
}
