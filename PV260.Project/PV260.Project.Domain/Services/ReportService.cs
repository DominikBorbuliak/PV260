using MimeKit.Text;
using PV260.Project.Domain.Interfaces.Domain;
using PV260.Project.Domain.Interfaces.Infrastructure.ArkFunds;
using PV260.Project.Domain.Interfaces.Infrastructure.Email;
using PV260.Project.Domain.Interfaces.Infrastructure.Persistence;
using PV260.Project.Domain.Models;
using System.Text;
using PV260.Project.Infrastructure.Persistence.Repositories;

namespace PV260.Project.Domain.Services;

public class ReportService : IReportService
{
    private readonly IReportRepository _reportRepository;
    private readonly IArkFundsApiRepository _arkRepository;
    private readonly IUserRepository _userRepository;
    private readonly IEmailSender _emailSender;

    public ReportService(IReportRepository reportRepository, IArkFundsApiRepository arkRepository,
        IUserRepository userRepository, IEmailSender emailSender)
    {
        _reportRepository = reportRepository;
        _arkRepository = arkRepository;
        _userRepository = userRepository;
        _emailSender = emailSender;
    }

    public async Task GenerateAndNotifyAsync()
    {
        var currentHoldings = await _arkRepository.GetCurrentHoldingsAsync();

        var lastReportEntity = await _reportRepository.GetLatestReportAsync();
        IList<ArkFundsHolding> previousHoldings = lastReportEntity?.Holdings ?? [];

        var diff = CreateReportDiff(previousHoldings, currentHoldings);

        await _reportRepository.SaveReportAsync(currentHoldings, diff);

        var subscribedEmails = await _userRepository.GetSubscribedUserEmailsAsync();
        if (!subscribedEmails.Any())
        {
            return;
        }

        string notificationText = BuildChangeSummary(diff);

        var emailConfig = new EmailConfiguration
        {
            Recipients = subscribedEmails,
            Subject = Constants.Email.Subject,
            Message = notificationText,
            Format = TextFormat.Text
        };

        await _emailSender.SendAsync(emailConfig);
    }

    private ReportDiff CreateReportDiff(IList<ArkFundsHolding> oldReport, IList<ArkFundsHolding> newReport)
    {
        var diff = new ReportDiff();

        var oldDict = oldReport
            .Where(h => !string.IsNullOrWhiteSpace(h.Ticker))
            .ToDictionary(h => h.Ticker);

        var newDict = newReport
            .Where(h => !string.IsNullOrWhiteSpace(h.Ticker))
            .ToDictionary(h => h.Ticker);

        foreach (var (ticker, newH) in newDict)
        {
            oldDict.TryGetValue(ticker, out var oldH);

            if (oldH == null || newH.Shares != oldH.Shares)
            {
                diff.Changes.Add(new HoldingChange
                {
                    Ticker = ticker,
                    Company = newH.Company,
                    ChangeType = oldH == null ? ChangeType.Added : ChangeType.Modified,
                    OldShares = oldH?.Shares ?? 0,
                    NewShares = newH.Shares
                });
            }
        }

        var removedTickers = oldDict.Keys.Except(newDict.Keys);
        foreach (var ticker in removedTickers)
        {
            var oldH = oldDict[ticker];
            diff.Changes.Add(new HoldingChange
            {
                Ticker = ticker,
                Company = oldH.Company,
                ChangeType = ChangeType.Removed,
                OldShares = oldH.Shares
            });
        }

        return diff;
    }

    private string BuildChangeSummary(ReportDiff diff)
    {
        if (!diff.Changes.Any())
        {
            return Constants.Email.NoChanges;
        }

        var sb = new StringBuilder();
        sb.AppendLine(string.Format(Constants.Email.ChangesIntroFormat, diff.Changes.Count));
        sb.AppendLine();

        foreach (var change in diff.Changes)
        {
            string line = change.ChangeType switch
            {
                ChangeType.Added => string.Format(Constants.Email.ChangeAddedFormat, change.Ticker, change.Company,
                    change.NewShares),
                ChangeType.Removed => string.Format(Constants.Email.ChangeRemovedFormat, change.Ticker, change.Company,
                    change.OldShares),
                ChangeType.Modified => string.Format(Constants.Email.ChangeModifiedFormat, change.Ticker,
                    change.Company, change.OldShares, change.NewShares),
                _ => string.Format(Constants.Email.ChangeUnknownFormat, change.Ticker)
            };

            sb.AppendLine(line);
        }

        return sb.ToString();
    }
}
