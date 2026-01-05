using Microsoft.AspNetCore.Identity;

namespace CircuitBreakerApp.Models.Entities;

public class ApplicationUser : IdentityUser
{
    public string? DisplayName { get; set; }

    public ICollection<TenantMembership> Memberships { get; set; } = new List<TenantMembership>();
    public ICollection<PanelAccess> PanelAccessEntries { get; set; } = new List<PanelAccess>();
}
