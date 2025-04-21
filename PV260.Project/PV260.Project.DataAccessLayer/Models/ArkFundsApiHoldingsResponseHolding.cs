using System.Text.Json.Serialization;

namespace PV260.Project.DataAccessLayer.Models;

public class ArkFundsApiHoldingsResponseHolding
{
    [JsonPropertyName("ticker")]
    public required string Ticker { get; set; }

    [JsonPropertyName("company")]
    public required string Company { get; set; }

    [JsonPropertyName("shares")]
    public required int Shares { get; set; }

    [JsonPropertyName("weight")]
    public required decimal Weight { get; set; }
}