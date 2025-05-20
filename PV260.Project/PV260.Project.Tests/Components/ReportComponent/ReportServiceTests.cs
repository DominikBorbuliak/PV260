using Moq;
using PV260.Project.Components.ReportComponent.Services;
using PV260.Project.Domain.Interfaces.ArkFunds;
using PV260.Project.Domain.Interfaces.Email;
using PV260.Project.Domain.Interfaces.Persistence;
using PV260.Project.Domain.Models;

namespace PV260.Project.Tests.Components.ReportComponent;

public class ReportServiceTests
{
    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldSendEmail_WhenThereAreChanges()
    {
        var scenario = await new When()
            .WithOldHolding(When.Holding("ARK", 50))
            .WithNewHolding(When.Holding("ARK", 100))
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
            .WithNewHolding(When.Holding("NEW"))
            .WithSubscribedEmails("user@example.com")
            .RunGenerateAndNotify();

        scenario.ThenChangeOfType(ChangeType.Added);
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldDetectRemovedHolding()
    {
        var scenario = await new When()
            .WithOldHolding(When.Holding("REMOVE"))
            .WithSubscribedEmails("user@example.com")
            .RunGenerateAndNotify();

        scenario.ThenChangeOfType(ChangeType.Removed);
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldDetectNoChanges()
    {
        var holding = When.Holding("UNCHANGED", 100, 10.5m);

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
        var changes = new[] { When.Change() };

        var scenario = await new When()
            .WithReportDiff(changes)
            .RunGetClosestPreviousReportDiff(null);

        scenario.ThenDiffShouldMatch(changes);
    }

    [Fact]
    public async Task GetClosestPreviousReportDiffAsync_ShouldReturnFromDate()
    {
        var date = DateTime.UtcNow;
        var changes = new[] { When.Change() };

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
                When.Holding(null!),
                When.Holding("  "))
            .WithNewHolding(
                When.Holding(""),
                When.Holding("VALID", 10))
            .RunCreateDiff()
            .ThenChangesShouldHaveTickers("VALID");
    }

    [Fact]
    public void CreateReportDiff_ShouldSetCorrectOldValues_WhenModified()
    {
        new When()
            .WithOldHolding(When.Holding("MOD", 100, 15m))
            .WithNewHolding(When.Holding("MOD", 150, 20m))
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
                When.Holding(null!),
                When.Holding(" "))
            .WithNewHolding(
                When.Holding(""),
                When.Holding("VALID", 10))
            .RunCreateDiff()
            .ThenChangesShouldHaveTickers("VALID");
    }

    [Fact]
    public void CreateReportDiff_ShouldSetCorrectOldAndNewValues_WhenModified()
    {
        new When()
            .WithOldHolding(When.Holding("MOD", 100, 15.5m))
            .WithNewHolding(When.Holding("MOD", 200, 20m))
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
        private Report? _report;
        private ReportService? _service;
        private Exception? _thrownException;
        private IList<HoldingChange> _resultDiff = [];
        private DateTime? _date;
        private bool _shouldThrowOnReport;
        private ReportDiff _diff = new ReportDiff();

        public When WithOldHolding(params ArkFundsHolding[] holdings)
        {
            _oldHoldings = holdings.ToList();
            _report = new Report
            {
                Holdings = _oldHoldings,
                Diff = []
            };
            return this;
        }

        public When WithNewHolding(params ArkFundsHolding[] holdings)
        {
            _newHoldings = holdings.Any() ? holdings.ToList() : [];
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
            _report = new Report
            {
                Holdings = [],
                Diff = changes.ToList()
            };

            _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync())
                .ReturnsAsync(_report);

            return this;
        }

        public When WithReportDiffFromDate(DateTime date, HoldingChange[] changes)
        {
            _date = date;
            var report = new Report
            {
                Holdings = [],
                Diff = changes.ToList()
            };

            _unitOfWorkMock.Setup(u => u.ReportRepository.GetClosestPreviousReportAsync(date)).ReturnsAsync(report);
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

        public When RunCreateDiff()
        {
            _diff = ReportService.CreateReportDiff(_oldHoldings, _newHoldings);
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

        public static ArkFundsHolding Holding(string ticker, int shares = 100, decimal weight = 10.5m)
        {
            return new ArkFundsHolding
            {
                Ticker = ticker,
                Company = "ARK Investments",
                Shares = shares,
                Weight = weight
            };
        }

        public static HoldingChange Change(
            string ticker = "ARK",
            ChangeType type = ChangeType.Modified,
            int oldShares = 50,
            int newShares = 100,
            decimal oldWeight = 5m,
            decimal newWeight = 10m)
        {
            return new HoldingChange
            {
                Ticker = ticker,
                Company = "ARK Investments",
                ChangeType = type,
                OldShares = oldShares,
                NewShares = newShares,
                OldWeight = oldWeight,
                NewWeight = newWeight
            };
        }
    }
}
