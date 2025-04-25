using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.Persistence.Models;

namespace PV260.Project.Infrastructure.Persistence.Mappers;
public static class ReportEntityMapper
{
    public static Report ToDomainModel(this ReportEntity entity)
    {
        return new Report
        {
            Id = entity.Id,
            CreatedAt = entity.CreatedAt,
            Holdings = entity.Holdings.ToDomainModel(),
            Diff = entity.Changes.ToDomainModel()
        };
    }
}
