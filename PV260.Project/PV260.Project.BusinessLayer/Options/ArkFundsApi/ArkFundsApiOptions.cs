namespace PV260.Project.BusinessLayer.Options.ArkFundsApi;

public class ArkFundsApiOptions
{
    public const string Key = "ArkFundsApi";

    public required string HttpClientKey { get; set; }
    public required string BaseUrl { get; set; }
    public required ArkFundsApiSymbols Symbols { get; set; }
    public required ArkFundsApiEndpoints Endpoints { get; set; }
}
