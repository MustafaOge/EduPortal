using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Microsoft.EntityFrameworkCore;
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
        public async Task<SubscriberInfo> GetSubscriberInfoAsync(int counterNumber)
        {
            var subscriber = await _context.Subscribers.FirstOrDefaultAsync(x => x.CounterNumber == counterNumber.ToString());

            if (subscriber == null)
            {
                return null;
            }

            var subscriberInfo = new SubscriberInfo();

            if (subscriber.SubscriberType == "bireysel")
            {
                subscriberInfo.Individual = await _context.Individuals.FirstOrDefaultAsync(x => x.CounterNumber == counterNumber.ToString());
            }
            else
            {
                subscriberInfo.Corprorate = await _context.Corprorates.FirstOrDefaultAsync(x => x.CounterNumber == counterNumber.ToString());
            }

            return subscriberInfo;
        }



    }
}
