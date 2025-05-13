using Microsoft.EntityFrameworkCore;
using PV260.Project.Domain.Interfaces.Persistence;
using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.Persistence.Mappers;
using PV260.Project.Infrastructure.Persistence.Models;

namespace PV260.Project.Infrastructure.Persistence.Repositories;

public class ReportRepository : IReportRepository
{
    private readonly AppDbContext _appDbContext;

    public ReportRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task SaveReportAsync(IList<ArkFundsHolding> holdings, ReportDiff diff)
    {
        var report = new ReportEntity()
        {
            CreatedAt = DateTime.UtcNow,
            Holdings = holdings.FromDomainModel(),
            Changes = diff.Changes.FromDomainModel()
        };

        _ = _appDbContext.Reports.Add(report);

        _ = await _appDbContext.SaveChangesAsync();
    }

    public async Task<Report?> GetLatestReportAsync()
    {
        ReportEntity? report = await _appDbContext.Reports
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();

        return report?.ToDomainModel();
    }

    public async Task<Report?> GetClosestPreviousReportAsync(DateTime date)
    {
        ReportEntity? report = await _appDbContext.Reports
            .Where(r => r.CreatedAt < date && r.Changes.Count > 0)
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();

        return report?.ToDomainModel();
    }
}
