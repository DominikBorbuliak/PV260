using PV260.Project.Components.ReportComponent.DTOs;
using PV260.Project.Components.ReportComponent.Mappers;
using PV260.Project.Domain.Models;

namespace PV260.Project.Tests.Components.ReportComponent;

public class HoldingChangeDtoMapperTests
{
    [Fact]
    public void ToDto_MapsSingleHoldingChangeCorrectly()
    {
        _ = new When()
            .WithHoldingChange(ChangeType.Modified, 50, 100)
            .ThenMapToDto()
            .ShouldMatchOriginal();
    }

    [Fact]
    public void ToDto_MapsMultipleHoldingChangesCorrectly()
    {
        _ = new When()
            .WithMultipleChanges()
            .ThenMapListToDto()
            .ShouldHaveMappedChangeTypes(ChangeType.Added.ToString(), ChangeType.Removed.ToString());
    }

    [Fact]
    public void ToDto_HandlesEmptyList()
    {
        _ = new When()
            .WithEmptyList()
            .ThenMapListToDto()
            .ShouldBeEmpty();
    }

    [Fact]
    public void ToDto_HandlesNullValuesGracefully()
    {
        _ = new When()
            .WithNullValues()
            .ThenMapToDto()
            .ShouldHandleNulls();
    }

    private sealed class When
    {
        private HoldingChange _change;
        private IList<HoldingChange> _list;
        private HoldingChangeDto _dto;
        private IList<HoldingChangeDto> _dtoList;

        public When WithHoldingChange(ChangeType type, int oldShares, int newShares)
        {
            _change = HoldingChange("ARKK", "ARK Invest", type, oldShares, newShares, 10, 20);
            return this;
        }

        public When WithMultipleChanges()
        {
            _list = new List<HoldingChange>
            {
                HoldingChange("TSLA", "Tesla", ChangeType.Added, 0, 100, 0, 5),
                HoldingChange("ZM", "Zoom", ChangeType.Removed, 100, 0, 4.5m, 0)
            };
            return this;
        }

        public When WithEmptyList()
        {
            _list = new List<HoldingChange>();
            return this;
        }

        public When WithNullValues()
        {
            _change = new HoldingChange
            {
                Ticker = null!,
                Company = null!,
                ChangeType = ChangeType.Added,
                OldShares = 0,
                NewShares = 10,
                OldWeight = 0,
                NewWeight = 5.5m
            };
            return this;
        }

        public When ThenMapToDto()
        {
            _dto = _change.ToDto();
            return this;
        }

        public When ThenMapListToDto()
        {
            _dtoList = _list.ToDto();
            return this;
        }

        public When ShouldMatchOriginal()
        {
            Assert.Equal(_change.Ticker, _dto.Ticker);
            Assert.Equal(_change.Company, _dto.Company);
            Assert.Equal(_change.ChangeType.ToString(), _dto.ChangeType);
            Assert.Equal(_change.OldShares, _dto.OldShares);
            Assert.Equal(_change.NewShares, _dto.NewShares);
            Assert.Equal(_change.OldWeight, _dto.OldWeight);
            Assert.Equal(_change.NewWeight, _dto.NewWeight);
            return this;
        }

        public When ShouldHaveMappedChangeTypes(string expected1, string expected2)
        {
            Assert.Equal(2, _dtoList.Count);
            Assert.Equal(expected1, _dtoList[0].ChangeType);
            Assert.Equal(expected2, _dtoList[1].ChangeType);
            return this;
        }

        public When ShouldBeEmpty()
        {
            Assert.Empty(_dtoList);
            return this;
        }

        public When ShouldHandleNulls()
        {
            Assert.Null(_dto.Ticker);
            Assert.Null(_dto.Company);
            Assert.Equal("Added", _dto.ChangeType);
            Assert.Equal(0, _dto.OldShares);
            Assert.Equal(10, _dto.NewShares);
            Assert.Equal(0, _dto.OldWeight);
            Assert.Equal(5.5m, _dto.NewWeight);
            return this;
        }

        private static HoldingChange HoldingChange(
            string? ticker,
            string? company,
            ChangeType type,
            int oldShares,
            int newShares,
            decimal oldWeight,
            decimal newWeight)
        {
            return new HoldingChange
            {
                Ticker = ticker,
                Company = company,
                ChangeType = type,
                OldShares = oldShares,
                NewShares = newShares,
                OldWeight = oldWeight,
                NewWeight = newWeight
            };
        }
    }
}
