using PV260.Project.Components.ReportsComponent.Services;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.ReportsComponent;

public class ReportsComponent : IReportsComponent
{
    private readonly IReportService _reportService;

    public ReportsComponent(IReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task GenerateAndNotifyAsync()
    {
        await _reportService.GenerateAndNotifyAsync();
    }

    public async Task<IList<HoldingChange>> GetClosestPreviousReportDiffAsync(DateTime? date)
    {
        return await _reportService.GetClosestPreviousReportDiffAsync(date);
    }
}
