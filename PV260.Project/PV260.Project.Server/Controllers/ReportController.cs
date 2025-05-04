using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PV260.Project.Domain.Interfaces.Domain;
using PV260.Project.Domain.Models;
using PV260.Project.Server.Dtos;
using PV260.Project.Server.Mappers;

namespace PV260.Project.Server.Controllers;

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

        return Created();
    }
    
    [Authorize(Roles = "Admin,User")]
    [HttpGet(Name = "ReportDiff")]
    public async Task<ActionResult<IList<HoldingChangeDto>>> ReportDiff(DateTime? date)
    {
        IList<HoldingChange> holdingChanges = await _reportService.GetClosestPreviousReportDiffAsync(date);

        return Ok(holdingChanges.ToDto());
    }
}
