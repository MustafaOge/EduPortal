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
        Task<IEnumerable<(int Id, int SubscriberId)>> GetUnpaidInvoices();
    }
}
