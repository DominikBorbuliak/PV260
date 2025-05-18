using PV260.Project.Components.ReportsComponent.Mappers;
using PV260.Project.Domain.Models;
using PV260.Project.Tests.Builders;

namespace PV260.Project.Tests.Components.ReportComponent;
public class HoldingChangeDtoMapperTests
{
    [Fact]
    public void ToDto_MapsSingleHoldingChangeCorrectly()
    {
        // Arrange
        var change = new HoldingChangeBuilder()
            .WithType(ChangeType.Modified)
            .WithShares(50, 100)
            .Build();

        // Act
        var dto = change.ToDto();

        // Assert
        Assert.Equal(change.Ticker, dto.Ticker);
        Assert.Equal(change.Company, dto.Company);
        Assert.Equal(change.ChangeType.ToString(), dto.ChangeType);
        Assert.Equal(change.OldShares, dto.OldShares);
        Assert.Equal(change.NewShares, dto.NewShares);
        Assert.Equal(change.OldWeight, dto.OldWeight);
        Assert.Equal(change.NewWeight, dto.NewWeight);
    }

    [Fact]
    public void ToDto_MapsMultipleHoldingChangesCorrectly()
    {
        // Arrange
        var change1 = new HoldingChangeBuilder()
            .WithType(ChangeType.Added)
            .WithShares(0, 100)
            .Build();

        var change2 = new HoldingChangeBuilder()
            .WithType(ChangeType.Removed)
            .WithShares(100, 0)
            .Build();

        var list = new List<HoldingChange> { change1, change2 };

        // Act
        var dtoList = list.ToDto();

        // Assert
        Assert.Equal(2, dtoList.Count);
        Assert.Equal(ChangeType.Added.ToString(), dtoList[0].ChangeType);
        Assert.Equal(ChangeType.Removed.ToString(), dtoList[1].ChangeType);
    }

    [Fact]
    public void ToDto_HandlesEmptyList()
    {
        // Arrange
        var list = new List<HoldingChange>();

        // Act
        var dtoList = list.ToDto();

        // Assert
        Assert.Empty(dtoList);
    }

    [Fact]
    public void ToDto_HandlesNullValuesGracefully()
    {
        // Arrange
        var change = new HoldingChange
        {
            Ticker = null!,
            Company = null!,
            ChangeType = ChangeType.Added,
            OldShares = 0,
            NewShares = 10,
            OldWeight = 0,
            NewWeight = 5.5m
        };

        // Act
        var dto = change.ToDto();

        // Assert
        Assert.Null(dto.Ticker);
        Assert.Null(dto.Company);
        Assert.Equal("Added", dto.ChangeType);
        Assert.Equal(0, dto.OldShares);
        Assert.Equal(10, dto.NewShares);
        Assert.Equal(0, dto.OldWeight);
        Assert.Equal(5.5m, dto.NewWeight);
    }
}
