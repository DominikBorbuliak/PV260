using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.Persistence.Models;

namespace PV260.Project.Infrastructure.Persistence.Mappers;

public static class ReportChangeEntityMapper
{
    public static ReportChangeEntity FromDomainModel(this HoldingChange source)
    {
        return new ReportChangeEntity
        {
            Ticker = source.Ticker,
            Company = source.Company,
            ChangeType = source.ChangeType,
            OldShares = source.OldShares,
            NewShares = source.NewShares
        };
    }

    public static ICollection<ReportChangeEntity> FromDomainModel(this IList<HoldingChange> source)
    {
        return [.. source.Select(FromDomainModel)];
    }

    public static HoldingChange ToDomainModel(this ReportChangeEntity source)
    {
        return new HoldingChange
        {
            Ticker = source.Ticker,
            Company = source.Company,
            ChangeType = source.ChangeType,
            OldShares = source.OldShares,
            NewShares = source.NewShares
        };
    }

    public static IList<HoldingChange> ToDomainModel(this ICollection<ReportChangeEntity> source)
    {
        return [.. source.Select(ToDomainModel)];
    }
}
