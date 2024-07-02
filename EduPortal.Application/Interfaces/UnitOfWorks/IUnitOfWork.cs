namespace EduPortal.Application.Interfaces.UnitOfWorks
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
        void Commit();
        Task BeginTransactionAsync();
        Task TransactionCommitAsync();
    }
}
