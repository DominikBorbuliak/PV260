using PV260.Project.Domain.Interfaces.Domain;
using Quartz;

namespace PV260.Project.Server.Jobs;

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
