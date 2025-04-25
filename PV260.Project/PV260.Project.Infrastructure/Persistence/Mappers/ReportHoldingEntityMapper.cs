using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.Persistence.Models;

namespace PV260.Project.Infrastructure.Persistence.Mappers;

public static class ReportHoldingEntityMapper
{
    public static ReportHoldingEntity FromDomainModel(this ArkFundsHolding source)
    {
        return new ReportHoldingEntity
        {
            Ticker = source.Ticker,
            Company = source.Company,
            Shares = source.Shares,
            Weight = source.Weight
        };
    }

    public static ICollection<ReportHoldingEntity> FromDomainModel(this IList<ArkFundsHolding> source)
    {
        return [.. source.Select(FromDomainModel)];
    }

    public static ArkFundsHolding ToDomainModel(this ReportHoldingEntity source)
    {
        return new ArkFundsHolding
        {
            Ticker = source.Ticker,
            Company = source.Company,
            Shares = source.Shares,
            Weight = source.Weight
        };
    }

    public static IList<ArkFundsHolding> ToDomainModel(this ICollection<ReportHoldingEntity> source)
    {
        return [.. source.Select(ToDomainModel)];
    }
}
