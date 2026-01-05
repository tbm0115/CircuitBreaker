namespace CircuitBreakerApp.Models.Entities;

public class TenantMembership
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; } = default!;

    public string UserId { get; set; } = default!;
    public ApplicationUser User { get; set; } = default!;

    public TenantRole Role { get; set; } = TenantRole.Member;
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}

public enum TenantRole
{
    Owner = 0,
    Admin = 1,
    Member = 2,
    Viewer = 3
}
