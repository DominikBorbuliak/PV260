using System.Text.Json.Serialization;

namespace PV260.Project.Infrastructure.ArkFunds.Models;

public class ArkFundsApiHoldingsResponse
{
    [JsonPropertyName("holdings")]
    public required IList<ArkFundsApiHoldingsResponseHolding> Holdings { get; set; }
}
