using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PV260.Project.Components.Common.Controllers;
using PV260.Project.Components.ReportsComponent.DTOs;
using PV260.Project.Components.ReportsComponent.Mappers;
using PV260.Project.Components.ReportsComponent.Services;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.ReportsComponent.Controllers;

[Authorize]
public class ReportController : ApiController
{
    private readonly IReportService _reportService;

    public ReportController(IReportService reportService)
    {
        _reportService = reportService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost(Name = "GenerateReport")]
    public async Task<ActionResult> GenerateDiffReport()
    {
        await _reportService.GenerateAndNotifyAsync();

        return Ok();
    }

    [HttpGet(Name = "ReportDiff")]
    public async Task<ActionResult<IList<HoldingChangeDto>>> ReportDiff(DateTime? date)
    {
        IList<HoldingChange> holdingChanges = await _reportService.GetClosestPreviousReportDiffAsync(date);

        return Ok(holdingChanges.ToDto());
    }
}
