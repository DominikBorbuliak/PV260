using Moq;
using PV260.Project.Components.ReportComponent.Services;
using PV260.Project.Domain.Interfaces.ArkFunds;
using PV260.Project.Domain.Interfaces.Email;
using PV260.Project.Domain.Interfaces.Persistence;
using PV260.Project.Domain.Models;
using PV260.Project.Tests.Builders;

namespace PV260.Project.Tests.Components.ReportComponent;

public class ReportServiceTests
{
    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldSendEmail_WhenThereAreChanges()
    {
        var scenario = await new When()
            .WithOldHolding(new ArkFundsHoldingBuilder().WithShares(50).Build())
            .WithNewHolding(new ArkFundsHoldingBuilder().WithShares(100).Build())
            .WithSubscribedEmails("user@example.com")
            .RunGenerateAndNotify();

        scenario.ThenEmailShouldBeSent();
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldNotSendEmail_WhenNoSubscribedUsers()
    {
        var scenario = await new When()
            .WithNewHolding()
            .WithSubscribedEmails()
            .RunGenerateAndNotify();

        scenario.ThenEmailShouldNotBeSent();
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldRollbackTransaction_OnException()
    {
        var scenario = await new When()
            .WithFailingReportRepository()
            .RunGenerateAndNotifyExpectingException();

        scenario.ThenTransactionShouldBeRolledBack();
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldDetectAddedHolding()
    {
        var scenario = await new When()
            .WithNewHolding(new ArkFundsHoldingBuilder().WithTicker("NEW").Build())
            .WithSubscribedEmails("user@example.com")
            .RunGenerateAndNotify();

        scenario.ThenChangeOfType(ChangeType.Added);
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldDetectRemovedHolding()
    {
        var scenario = await new When()
            .WithOldHolding(new ArkFundsHoldingBuilder().WithTicker("REMOVE").Build())
            .WithSubscribedEmails("user@example.com")
            .RunGenerateAndNotify();

        scenario.ThenChangeOfType(ChangeType.Removed);
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldDetectNoChanges()
    {
        var holding = new ArkFundsHoldingBuilder().Build();

        var scenario = await new When()
            .WithOldHolding(holding)
            .WithNewHolding(holding)
            .WithSubscribedEmails("user@example.com")
            .RunGenerateAndNotify();

        scenario.ThenNoChanges();
    }

    [Fact]
    public async Task GetClosestPreviousReportDiffAsync_ShouldReturnLatest_WhenNoDateGiven()
    {
        var changes = new[] { new HoldingChangeBuilder().Build() };

        var scenario = await new When()
            .WithReportDiff(changes)
            .RunGetClosestPreviousReportDiff(null);

        scenario.ThenDiffShouldMatch(changes);
    }

    [Fact]
    public async Task GetClosestPreviousReportDiffAsync_ShouldReturnFromDate()
    {
        var date = DateTime.UtcNow;
        var changes = new[] { new HoldingChangeBuilder().Build() };

        var scenario = await new When()
            .WithReportDiffFromDate(date, changes)
            .RunGetClosestPreviousReportDiff(date);

        scenario.ThenDiffShouldMatch(changes);
    }

    [Fact]
    public async Task GetClosestPreviousReportDiffAsync_ShouldReturnEmpty_WhenNull()
    {
        var scenario = await new When()
            .WithNullReport()
            .RunGetClosestPreviousReportDiff(null);

        scenario.ThenDiffShouldBeEmpty();
    }

    [Fact]
    public void CreateReportDiff_ShouldIgnoreHoldingsWithNullOrWhitespaceTickers()
    {
        new When()
            .WithOldHolding(
                new ArkFundsHoldingBuilder().WithTicker(null!).Build(),
                new ArkFundsHoldingBuilder().WithTicker("  ").Build())
            .WithNewHolding(
                new ArkFundsHoldingBuilder().WithTicker("").Build(),
                new ArkFundsHoldingBuilder().WithTicker("VALID").WithShares(10).Build())
            .RunCreateDiff()
            .ThenChangesShouldHaveTickers("VALID");
    }

    [Fact]
    public void CreateReportDiff_ShouldSetCorrectOldValues_WhenModified()
    {
        var old = new ArkFundsHoldingBuilder().WithTicker("MOD").WithShares(100).WithWeight(15m).Build();
        var updated = new ArkFundsHoldingBuilder().WithTicker("MOD").WithShares(150).WithWeight(20m).Build();

        new When()
            .WithOldHolding(old)
            .WithNewHolding(updated)
            .RunCreateDiff()
            .ThenShouldContainChange("MOD", ChangeType.Modified, 100, 150, 15m, 20m);
    }

    [Fact]
    public void CreateReportDiff_ShouldHandleEmptyInputs()
    {
        new When()
            .WithOldHolding()
            .WithNewHolding()
            .RunCreateDiff()
            .ThenShouldHaveNoChanges();
    }

    [Fact]
    public void CreateReportDiff_ShouldIgnoreHoldingsWithEmptyOrNullTickers()
    {
        new When()
            .WithOldHolding(
                new ArkFundsHoldingBuilder().WithTicker(null!).Build(),
                new ArkFundsHoldingBuilder().WithTicker(" ").Build())
            .WithNewHolding(
                new ArkFundsHoldingBuilder().WithTicker("").Build(),
                new ArkFundsHoldingBuilder().WithTicker("VALID").WithShares(10).Build())
            .RunCreateDiff()
            .ThenChangesShouldHaveTickers("VALID");
    }

    [Fact]
    public void CreateReportDiff_ShouldSetCorrectOldAndNewValues_WhenModified()
    {
        var old = new ArkFundsHoldingBuilder().WithTicker("MOD").WithShares(100).WithWeight(15.5m).Build();
        var updated = new ArkFundsHoldingBuilder().WithTicker("MOD").WithShares(200).WithWeight(20m).Build();

        new When()
            .WithOldHolding(old)
            .WithNewHolding(updated)
            .RunCreateDiff()
            .ThenShouldContainChange("MOD", ChangeType.Modified, 100, 200, 15.5m, 20m);
    }

    [Fact]
    public void CreateReportDiff_ShouldHandleCompletelyEmptyReports()
    {
        new When()
            .WithOldHolding()
            .WithNewHolding()
            .RunCreateDiff()
            .ThenShouldHaveNoChanges();
    }

    private sealed class When
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<IArkFundsApiRepository> _apiRepoMock = new();
        private readonly Mock<IEmailSender> _emailSenderMock = new();
        private IList<ArkFundsHolding> _oldHoldings = new List<ArkFundsHolding>();
        private IList<ArkFundsHolding> _newHoldings = new List<ArkFundsHolding>();
        private IList<string> _emails = new List<string>();
        private Report _report;
        private ReportService _service;
        private Exception _thrownException;
        private IList<HoldingChange> _resultDiff;
        private DateTime? _date;
        private bool _shouldThrowOnReport;
        private ReportDiff _diff;

        public When WithOldHolding(params ArkFundsHolding[] holdings)
        {
            _oldHoldings = holdings.ToList();
            _report = new ReportBuilder().WithHoldings(_oldHoldings.ToArray()).Build();
            return this;
        }

        public When WithNewHolding(params ArkFundsHolding[] holdings)
        {
            _newHoldings = holdings.Any() ? holdings.ToList() : new List<ArkFundsHolding>();
            return this;
        }

        public When WithSubscribedEmails(params string[] emails)
        {
            _emails = emails;
            return this;
        }

        public When WithFailingReportRepository()
        {
            _shouldThrowOnReport = true;
            return this;
        }

        public When WithReportDiff(HoldingChange[] changes)
        {
            _report = new ReportBuilder().WithDiff(changes).Build();

            _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync())
                .ReturnsAsync(_report);

            return this;
        }

        public When WithReportDiffFromDate(DateTime date, HoldingChange[] changes)
        {
            _date = date;
            _unitOfWorkMock.Setup(u => u.ReportRepository.GetClosestPreviousReportAsync(date)).ReturnsAsync(
                new ReportBuilder().WithDiff(changes).Build());
            return this;
        }

        public When WithNullReport()
        {
            _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync())
                .ReturnsAsync((Report)null!);
            return this;
        }

        public async Task<When> RunGenerateAndNotify()
        {
            SetupMocks();
            _service = new ReportService(_unitOfWorkMock.Object, _apiRepoMock.Object, _emailSenderMock.Object);
            await _service.GenerateAndNotifyAsync();
            return this;
        }

        public async Task<When> RunGenerateAndNotifyExpectingException()
        {
            SetupMocks();
            _service = new ReportService(_unitOfWorkMock.Object, _apiRepoMock.Object, _emailSenderMock.Object);
            try
            {
                await _service.GenerateAndNotifyAsync();
            }
            catch (Exception ex)
            {
                _thrownException = ex;
            }
            return this;
        }

        public async Task<When> RunGetClosestPreviousReportDiff(DateTime? date)
        {
            _service = new ReportService(_unitOfWorkMock.Object, _apiRepoMock.Object, _emailSenderMock.Object);
            _resultDiff = (await _service.GetClosestPreviousReportDiffAsync(date)).ToList();
            return this;
        }

        public When ThenEmailShouldBeSent()
        {
            _emailSenderMock.Verify(e => e.SendAsync(It.IsAny<EmailConfiguration>()), Times.Once);
            return this;
        }

        public When ThenEmailShouldNotBeSent()
        {
            _emailSenderMock.Verify(e => e.SendAsync(It.IsAny<EmailConfiguration>()), Times.Never);
            return this;
        }

        public When ThenTransactionShouldBeRolledBack()
        {
            Assert.NotNull(_thrownException);
            _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
            return this;
        }

        public When ThenChangeOfType(ChangeType changeType)
        {
            _unitOfWorkMock.Verify(u =>
                u.ReportRepository.SaveReportAsync(It.IsAny<IList<ArkFundsHolding>>(),
                It.Is<ReportDiff>(d => d.Changes.Any(c => c.ChangeType == changeType))), Times.Once);
            return this;
        }

        public When ThenNoChanges()
        {
            _unitOfWorkMock.Verify(u =>
                u.ReportRepository.SaveReportAsync(It.IsAny<IList<ArkFundsHolding>>(),
                It.Is<ReportDiff>(d => !d.Changes.Any())), Times.Once);
            return this;
        }

        public When ThenDiffShouldMatch(IEnumerable<HoldingChange> expected)
        {
            Assert.Equal(expected, _resultDiff);
            return this;
        }

        public When ThenDiffShouldBeEmpty()
        {
            Assert.Empty(_resultDiff);
            return this;
        }

        public When ThenShouldHaveNoChanges()
        {
            Assert.Empty(_diff.Changes);
            return this;
        }

        public When ThenChangesShouldHaveTickers(params string[] expectedTickers)
        {
            var actualTickers = _diff.Changes.Select(c => c.Ticker).ToList();
            Assert.Equal(expectedTickers, actualTickers);
            return this;
        }

        public When ThenShouldContainChange(string ticker, ChangeType type, int oldShares, int newShares, decimal oldWeight, decimal newWeight)
        {
            var change = _diff.Changes.Single(c => c.Ticker == ticker);
            Assert.Equal(type, change.ChangeType);
            Assert.Equal(oldShares, change.OldShares);
            Assert.Equal(newShares, change.NewShares);
            Assert.Equal(oldWeight, change.OldWeight);
            Assert.Equal(newWeight, change.NewWeight);
            return this;
        }
        public When RunCreateDiff()
        {
            _diff = ReportService.CreateReportDiff(_oldHoldings, _newHoldings);
            return this;
        }

        private void SetupMocks()
        {
            _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.RollbackTransactionAsync()).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.UserRepository.GetSubscribedUserEmailsAsync()).ReturnsAsync(_emails);
            _unitOfWorkMock.Setup(u => u.ReportRepository.SaveReportAsync(It.IsAny<IList<ArkFundsHolding>>(), It.IsAny<ReportDiff>())).Returns(Task.CompletedTask);

            if (_shouldThrowOnReport)
            {
                _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync()).ThrowsAsync(new Exception("Test failure"));
            }
            else if (_date == null)
            {
                _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync()).ReturnsAsync(_report);
            }

            _apiRepoMock.Setup(a => a.GetCurrentHoldingsAsync()).ReturnsAsync(_newHoldings);
        }
    }
}
