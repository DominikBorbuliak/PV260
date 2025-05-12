using PV260.Project.Components.ReportsComponent.DTOs;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.ReportsComponent.Mappers;

internal static class HoldingChangeDtoMapper
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