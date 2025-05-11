namespace PV260.Project.Domain.Interfaces.Infrastructure.Persistence;

public interface IUnitOfWork
{
    IReportRepository ReportRepository { get; }

    IUserRepository UserRepository { get; }

    Task BeginTransactionAsync();

    Task CommitTransactionAsync();

    Task RollbackTransactionAsync();
}
