using PV260.Project.Domain.Models;
using System.Text.Json;

namespace PV260.Project.Infrastructure.Persistence.Repositories;

public interface IReportRepository
{
    Task SaveReportAsync(IList<ArkFundsHolding> holdings, ReportDiff diff);

    Task<Report?> GetLatestReportAsync();
}
