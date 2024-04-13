using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Repositories
{
    public class SubscriberRepository : GenericRepository<Subscriber, int>, ISubscriberRepository
    {
        private readonly AppDbContext _context;
        public SubscriberRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> HasUnpaidInvoices(int subscriberId)
        {
            return await Task.FromResult(_context.Invoices.Any(i => i.SubscriberId == subscriberId && !i.IsPaid));
        }

    }
}
