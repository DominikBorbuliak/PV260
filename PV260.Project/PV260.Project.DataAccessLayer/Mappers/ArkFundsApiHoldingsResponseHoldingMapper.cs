using PV260.Project.BusinessLayer.Models;
using PV260.Project.DataAccessLayer.Models;

namespace PV260.Project.DataAccessLayer.Mappers;

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
