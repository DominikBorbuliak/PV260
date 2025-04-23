using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.Persistence.Models;

namespace PV260.Project.Infrastructure.Persistence.Mappers;

public static class ArkFundsApiHoldingsResponseHoldingMapper
{
    public static ArkFundsHolding ToDomainModel(this ArkFundsApiHoldingsResponseHolding source)
    {
        return new ArkFundsHolding
        {
            Ticker = source.Ticker,
            Company = source.Company,
            Shares = source.Shares,
            Weight = source.Weight
        };
    }
}
