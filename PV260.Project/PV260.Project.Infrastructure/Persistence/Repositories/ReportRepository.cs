using PV260.Project.Domain.Models;
using PV260.Project.Infrastructure.Persistence.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using PV260.Project.Domain;
using PV260.Project.Domain.Exceptions;
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
        var reportJson = JsonSerializer.Serialize(holdings);
        var diffJson = JsonSerializer.Serialize(diff);

        var report = new ReportEntity()
        {
            ReportJson = reportJson,
            DiffJson = diffJson
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
