using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using PV260.Project.Infrastructure.Persistence.Mappers;

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
            Holdings = holdings.Select(h => new ReportHoldingEntity
            {
                Ticker = h.Ticker,
                Company = h.Company,
                Shares = h.Shares,
                Weight = h.Weight
            }).ToList(),

            Changes = diff.Changes.Select(c => new ReportChangeEntity
            {
                Ticker = c.Ticker,
                Company = c.Company,
                ChangeType = c.ChangeType,
                OldShares = c.OldShares,
                NewShares = c.NewShares
            }).ToList()
        };

        _appDbContext.Reports.Add(report);
        await _appDbContext.SaveChangesAsync();
    }

    public async Task<Report?> GetLatestReportAsync()
    {
        var report =  await _appDbContext.Reports
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();

        return report?.ToDomainModel();
    }
}
