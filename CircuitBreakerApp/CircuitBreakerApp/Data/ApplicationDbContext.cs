using CircuitBreakerApp.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CircuitBreakerApp.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<TenantMembership> TenantMemberships => Set<TenantMembership>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Panel> Panels => Set<Panel>();
    public DbSet<PanelBreaker> PanelBreakers => Set<PanelBreaker>();
    public DbSet<PanelAccess> PanelAccessEntries => Set<PanelAccess>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Tenant>(entity =>
        {
            entity.HasIndex(t => t.NormalizedName).IsUnique();
        });

        builder.Entity<Address>(entity =>
        {
            entity.HasIndex(a => a.NormalizedKey).IsUnique();
        });

        builder.Entity<Panel>(entity =>
        {
            entity.HasIndex(p => p.PublicSlug).IsUnique();
            entity.HasIndex(p => new { p.AddressId, p.Name }).IsUnique();

            entity.HasOne(p => p.Address)
                .WithMany(a => a.Panels)
                .HasForeignKey(p => p.AddressId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(p => p.OwnerTenant)
                .WithMany(t => t.Panels)
                .HasForeignKey(p => p.OwnerTenantId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(p => p.Breakers)
                .WithOne(b => b.Panel)
                .HasForeignKey(b => b.PanelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<PanelBreaker>(entity =>
        {
            entity.HasIndex(b => new { b.PanelId, b.SlotIndex }).IsUnique();
        });

        builder.Entity<PanelAccess>(entity =>
        {
            entity.HasIndex(a => new { a.PanelId, a.UserId, a.TenantId }).IsUnique();
            entity.HasOne(a => a.Panel)
                .WithMany(p => p.AccessList)
                .HasForeignKey(a => a.PanelId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<TenantMembership>(entity =>
        {
            entity.HasIndex(m => new { m.TenantId, m.UserId }).IsUnique();
            entity.HasOne(m => m.Tenant)
                .WithMany(t => t.Memberships)
                .HasForeignKey(m => m.TenantId);
        });
    }
}
