using Microsoft.Extensions.Options;
using PV260.Project.Domain.Exceptions;
using PV260.Project.Domain.Interfaces.ArkFunds;
using PV260.Project.Domain.Models;
using PV260.Project.Domain.Options.ArkFundsApi;
using PV260.Project.Infrastructure.ArkFunds.Mappers;
using PV260.Project.Infrastructure.ArkFunds.Models;
using PV260.Project.Infrastructure.Persistence.Mappers;
using System.Collections.Specialized;
using System.Text.Json;
using System.Web;

namespace PV260.Project.Infrastructure.ArkFunds.Repositories;

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

        NameValueCollection query = HttpUtility.ParseQueryString(uriBuilder.Query);
        query["symbol"] = _arkFundsApiOptions.Symbols.Innovation;

        uriBuilder.Query = query.ToString();

        HttpResponseMessage response = await _httpClient.GetAsync(uriBuilder.Uri);
        _ = response.EnsureSuccessStatusCode();

        string responseBody = await response.Content.ReadAsStringAsync();

        ArkFundsApiHoldingsResponse result = JsonSerializer.Deserialize<ArkFundsApiHoldingsResponse>(responseBody)
            ?? throw new HttpRequestException($"Could not deserialize response of '{uriBuilder.Uri}'");

        return result.Holdings.ToDomainModel();
    }
}
