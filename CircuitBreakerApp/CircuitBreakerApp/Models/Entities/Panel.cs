namespace CircuitBreakerApp.Models.Entities;

public class Panel
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid AddressId { get; set; }
    public Address Address { get; set; } = default!;

    public Guid? OwnerTenantId { get; set; }
    public Tenant? OwnerTenant { get; set; }
    public string Name { get; set; } = "Main";
    public string? Description { get; set; }
    public string PublicSlug { get; set; } = string.Empty;
    public bool IsPublic { get; set; } = false;
    public bool AllowAnonymousView { get; set; } = true;

    public int SlotCount { get; set; } = 40;

    public ICollection<PanelBreaker> Breakers { get; set; } = new List<PanelBreaker>();
    public ICollection<PanelAccess> AccessList { get; set; } = new List<PanelAccess>();
}
