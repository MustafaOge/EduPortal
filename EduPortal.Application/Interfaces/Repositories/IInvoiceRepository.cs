using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

