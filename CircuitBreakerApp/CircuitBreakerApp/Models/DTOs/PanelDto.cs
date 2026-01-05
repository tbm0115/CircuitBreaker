using CircuitBreakerApp.Models.Entities;

namespace CircuitBreakerApp.Models.DTOs;

public record BreakerDto(int SlotIndex, string Label, int Amperage, BreakerType Type, bool IsDuplex);

public record PanelSummaryDto(Guid Id, string Name, string AddressLabel, string PublicSlug, bool AllowAnonymousView);

public record PanelDetailDto(Guid Id, string Name, string? Description, string AddressLabel, string PublicSlug, bool AllowAnonymousView, int SlotCount, IEnumerable<BreakerDto> Breakers);
