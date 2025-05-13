using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PV260.Project.Components.Common.Controllers;
using PV260.Project.Components.ReportsComponent.DTOs;
using PV260.Project.Components.ReportsComponent.Mappers;
using PV260.Project.Domain.Models;

namespace PV260.Project.Components.ReportComponent.Controllers;

[Authorize]
public class ReportController : ApiController
{
    private readonly IReportComponent _reportComponent;

    public ReportController(IReportComponent reportComponent)
    {
        _reportComponent = reportComponent;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost(Name = "GenerateReport")]
    public async Task<ActionResult> GenerateDiffReport()
    {
        await _reportComponent.GenerateAndNotifyAsync();

        return Ok();
    }

    [HttpGet(Name = "ReportDiff")]
    public async Task<ActionResult<IList<HoldingChangeDto>>> ReportDiff(DateTime? date)
    {
        IList<HoldingChange> holdingChanges = await _reportComponent.GetClosestPreviousReportDiffAsync(date);

        return Ok(holdingChanges.ToDto());
    }
}
