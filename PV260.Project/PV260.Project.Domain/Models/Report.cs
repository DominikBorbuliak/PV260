namespace PV260.Project.Domain.Models;

public class Report
{
    public Guid Id { get; set; }

    public ReportDiff? Diff { get; set; }

    public DateTime CreatedAt { get; set; }

    public IList<ArkFundsHolding> Holdings { get; set; } = new List<ArkFundsHolding>();
}
