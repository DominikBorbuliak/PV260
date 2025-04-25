namespace PV260.Project.Server.Dtos;

public class HoldingChangeDto
{
    public string Ticker { get; set; } = string.Empty;
    public string Company { get; set; } = string.Empty;
    public string ChangeType { get; set; } = string.Empty;
    public int OldShares { get; set; } = 0;
    public int NewShares { get; set; }
}
