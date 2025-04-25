using MimeKit.Text;
using PV260.Project.Domain.Interfaces.Domain;
using PV260.Project.Domain.Interfaces.Infrastructure.ArkFunds;
using PV260.Project.Domain.Interfaces.Infrastructure.Email;
using PV260.Project.Domain.Interfaces.Infrastructure.Persistence;
using PV260.Project.Domain.Models;
using System.Text;

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
        IList<ArkFundsHolding> currentHoldings = await _arkRepository.GetCurrentHoldingsAsync();

        Report? lastReportEntity = await _reportRepository.GetLatestReportAsync();
        IList<ArkFundsHolding> previousHoldings = lastReportEntity?.Holdings ?? [];

        ReportDiff diff = CreateReportDiff(previousHoldings, currentHoldings);

        await _reportRepository.SaveReportAsync(currentHoldings, diff);

        IList<string> subscribedEmails = await _userRepository.GetSubscribedUserEmailsAsync();
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

    private static ReportDiff CreateReportDiff(IList<ArkFundsHolding> oldReport, IList<ArkFundsHolding> newReport)
    {
        var diff = new ReportDiff();

        var oldDict = oldReport
            .Where(h => !string.IsNullOrWhiteSpace(h.Ticker))
            .ToDictionary(h => h.Ticker);

        var newDict = newReport
            .Where(h => !string.IsNullOrWhiteSpace(h.Ticker))
            .ToDictionary(h => h.Ticker);

        foreach ((string ticker, ArkFundsHolding newH) in newDict)
        {
            _ = oldDict.TryGetValue(ticker, out ArkFundsHolding? oldH);

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

        IEnumerable<string> removedTickers = oldDict.Keys.Except(newDict.Keys);
        foreach (string ticker in removedTickers)
        {
            ArkFundsHolding oldH = oldDict[ticker];
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

    private static string BuildChangeSummary(ReportDiff diff)
    {
        if (!diff.Changes.Any())
        {
            return Constants.Email.NoChanges;
        }

        var sb = new StringBuilder();
        _ = sb.AppendLine(string.Format(Constants.Email.ChangesIntroFormat, diff.Changes.Count));
        _ = sb.AppendLine();

        foreach (HoldingChange change in diff.Changes)
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

            _ = sb.AppendLine(line);
        }

        return sb.ToString();
    }
}
