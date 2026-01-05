namespace CircuitBreakerApp.Models.Entities;

public class Address
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Line1 { get; set; } = string.Empty;
    public string? Line2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string StateOrProvince { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public string NormalizedKey { get; set; } = string.Empty;

    public ICollection<Panel> Panels { get; set; } = new List<Panel>();
}
