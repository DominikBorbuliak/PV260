using PV260.Project.Components.ReportsComponent.Services;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.ReportComponent;

public class ReportComponent : IReportComponent
{
    private readonly IReportService _reportService;

    public ReportComponent(IReportService reportService)
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
