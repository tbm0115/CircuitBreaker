using CircuitBreakerApp.Models.DTOs;

namespace CircuitBreakerApp.Services.Interfaces;

public interface IPanelService
{
    Task<IEnumerable<PanelSummaryDto>> GetPanelsForTenantAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<PanelDetailDto?> GetPanelBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<PanelDetailDto> CreatePanelAsync(CreatePanelRequest request, CancellationToken cancellationToken = default);
    Task<PanelDetailDto> UpdatePanelAsync(Guid panelId, UpdatePanelRequest request, CancellationToken cancellationToken = default);
}

public record CreatePanelRequest(Guid? TenantId, string Name, string AddressLine1, string? AddressLine2, string City, string State, string PostalCode, string Country, int SlotCount, bool AllowAnonymousView, IEnumerable<BreakerDto> Breakers);

public record UpdatePanelRequest(string Name, string? Description, bool AllowAnonymousView, int SlotCount, IEnumerable<BreakerDto> Breakers);
