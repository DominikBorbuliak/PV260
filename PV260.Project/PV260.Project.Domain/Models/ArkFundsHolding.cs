namespace PV260.Project.Domain.Models;

public class ArkFundsHolding
{
    public string? Ticker { get; set; }

    public required string Company { get; set; }

    public int Shares { get; set; }

    public required decimal Weight { get; set; }
}
