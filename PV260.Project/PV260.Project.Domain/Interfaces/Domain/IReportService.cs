using PV260.Project.Domain.Models;

namespace PV260.Project.Domain.Interfaces.Domain;

public interface IReportService
{
    Task GenerateAndNotifyAsync();
    Task<IList<HoldingChange>> GetClosestPreviousReportDiffAsync(DateTime? date);
}
