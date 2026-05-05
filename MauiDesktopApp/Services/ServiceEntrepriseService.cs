using MauiDesktopApp.Data;
using MauiDesktopApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MauiDesktopApp.Services;

public class ServiceEntrepriseService
{
    private readonly AppDbContext _dbContext;

    public ServiceEntrepriseService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task<List<Service>> GetAllAsync()
    {
        return _dbContext.Services.OrderBy(s => s.Nom).ToListAsync();
    }

    public Task<Service?> GetByIdAsync(int id)
    {
        return _dbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Service> AddAsync(Service service)
    {
        _dbContext.Services.Add(service);
        await _dbContext.SaveChangesAsync();
        return service;
    }

    public async Task<bool> UpdateAsync(Service service)
    {
        var existing = await _dbContext.Services.FirstOrDefaultAsync(s => s.Id == service.Id);
        if (existing is null)
        {
            return false;
        }

        existing.Nom = service.Nom;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _dbContext.Services.FirstOrDefaultAsync(s => s.Id == id);
        if (existing is null)
        {
            return false;
        }

        _dbContext.Services.Remove(existing);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
