using System.Text.Json.Serialization;

namespace PV260.Project.DataAccessLayer.Models;

public class ArkFundsApiHoldingsResponse
{
    [JsonPropertyName("holdings")]
    public required IList<ArkFundsApiHoldingsResponseHolding> Holdings { get; set; }
}
