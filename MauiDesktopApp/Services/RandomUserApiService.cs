using System.Text.Json;
using MauiDesktopApp.Data;
using MauiDesktopApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MauiDesktopApp.Services;

public class RandomUserApiService
{
    private readonly HttpClient _httpClient;
    private readonly AppDbContext _dbContext;
    private readonly LogService _logService;

    public RandomUserApiService(HttpClient httpClient, AppDbContext dbContext, LogService logService)
    {
        _httpClient = httpClient;
        _dbContext = dbContext;
        _logService = logService;
    }

    public async Task<int> ImportFrenchUsersAsync(int count = 10)
    {
        try
        {
            var sites = await _dbContext.Sites.AsNoTracking().ToListAsync();
            var services = await _dbContext.Services.AsNoTracking().ToListAsync();
            if (!sites.Any() || !services.Any())
            {
                await _logService.WriteErrorAsync("Import RandomUser impossible: sites/services absents.");
                return 0;
            }

            var response = await _httpClient.GetAsync($"https://randomuser.me/api/?results={count}&nat=fr");
            response.EnsureSuccessStatusCode();

            await using var stream = await response.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);
            var results = doc.RootElement.GetProperty("results");

            var rng = new Random();
            var newSalaries = new List<Salarie>();

            foreach (var item in results.EnumerateArray())
            {
                var first = item.GetProperty("name").GetProperty("first").GetString() ?? "Prenom";
                var last = item.GetProperty("name").GetProperty("last").GetString() ?? "Nom";
                var email = item.GetProperty("email").GetString() ?? $"{first}.{last}@example.fr";
                var phone = item.GetProperty("phone").GetString() ?? string.Empty;
                var cell = item.GetProperty("cell").GetString() ?? string.Empty;

                var site = sites[rng.Next(sites.Count)];
                var service = services[rng.Next(services.Count)];

                newSalaries.Add(new Salarie
                {
                    Prenom = first,
                    Nom = last,
                    Email = email,
                    TelephoneFixe = phone,
                    TelephonePortable = cell,
                    SiteId = site.Id,
                    ServiceId = service.Id
                });
            }

            _dbContext.Salaries.AddRange(newSalaries);
            await _dbContext.SaveChangesAsync();
            return newSalaries.Count;
        }
        catch (Exception ex)
        {
            await _logService.WriteErrorAsync($"Erreur API RandomUser: {ex.Message}");
            return 0;
        }
    }
}
