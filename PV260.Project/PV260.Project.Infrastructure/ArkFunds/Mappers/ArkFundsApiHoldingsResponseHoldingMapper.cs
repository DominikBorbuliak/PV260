using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.ArkFunds.Models;

namespace PV260.Project.Infrastructure.ArkFunds.Mappers;

public static class ArkFundsApiHoldingsResponseHoldingMapper
{
    public static ArkFundsHolding ToDomainModel(this ArkFundsApiHoldingsResponseHolding source)
    {
        return new ArkFundsHolding
        {
            Ticker = source.Ticker ?? string.Empty,
            Company = source.Company,
            Shares = source.Shares ?? 0,
            Weight = source.Weight
        };
    }

    public static IList<ArkFundsHolding> ToDomainModel(this IList<ArkFundsApiHoldingsResponseHolding> source)
    {
        return [.. source.Select(ToDomainModel)];
    }
}
