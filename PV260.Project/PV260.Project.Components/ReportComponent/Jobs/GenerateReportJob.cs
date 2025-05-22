using PV260.Project.Components.ReportComponent.Services;
using Quartz;

namespace PV260.Project.Components.ReportComponent.Jobs;

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
