using Microsoft.Extensions.Options;
using PV260.Project.BusinessLayer.Interfaces.DataAccessLayer;
using PV260.Project.BusinessLayer.Models;
using PV260.Project.BusinessLayer.Options.ArkFundsApi;
using PV260.Project.DataAccessLayer.Mappers;
using PV260.Project.DataAccessLayer.Models;
using System.Collections.Specialized;
using System.Text.Json;
using System.Web;

namespace PV260.Project.DataAccessLayer.Data;

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
            throw new Exception("Base address is not set for ARK funds API.");
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

        return result.Holdings
            .Select(h => h.ToDomainModel())
            .ToList();
    }
}
