using MauiDesktopApp.Models;
using Microsoft.EntityFrameworkCore;

namespace MauiDesktopApp.Data;

public class AppDbContext : DbContext
{
    public DbSet<Site> Sites => Set<Site>();
    public DbSet<Service> Services => Set<Service>();
    public DbSet<Salarie> Salaries => Set<Salarie>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            // WAL + Shared cache + Busy timeout pour réduire les conflits en multi-instance.
            optionsBuilder.UseSqlite("Data Source=annuai.db;Cache=Shared;Pooling=True;Default Timeout=15");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Site>()
            .HasMany(s => s.Salaries)
            .WithOne(s => s.Site)
            .HasForeignKey(s => s.SiteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Service>()
            .HasMany(s => s.Salaries)
            .WithOne(s => s.Service)
            .HasForeignKey(s => s.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Site>().HasData(
            new Site { Id = 1, Nom = "Siège Paris", Ville = "Paris" },
            new Site { Id = 2, Nom = "Agence Lyon", Ville = "Lyon" },
            new Site { Id = 3, Nom = "Agence Marseille", Ville = "Marseille" },
            new Site { Id = 4, Nom = "Agence Toulouse", Ville = "Toulouse" },
            new Site { Id = 5, Nom = "Agence Nantes", Ville = "Nantes" }
        );

        modelBuilder.Entity<Service>().HasData(
            new Service { Id = 1, Nom = "Ressources Humaines" },
            new Service { Id = 2, Nom = "Informatique" },
            new Service { Id = 3, Nom = "Finance" },
            new Service { Id = 4, Nom = "Commercial" },
            new Service { Id = 5, Nom = "Support" }
        );

        modelBuilder.Entity<Salarie>().HasData(
            new Salarie { Id = 1, Prenom = "Camille", Nom = "Martin", TelephoneFixe = "01 44 10 20 30", TelephonePortable = "06 11 22 33 44", Email = "camille.martin@annuai.fr", ServiceId = 1, SiteId = 1 },
            new Salarie { Id = 2, Prenom = "Lucas", Nom = "Bernard", TelephoneFixe = "01 44 10 20 31", TelephonePortable = "06 22 33 44 55", Email = "lucas.bernard@annuai.fr", ServiceId = 2, SiteId = 1 },
            new Salarie { Id = 3, Prenom = "Emma", Nom = "Dubois", TelephoneFixe = "04 72 10 20 30", TelephonePortable = "06 33 44 55 66", Email = "emma.dubois@annuai.fr", ServiceId = 3, SiteId = 2 },
            new Salarie { Id = 4, Prenom = "Hugo", Nom = "Petit", TelephoneFixe = "04 91 10 20 30", TelephonePortable = "06 44 55 66 77", Email = "hugo.petit@annuai.fr", ServiceId = 4, SiteId = 3 },
            new Salarie { Id = 5, Prenom = "Lina", Nom = "Moreau", TelephoneFixe = "05 61 10 20 30", TelephonePortable = "06 55 66 77 88", Email = "lina.moreau@annuai.fr", ServiceId = 5, SiteId = 4 },
            new Salarie { Id = 6, Prenom = "Noah", Nom = "Garcia", TelephoneFixe = "02 40 10 20 30", TelephonePortable = "06 66 77 88 99", Email = "noah.garcia@annuai.fr", ServiceId = 2, SiteId = 5 },
            new Salarie { Id = 7, Prenom = "Chloé", Nom = "Roux", TelephoneFixe = "04 72 10 20 31", TelephonePortable = "06 77 88 99 00", Email = "chloe.roux@annuai.fr", ServiceId = 4, SiteId = 2 },
            new Salarie { Id = 8, Prenom = "Jules", Nom = "Fournier", TelephoneFixe = "04 91 10 20 31", TelephonePortable = "06 88 99 00 11", Email = "jules.fournier@annuai.fr", ServiceId = 3, SiteId = 3 }
        );
    }
}
