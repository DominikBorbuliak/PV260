namespace PV260.Project.Domain.Models;

public class Report
{
    public Guid Id { get; set; }

    public IList<HoldingChange> Diff { get; set; } = [];

    public DateTime CreatedAt { get; set; }

    public required IList<ArkFundsHolding> Holdings { get; set; }
}
