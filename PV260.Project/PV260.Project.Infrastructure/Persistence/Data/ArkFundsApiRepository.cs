using Microsoft.Extensions.Options;
using PV260.Project.Domain.Exceptions;
using PV260.Project.Domain.Interfaces.Infrastructure;
using PV260.Project.Domain.Models;
using PV260.Project.Domain.Options.ArkFundsApi;
using PV260.Project.Infrastructure.Persistence.Mappers;
using PV260.Project.Infrastructure.Persistence.Models;
using System.Text.Json;
using System.Web;

namespace PV260.Project.Infrastructure.Persistence.Data;

public class ArkFundsApiRepository : IArkFundsApiRepository
{
    private readonly ArkFundsApiOptions _arkFundsApiOptions;
    private readonly HttpClient _httpClient;

    public ArkFundsApiRepository(IOptions<ArkFundsApiOptions> arkFundsApiOptions, IHttpClientFactory httpClientFactory)
    {
        _arkFundsApiOptions = arkFundsApiOptions.Value;
        _httpClient = httpClientFactory.CreateClient(_arkFundsApiOptions.HttpClientKey);
    }

    public async Task<IList<ArkFundsHolding>> GetCurrentHoldingsAsync()
    {
        if (_httpClient.BaseAddress == null)
        {
            throw new ConfigurationException("Base address is not set for ARK funds API.");
        }

        var uriBuilder = new UriBuilder(_httpClient.BaseAddress)
        {
            Path = _arkFundsApiOptions.Endpoints.Holdings
        };

        var query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["symbol"] = _arkFundsApiOptions.Symbols.Innovation;

        uriBuilder.Query = query.ToString();

        var response = await _httpClient.GetAsync(uriBuilder.Uri);
        _ = response.EnsureSuccessStatusCode();

        var responseBody = await response.Content.ReadAsStringAsync();

        var result = JsonSerializer.Deserialize<ArkFundsApiHoldingsResponse>(responseBody)
            ?? throw new HttpRequestException($"Could not deserialize response of '{uriBuilder.Uri}'");

        return result.Holdings
            .Select(h => h.ToDomainModel())
            .ToList();
    }
}
