using Microsoft.EntityFrameworkCore;
using SchoolAPI.Data;

namespace SchoolAPI.Repositories
{
    public interface IUnitOfWork
    {
        Task ExecuteInTransactionAsync(Func<Task> action);
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SchoolDbContext _dbContext;
        public UnitOfWork(SchoolDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task ExecuteInTransactionAsync(Func<Task> action)
        {
            var strategy = _dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                await using var tx = await _dbContext.Database.BeginTransactionAsync();
                try
                {
                    await action();
                    await tx.CommitAsync();
                }
                catch
                {
                    await tx.RollbackAsync();
                    throw;
                }
            });
        }
    }
}
