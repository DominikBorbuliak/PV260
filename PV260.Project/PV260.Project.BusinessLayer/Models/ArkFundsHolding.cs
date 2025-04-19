namespace PV260.Project.BusinessLayer.Models;

public class ArkFundsHolding
{
    public required string Ticker { get; set; }

    public required string Company { get; set; }

    public required int Shares { get; set; }

    public required decimal Weight { get; set; }
}
