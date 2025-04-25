namespace PV260.Project.Domain.Models;

public class HoldingChange
{
    public string Ticker { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public ChangeType ChangeType { get; set; }

    public int OldShares { get; set; } = 0;
    public int NewShares { get; set; }
}
