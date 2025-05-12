using PV260.Project.Components.ReportsComponent.Services;
using Quartz;

namespace PV260.Project.Components.ReportsComponent.Jobs;

public class GenerateReportJob : IJob
{
    private readonly IReportService _reportService;

    public GenerateReportJob(IReportService reportService)
    {
        _reportService = reportService;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        await _reportService.GenerateAndNotifyAsync();
    }
}
