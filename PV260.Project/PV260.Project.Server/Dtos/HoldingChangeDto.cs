namespace PV260.Project.Server.Dtos;

public class HoldingChangeDto
{
    public required string Ticker { get; set; }
    public required string Company { get; set; }
    public required string ChangeType { get; set; }
    public int OldShares { get; set; }
    public int NewShares { get; set; }
    public decimal OldWeight { get; set; }
    public decimal NewWeight { get; set; }
}
