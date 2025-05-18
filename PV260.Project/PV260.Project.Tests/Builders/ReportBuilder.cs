using PV260.Project.Domain.Models;

namespace PV260.Project.Tests.Builders;
public class ReportBuilder
{
    private readonly Report _report = new()
    {
        Holdings = new List<ArkFundsHolding>(),
        Diff = new List<HoldingChange>()
    };

    public ReportBuilder WithHoldings(params ArkFundsHolding[] holdings)
    {
        _report.Holdings = holdings.ToList();
        return this;
    }

    public ReportBuilder WithDiff(params HoldingChange[] changes)
    {
        _report.Diff = changes.ToList();
        return this;
    }

    public Report Build() => _report;
}

