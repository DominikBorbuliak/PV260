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
            Holdings = JsonSerializer.Deserialize<IList<ArkFundsHolding>>(entity.ReportJson) ?? [],
            Diff = JsonSerializer.Deserialize<ReportDiff>(entity.DiffJson) ?? new()
        };
    }
}
