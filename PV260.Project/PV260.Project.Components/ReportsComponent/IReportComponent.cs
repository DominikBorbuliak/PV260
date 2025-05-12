using PV260.Project.Domain.Models;

namespace PV260.Project.Components.ReportsComponent;

public interface IReportsComponent
{
    Task GenerateAndNotifyAsync();

    Task<IList<HoldingChange>> GetClosestPreviousReportDiffAsync(DateTime? date);
}