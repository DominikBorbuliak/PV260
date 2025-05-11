using PV260.Project.Domain.Models;

namespace PV260.Project.Domain.Interfaces.Infrastructure.Persistence;

public interface IReportRepository
{
    Task SaveReportAsync(IList<ArkFundsHolding> holdings, ReportDiff diff);

    Task<Report?> GetLatestReportAsync();

    Task<Report?> GetClosestPreviousReportAsync(DateTime date);
}
