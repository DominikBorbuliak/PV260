using PV260.Project.Domain.Models;
using PV260.Project.Server.Dtos;

namespace PV260.Project.Server.Mappers;

public static class HoldingChangeDtoMapper
{
    public static IList<HoldingChangeDto> ToDto(this IList<HoldingChange> holdingChanges)
    {
        return holdingChanges.Select(h => new HoldingChangeDto
        {
            Ticker = h.Ticker,
            Company = h.Company,
            ChangeType = h.ChangeType.ToString(),
            OldShares = h.OldShares,
            NewShares = h.NewShares,
            OldWeight = h.OldWeight,
            NewWeight = h.NewWeight
        }).ToList();
    }
}