using MauiDesktopApp.Data;
using MauiDesktopApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MauiDesktopApp.Services;

public class SalarieService
{
    private readonly AppDbContext _dbContext;

    public SalarieService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Salarie>> GetAllAsync()
    {
        return _dbContext.Salaries
            .Include(s => s.Site)
            .Include(s => s.Service)
            .OrderBy(s => s.Nom)
            .ThenBy(s => s.Prenom)
            .ToListAsync();
    }

    public Task<Salarie?> GetByIdAsync(int id)
    {
        return _dbContext.Salaries
            .Include(s => s.Site)
            .Include(s => s.Service)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public Task<List<Salarie>> SearchAsync(string terme)
    {
        terme = terme.Trim();

        return _dbContext.Salaries
            .Include(s => s.Site)
            .Include(s => s.Service)
            .Where(s => s.Nom.Contains(terme)
                     || s.Prenom.Contains(terme)
                     || s.Email.Contains(terme)
                     || s.Service!.Nom.Contains(terme)
                     || s.Site!.Nom.Contains(terme)
                     || s.Site.Ville.Contains(terme))
            .OrderBy(s => s.Nom)
            .ThenBy(s => s.Prenom)
            .ToListAsync();
    }

    public async Task<Salarie> AddAsync(Salarie salarie)
    {
        _dbContext.Salaries.Add(salarie);
        await _dbContext.SaveChangesAsync();
        return salarie;
    }

    public async Task<bool> UpdateAsync(Salarie salarie)
    {
        var existing = await _dbContext.Salaries.FirstOrDefaultAsync(s => s.Id == salarie.Id);
        if (existing is null)
        {
            return false;
        }

        existing.Prenom = salarie.Prenom;
        existing.Nom = salarie.Nom;
        existing.Email = salarie.Email;
        existing.SiteId = salarie.SiteId;
        existing.ServiceId = salarie.ServiceId;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _dbContext.Salaries.FirstOrDefaultAsync(s => s.Id == id);
        if (existing is null)
        {
            return false;
        }

        _dbContext.Salaries.Remove(existing);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
