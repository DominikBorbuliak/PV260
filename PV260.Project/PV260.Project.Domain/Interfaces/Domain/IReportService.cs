namespace PV260.Project.Domain.Interfaces.Domain;

public interface IReportService
{
    Task GenerateAndNotifyAsync();
}
