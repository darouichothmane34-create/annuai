using MauiDesktopApp.Data;
using MauiDesktopApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MauiDesktopApp.Services;

public class SiteService
{
    private readonly AppDbContext _dbContext;

    public SiteService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Site>> GetAllAsync()
    {
        return _dbContext.Sites.OrderBy(s => s.Nom).ToListAsync();
    }

    public Task<Site?> GetByIdAsync(int id)
    {
        return _dbContext.Sites.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Site> AddAsync(Site site)
    {
        _dbContext.Sites.Add(site);
        await _dbContext.SaveChangesAsync();
        return site;
    }

    public async Task<bool> UpdateAsync(Site site)
    {
        var existing = await _dbContext.Sites.FirstOrDefaultAsync(s => s.Id == site.Id);
        if (existing is null)
        {
            return false;
        }

        existing.Nom = site.Nom;
        existing.Ville = site.Ville;

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _dbContext.Sites.FirstOrDefaultAsync(s => s.Id == id);
        if (existing is null)
        {
            return false;
        }

        _dbContext.Sites.Remove(existing);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
