using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface IQueueRepository : IGenericRepository<Invoice, int>
    {
        /// <summary>
        /// Asynchronously retrieves unpaid invoice IDs and subscriber IDs from the database.
        /// </summary>
        /// <returns>
        /// A collection of tuples containing unpaid invoice IDs and subscriber IDs.
        /// If an error occurs during the operation, returns an empty collection.
        /// </returns>
        /// <remarks>
        /// Note: This method is intended for temporary use and may be moved to a more appropriate location in the future.
        /// </remarks>
        Task<IEnumerable<(int Id, int SubscriberId)>> GetUnpaidInvoices();
    }
}
