namespace CircuitBreakerApp.Models.Entities;

public class PanelAccess
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PanelId { get; set; }
    public Panel Panel { get; set; } = default!;

    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    public Guid? TenantId { get; set; }
    public Tenant? Tenant { get; set; }

    public PanelAccessLevel AccessLevel { get; set; } = PanelAccessLevel.View;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

public enum PanelAccessLevel
{
    View = 0,
    Edit = 1,
    Owner = 2
}
