using PV260.Project.Components.ReportsComponent.Services;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.ReportsComponent;

public class ReportsComponent(IReportService reportService) : IReportsComponent
{
    public async Task GenerateAndNotifyAsync()
    {
        await reportService.GenerateAndNotifyAsync();
    }

    public async Task<IList<HoldingChange>> GetClosestPreviousReportDiffAsync(DateTime? date)
    {
        return await reportService.GetClosestPreviousReportDiffAsync(date);
    }
}
