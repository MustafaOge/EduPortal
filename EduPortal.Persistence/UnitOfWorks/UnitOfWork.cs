
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore.Storage;

namespace CreditWiseHub.Repository.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction _transaction;
        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            if(!TestServerOptions.TestServer)
                _transaction = await _context.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task TransactionCommitAsync()
        {
            if (!TestServerOptions.TestServer)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    await _transaction.CommitAsync();
                }
                catch (Exception)
                {
                    await _transaction.RollbackAsync();
                    throw;
                }
                finally
                {
                    _transaction.Dispose();
                }
            }
        }
    }
}
//TO-DO
public static class TestServerOptions
{
    public static bool TestServer = false;
}