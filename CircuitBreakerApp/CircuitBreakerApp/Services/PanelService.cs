using System.Security.Cryptography;
using System.Text;
using CircuitBreakerApp.Data;
using CircuitBreakerApp.Models.DTOs;
using CircuitBreakerApp.Models.Entities;
using CircuitBreakerApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CircuitBreakerApp.Services;

public class PanelService(ApplicationDbContext dbContext) : IPanelService
{
    public async Task<IEnumerable<PanelSummaryDto>> GetPanelsForTenantAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        var panels = await dbContext.Panels
            .Include(p => p.Address)
            .Where(p => p.OwnerTenantId == tenantId)
            .OrderBy(p => p.Name)
            .ToListAsync(cancellationToken);

        return panels.Select(p => new PanelSummaryDto(p.Id, p.Name, BuildAddressLabel(p.Address), p.PublicSlug, p.AllowAnonymousView));
    }

    public async Task<PanelDetailDto?> GetPanelBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var panel = await dbContext.Panels
            .Include(p => p.Address)
            .Include(p => p.Breakers)
            .FirstOrDefaultAsync(p => p.PublicSlug == slug, cancellationToken);

        if (panel is null)
        {
            return null;
        }

        return MapToDetail(panel);
    }

    public async Task<PanelDetailDto> CreatePanelAsync(CreatePanelRequest request, CancellationToken cancellationToken = default)
    {
        var address = await dbContext.Addresses.FirstOrDefaultAsync(a => a.NormalizedKey == NormalizeAddressKey(request.AddressLine1, request.AddressLine2, request.City, request.State, request.PostalCode, request.Country), cancellationToken);
        if (address is null)
        {
            address = new Address
            {
                Line1 = request.AddressLine1.Trim(),
                Line2 = request.AddressLine2?.Trim(),
                City = request.City.Trim(),
                StateOrProvince = request.State.Trim(),
                PostalCode = request.PostalCode.Trim(),
                Country = request.Country.Trim(),
                NormalizedKey = NormalizeAddressKey(request.AddressLine1, request.AddressLine2, request.City, request.State, request.PostalCode, request.Country)
            };
            dbContext.Addresses.Add(address);
        }

        var panel = new Panel
        {
            Name = request.Name.Trim(),
            Address = address,
            SlotCount = request.SlotCount,
            AllowAnonymousView = request.AllowAnonymousView,
            OwnerTenantId = request.TenantId,
            PublicSlug = GenerateSlug()
        };

        panel.Breakers = request.Breakers.Select(b => new PanelBreaker
        {
            SlotIndex = b.SlotIndex,
            Label = b.Label.Trim(),
            Amperage = b.Amperage,
            Type = b.Type,
            IsDuplex = b.IsDuplex
        }).ToList();

        dbContext.Panels.Add(panel);
        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDetail(panel);
    }

    public async Task<PanelDetailDto> UpdatePanelAsync(Guid panelId, UpdatePanelRequest request, CancellationToken cancellationToken = default)
    {
        var panel = await dbContext.Panels.Include(p => p.Breakers).Include(p => p.Address).FirstAsync(p => p.Id == panelId, cancellationToken);

        panel.Name = request.Name.Trim();
        panel.Description = request.Description?.Trim();
        panel.AllowAnonymousView = request.AllowAnonymousView;
        panel.SlotCount = request.SlotCount;

        dbContext.PanelBreakers.RemoveRange(panel.Breakers);
        panel.Breakers = request.Breakers.Select(b => new PanelBreaker
        {
            SlotIndex = b.SlotIndex,
            Label = b.Label.Trim(),
            Amperage = b.Amperage,
            Type = b.Type,
            IsDuplex = b.IsDuplex
        }).ToList();

        await dbContext.SaveChangesAsync(cancellationToken);

        return MapToDetail(panel);
    }

    private static string NormalizeAddressKey(string line1, string? line2, string city, string state, string postalCode, string country)
    {
        return string.Join('|', new[] { line1, line2, city, state, postalCode, country }
            .Where(s => !string.IsNullOrWhiteSpace(s))
            .Select(s => s!.Trim().ToUpperInvariant()));
    }

    private static string BuildAddressLabel(Address address)
    {
        var parts = new[]
        {
            address.Line1,
            address.Line2,
            address.City,
            address.StateOrProvince,
            address.PostalCode,
            address.Country
        }.Where(p => !string.IsNullOrWhiteSpace(p));

        return string.Join(", ", parts);
    }

    private static PanelDetailDto MapToDetail(Panel panel)
    {
        return new PanelDetailDto(panel.Id, panel.Name, panel.Description, BuildAddressLabel(panel.Address), panel.PublicSlug, panel.AllowAnonymousView, panel.SlotCount, panel.Breakers
            .OrderBy(b => b.SlotIndex)
            .Select(b => new BreakerDto(b.SlotIndex, b.Label, b.Amperage, b.Type, b.IsDuplex)));
    }

    private static string GenerateSlug()
    {
        var bytes = RandomNumberGenerator.GetBytes(6);
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
