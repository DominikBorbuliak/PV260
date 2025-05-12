using PV260.Project.Domain.Models;

namespace PV260.Project.Components.ReportComponent;

public interface IReportComponent
{
    Task GenerateAndNotifyAsync();

    Task<IList<HoldingChange>> GetClosestPreviousReportDiffAsync(DateTime? date);
}