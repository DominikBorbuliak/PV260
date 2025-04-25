using System.Text.Json.Serialization;

namespace PV260.Project.Infrastructure.ArkFunds.Models;

public class ArkFundsApiHoldingsResponseHolding
{
    [JsonPropertyName("ticker")]
    public string? Ticker { get; set; }

    [JsonPropertyName("company")]
    public required string Company { get; set; }

    [JsonPropertyName("shares")]
    public int? Shares { get; set; }

    [JsonPropertyName("weight")]
    public required decimal Weight { get; set; }
}