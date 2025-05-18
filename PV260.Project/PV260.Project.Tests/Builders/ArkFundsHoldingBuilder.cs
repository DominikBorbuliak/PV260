using PV260.Project.Domain.Models;

namespace PV260.Project.Tests.Builders;
public class ArkFundsHoldingBuilder
{
    private readonly ArkFundsHolding _holding = new()
    {
        Ticker = "ARK",
        Company = "ARK Investments",
        Shares = 100,
        Weight = 10.5m
    };

    public ArkFundsHoldingBuilder WithTicker(string ticker)
    {
        _holding.Ticker = ticker;
        return this;
    }

    public ArkFundsHoldingBuilder WithShares(int shares)
    {
        _holding.Shares = shares;
        return this;
    }

    public ArkFundsHoldingBuilder WithWeight(decimal weight)
    {
        _holding.Weight = weight;
        return this;
    }

    public ArkFundsHolding Build() => _holding;
}