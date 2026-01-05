namespace CircuitBreakerApp.Models.Entities;

public class PanelBreaker
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid PanelId { get; set; }
    public Panel Panel { get; set; } = default!;

    public int SlotIndex { get; set; }
    public string Label { get; set; } = "N/A";
    public int Amperage { get; set; }
    public BreakerType Type { get; set; } = BreakerType.Unused;
    public bool IsDuplex { get; set; } = false;
}

public enum BreakerType
{
    Unused = 0,
    SinglePole = 1,
    DoublePole = 2,
    DoubleCircuit = 3
}
