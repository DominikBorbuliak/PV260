using PV260.Project.Components.ReportComponent.DTOs;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.ReportComponent.Mappers;

public static class HoldingChangeDtoMapper
{
    public static HoldingChangeDto ToDto(this HoldingChange source)
    {
        return new HoldingChangeDto
        {
            Ticker = source.Ticker,
            Company = source.Company,
            ChangeType = source.ChangeType.ToString(),
            OldShares = source.OldShares,
            NewShares = source.NewShares,
            OldWeight = source.OldWeight,
            NewWeight = source.NewWeight
        };
    }

    public static IList<HoldingChangeDto> ToDto(this IList<HoldingChange> source)
    {
        return [.. source.Select(ToDto)];
    }
}