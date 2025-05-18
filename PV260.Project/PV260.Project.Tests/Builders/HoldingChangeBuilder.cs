using PV260.Project.Domain.Models;

namespace PV260.Project.Tests.Builders;

public class HoldingChangeBuilder
{
    private readonly HoldingChange _change = new()
    {
        Ticker = "ARK",
        Company = "ARK Investments",
        ChangeType = ChangeType.Modified,
        OldShares = 50,
        NewShares = 100,
        OldWeight = 5m,
        NewWeight = 10m
    };

    public HoldingChangeBuilder WithType(ChangeType type)
    {
        _change.ChangeType = type;
        return this;
    }

    public HoldingChangeBuilder WithShares(int oldShares, int newShares)
    {
        _change.OldShares = oldShares;
        _change.NewShares = newShares;
        return this;
    }

    public HoldingChangeBuilder WithWeight(decimal oldWeight, decimal newWeight)
    {
        _change.OldWeight = oldWeight;
        _change.NewWeight = newWeight;
        return this;
    }

    public HoldingChange Build() => _change;
}
