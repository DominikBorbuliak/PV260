using Microsoft.EntityFrameworkCore.Storage;
using PV260.Project.Domain.Interfaces.Infrastructure.Persistence;

namespace PV260.Project.Infrastructure.Persistence;

public class UnitOfWork : IUnitOfWork
{
    public IReportRepository ReportRepository { get; }
    public IUserRepository UserRepository { get; }

    private readonly AppDbContext _appDbContext;
    private IDbContextTransaction? _dbContextTransaction;

    public UnitOfWork(IReportRepository reportRepository, IUserRepository userRepository, AppDbContext appDbContext)
    {
        ReportRepository = reportRepository;
        UserRepository = userRepository;

        _appDbContext = appDbContext;
    }

    public async Task BeginTransactionAsync()
    {
        _dbContextTransaction = await _appDbContext.Database.BeginTransactionAsync();
    }

    public async Task CommitTransactionAsync()
    {
        if (_dbContextTransaction != null)
        {
            await _dbContextTransaction.CommitAsync();
        }
    }

    public async Task RollbackTransactionAsync()
    {
        if (_dbContextTransaction != null)
        {
            await _dbContextTransaction.RollbackAsync();
        }
    }
}
