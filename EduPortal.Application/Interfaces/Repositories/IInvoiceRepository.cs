using EduPortal.Domain.Entities;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface IInvoiceRepository : IGenericRepository<Invoice, int>
    {
        /// <summary>
        /// Checks if there are any invoices in the database asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation, returning true if there are invoices, otherwise false.</returns>
        Task<bool> AnyInvoiceAsync();
    }
}
