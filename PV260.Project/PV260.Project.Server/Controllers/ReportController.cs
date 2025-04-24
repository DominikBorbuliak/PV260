using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PV260.Project.Domain.Interfaces.Domain;

namespace PV260.Project.Server.Controllers;

[Authorize(Roles = "Admin")]
public class ReportController : ApiController
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [HttpPost(Name = "GenerateReport")]
    public async Task<IActionResult> GenerateDiffReport()
    {
        await _reportService.GenerateAndNotifyAsync();
        return Ok(new { message = "Diff report generated and notifications sent." });
    }
}
