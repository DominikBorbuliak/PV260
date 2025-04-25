using PV260.Project.Domain.Models;
using System.Text.Json;
using PV260.Project.Infrastructure.Persistence.Models;

namespace PV260.Project.Infrastructure.Persistence.Mappers;
public static class ReportMapper
{
    public static Report ToDomainModel(this ReportEntity entity)
    {
        return new Report
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Holdings = entity.Holdings
                .Select(h => new ArkFundsHolding
                {
                    Ticker = h.Ticker,
                    Company = h.Company,
                    Shares = h.Shares,
                    Weight = h.Weight
                })
                .ToList(),
            
            Diff = entity.Changes
                .Select(c => new HoldingChange
                {
                    Ticker = c.Ticker,
                    Company = c.Company,
                    ChangeType = c.ChangeType,
                    OldShares = c.OldShares,
                    NewShares = c.NewShares
                })
                .ToList()
        };
    }
}
