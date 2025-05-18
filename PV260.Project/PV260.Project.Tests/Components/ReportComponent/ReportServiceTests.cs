using Moq;
using PV260.Project.Components.ReportsComponent.Services;
using PV260.Project.Domain.Interfaces.ArkFunds;
using PV260.Project.Domain.Interfaces.Email;
using PV260.Project.Domain.Interfaces.Persistence;
using PV260.Project.Domain.Models;
using PV260.Project.Tests.Builders;

namespace PV260.Project.Tests.Components.ReportComponent;

public class ReportServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IArkFundsApiRepository> _apiRepoMock = new();
    private readonly Mock<IEmailSender> _emailSenderMock = new();

    private ReportService CreateService() => new(_unitOfWorkMock.Object, _apiRepoMock.Object, _emailSenderMock.Object);

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldSendEmail_WhenThereAreChanges()
    {
        var oldHolding = new ArkFundsHoldingBuilder().WithShares(50).Build();
        var newHolding = new ArkFundsHoldingBuilder().WithShares(100).Build();
        var report = new ReportBuilder().WithHoldings(oldHolding).Build();

        _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync()).ReturnsAsync(report);
        _unitOfWorkMock.Setup(u => u.ReportRepository.SaveReportAsync(It.IsAny<IList<ArkFundsHolding>>(), It.IsAny<ReportDiff>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.UserRepository.GetSubscribedUserEmailsAsync()).ReturnsAsync(new List<string> { "user@example.com" });
        _unitOfWorkMock.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
        _apiRepoMock.Setup(a => a.GetCurrentHoldingsAsync()).ReturnsAsync(new List<ArkFundsHolding> { newHolding });

        var service = CreateService();

        await service.GenerateAndNotifyAsync();

        _emailSenderMock.Verify(e => e.SendAsync(It.IsAny<EmailConfiguration>()), Times.Once);
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldNotSendEmail_WhenNoSubscribedUsers()
    {
        _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync()).ReturnsAsync((Report)null);
        _unitOfWorkMock.Setup(u => u.ReportRepository.SaveReportAsync(It.IsAny<IList<ArkFundsHolding>>(), It.IsAny<ReportDiff>())).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.UserRepository.GetSubscribedUserEmailsAsync()).ReturnsAsync(new List<string>());
        _unitOfWorkMock.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
        _apiRepoMock.Setup(a => a.GetCurrentHoldingsAsync()).ReturnsAsync(new List<ArkFundsHolding>());

        var service = CreateService();

        await service.GenerateAndNotifyAsync();

        _emailSenderMock.Verify(e => e.SendAsync(It.IsAny<EmailConfiguration>()), Times.Never);
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldRollbackTransaction_OnException()
    {
        _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync()).ThrowsAsync(new Exception("Test failure"));
        _unitOfWorkMock.Setup(u => u.RollbackTransactionAsync()).Returns(Task.CompletedTask);

        var service = CreateService();

        await Assert.ThrowsAsync<Exception>(() => service.GenerateAndNotifyAsync());

        _unitOfWorkMock.Verify(u => u.RollbackTransactionAsync(), Times.Once);
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldDetectAddedHolding()
    {
        var report = new ReportBuilder().WithHoldings().Build();
        var newHolding = new ArkFundsHoldingBuilder().WithTicker("NEW").Build();

        _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync()).ReturnsAsync(report);
        _unitOfWorkMock.Setup(u => u.ReportRepository.SaveReportAsync(It.IsAny<IList<ArkFundsHolding>>(), It.Is<ReportDiff>(d => d.Changes.Any(c => c.ChangeType == ChangeType.Added)))).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.UserRepository.GetSubscribedUserEmailsAsync()).ReturnsAsync(new List<string> { "user@example.com" });
        _unitOfWorkMock.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
        _apiRepoMock.Setup(a => a.GetCurrentHoldingsAsync()).ReturnsAsync(new List<ArkFundsHolding> { newHolding });

        var service = CreateService();

        await service.GenerateAndNotifyAsync();
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldDetectRemovedHolding()
    {
        var oldHolding = new ArkFundsHoldingBuilder().WithTicker("REMOVE").Build();
        var report = new ReportBuilder().WithHoldings(oldHolding).Build();

        _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync()).ReturnsAsync(report);
        _unitOfWorkMock.Setup(u => u.ReportRepository.SaveReportAsync(It.IsAny<IList<ArkFundsHolding>>(), It.Is<ReportDiff>(d => d.Changes.Any(c => c.ChangeType == ChangeType.Removed)))).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.UserRepository.GetSubscribedUserEmailsAsync()).ReturnsAsync(new List<string> { "user@example.com" });
        _unitOfWorkMock.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
        _apiRepoMock.Setup(a => a.GetCurrentHoldingsAsync()).ReturnsAsync(new List<ArkFundsHolding>());

        var service = CreateService();

        await service.GenerateAndNotifyAsync();
    }

    [Fact]
    public async Task GenerateAndNotifyAsync_ShouldDetectNoChanges()
    {
        var holding = new ArkFundsHoldingBuilder().Build();
        var report = new ReportBuilder().WithHoldings(holding).Build();

        _unitOfWorkMock.Setup(u => u.BeginTransactionAsync()).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync()).ReturnsAsync(report);
        _unitOfWorkMock.Setup(u => u.ReportRepository.SaveReportAsync(It.IsAny<IList<ArkFundsHolding>>(), It.Is<ReportDiff>(d => !d.Changes.Any()))).Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.UserRepository.GetSubscribedUserEmailsAsync()).ReturnsAsync(new List<string> { "user@example.com" });
        _unitOfWorkMock.Setup(u => u.CommitTransactionAsync()).Returns(Task.CompletedTask);
        _apiRepoMock.Setup(a => a.GetCurrentHoldingsAsync()).ReturnsAsync(new List<ArkFundsHolding> { holding });

        var service = CreateService();

        await service.GenerateAndNotifyAsync();
    }

    [Fact]
    public async Task GetClosestPreviousReportDiffAsync_ShouldReturnLatest_WhenNoDateGiven()
    {
        var changes = new[] { new HoldingChangeBuilder().Build() };
        var report = new ReportBuilder().WithDiff(changes).Build();
        _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync()).ReturnsAsync(report);

        var service = CreateService();
        var result = await service.GetClosestPreviousReportDiffAsync(null);

        Assert.Equal(changes, result);
    }

    [Fact]
    public async Task GetClosestPreviousReportDiffAsync_ShouldReturnFromDate()
    {
        var date = DateTime.UtcNow;
        var changes = new[] { new HoldingChangeBuilder().Build() };
        var report = new ReportBuilder().WithDiff(changes).Build();
        _unitOfWorkMock.Setup(u => u.ReportRepository.GetClosestPreviousReportAsync(date)).ReturnsAsync(report);

        var service = CreateService();
        var result = await service.GetClosestPreviousReportDiffAsync(date);

        Assert.Equal(changes, result);
    }

    [Fact]
    public async Task GetClosestPreviousReportDiffAsync_ShouldReturnEmpty_WhenNull()
    {
        _unitOfWorkMock.Setup(u => u.ReportRepository.GetLatestReportAsync()).ReturnsAsync((Report)null);

        var service = CreateService();
        var result = await service.GetClosestPreviousReportDiffAsync(null);

        Assert.Empty(result);
    }

    [Fact]
    public void CreateReportDiff_ShouldIgnoreHoldingsWithNullOrWhitespaceTickers()
    {
        var oldReport = new List<ArkFundsHolding>
        {
            new ArkFundsHoldingBuilder().WithTicker(null!).Build(),
            new ArkFundsHoldingBuilder().WithTicker("  ").Build()
        };

        var newReport = new List<ArkFundsHolding>
        {
            new ArkFundsHoldingBuilder().WithTicker("").Build(),
            new ArkFundsHoldingBuilder().WithTicker("VALID").WithShares(10).Build()
        };

        var result = ReportService.CreateReportDiff(oldReport, newReport);

        Assert.Single(result.Changes);
        Assert.Equal("VALID", result.Changes[0].Ticker);
    }

    [Fact]
    public void CreateReportDiff_ShouldSetCorrectOldValues_WhenModified()
    {
        var old = new ArkFundsHoldingBuilder().WithTicker("MOD").WithShares(100).WithWeight(15m).Build();
        var updated = new ArkFundsHoldingBuilder().WithTicker("MOD").WithShares(150).WithWeight(20m).Build();

        var result = ReportService.CreateReportDiff(new List<ArkFundsHolding> { old }, new List<ArkFundsHolding> { updated });

        var change = result.Changes[0];
        Assert.Equal(100, change.OldShares);
        Assert.Equal(150, change.NewShares);
        Assert.Equal(15m, change.OldWeight);
        Assert.Equal(20m, change.NewWeight);
    }

    [Fact]
    public void CreateReportDiff_ShouldHandleEmptyInputs()
    {
        var result = ReportService.CreateReportDiff(new List<ArkFundsHolding>(), new List<ArkFundsHolding>());
        Assert.Empty(result.Changes);
    }

    [Fact]
    public void CreateReportDiff_ShouldIgnoreHoldingsWithEmptyOrNullTickers()
    {
        var oldReport = new List<ArkFundsHolding>
        {
            new ArkFundsHoldingBuilder().WithTicker(null!).Build(),
            new ArkFundsHoldingBuilder().WithTicker(" ").Build()
        };

        var newReport = new List<ArkFundsHolding>
        {
            new ArkFundsHoldingBuilder().WithTicker("").Build(),
            new ArkFundsHoldingBuilder().WithTicker("VALID").WithShares(10).Build()
        };

        var result = ReportService.CreateReportDiff(oldReport, newReport);

        Assert.Single(result.Changes);
        Assert.Equal("VALID", result.Changes[0].Ticker);
    }

    [Fact]
    public void CreateReportDiff_ShouldSetCorrectOldAndNewValues_WhenModified()
    {
        var old = new ArkFundsHoldingBuilder()
            .WithTicker("MOD")
            .WithShares(100)
            .WithWeight(15.5m)
            .Build();

        var updated = new ArkFundsHoldingBuilder()
            .WithTicker("MOD")
            .WithShares(200)
            .WithWeight(20m)
            .Build();

        var result = ReportService.CreateReportDiff(new List<ArkFundsHolding> { old }, new List<ArkFundsHolding> { updated });

        var change = result.Changes.Single();
        Assert.Equal(ChangeType.Modified, change.ChangeType);
        Assert.Equal(100, change.OldShares);
        Assert.Equal(200, change.NewShares);
        Assert.Equal(15.5m, change.OldWeight);
        Assert.Equal(20m, change.NewWeight);
    }

    [Fact]
    public void CreateReportDiff_ShouldHandleCompletelyEmptyReports()
    {
        var result = ReportService.CreateReportDiff([], []);
        Assert.Empty(result.Changes);
    }
}