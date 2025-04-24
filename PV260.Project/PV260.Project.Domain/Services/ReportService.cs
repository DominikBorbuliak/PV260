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

    public ReportService(IReportRepository reportRepository, IArkFundsApiRepository arkRepository, IUserRepository userRepository, IEmailSender emailSender)
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
        IList<ArkFundsHolding> previousHoldings = lastReportEntity?.Holdings ?? new List<ArkFundsHolding>();

        var diff = CreateReportDiff(previousHoldings, currentHoldings);

        await _reportRepository.SaveReportAsync(currentHoldings, diff);

        var subscribedEmails = await _userRepository.GetSubscribedUserEmailsAsync();
        if (!subscribedEmails.Any())
            return;

        string notificationText = BuildChangeSummary(diff);

        var emailConfig = new EmailConfiguration
        {
            Recipients = subscribedEmails,
            Subject = "New ARK Diff Report Available",
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
            if (oldDict.TryGetValue(ticker, out var oldH))
            {
                if (newH.Shares != oldH.Shares)
                {
                    diff.Changes.Add(new HoldingChange
                    {
                        Ticker = ticker,
                        Company = newH.Company,
                        ChangeType = ChangeType.Modified,
                        OldShares = oldH.Shares,
                        NewShares = newH.Shares
                    });
                }
            }
            else
            {
                diff.Changes.Add(new HoldingChange
                {
                    Ticker = ticker,
                    Company = newH.Company,
                    ChangeType = ChangeType.Added,
                    NewShares = newH.Shares
                });
            }
        }

        foreach (var ticker in oldDict.Keys.Except(newDict.Keys))
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
            return "A new diff report has been generated, but no changes were detected.";

        var sb = new StringBuilder();
        sb.AppendLine($"A new diff report has been generated with {diff.Changes.Count} change(s):");
        sb.AppendLine();

        foreach (var change in diff.Changes)
        {
            string line = change.ChangeType switch
            {
                ChangeType.Added => $"{change.Ticker} ({change.Company}) — Added with {change.NewShares} shares.",
                ChangeType.Removed => $"{change.Ticker} ({change.Company}) — Removed (had {change.OldShares} shares).",
                ChangeType.Modified => $"{change.Ticker} ({change.Company}) — Shares changed from {change.OldShares} to {change.NewShares}.",
                _ => $"{change.Ticker} — Unknown change."
            };

            sb.AppendLine(line);
        }

        return sb.ToString();
    }
}
