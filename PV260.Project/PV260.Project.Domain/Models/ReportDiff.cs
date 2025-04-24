namespace PV260.Project.Domain.Models;

public class ReportDiff
{
    public IList<HoldingChange> Changes { get; set; } = [];
}
