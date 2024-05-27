using Castle.Core.Logging;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Hangfire.Logging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Persistence.Repositories
{
    public class QueueRepository : GenericRepository<Invoice, int>, IQueueRepository
    {
        private readonly AppDbContext _context;
        public QueueRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<(int Id, int SubscriberId)>> GetUnpaidInvoices()
        {
            var currentDate = DateTime.Today;
            var dueDateThreshold = currentDate.AddDays(-30);

            try
            {
                var unpaidInvoiceData = await _context.Invoices
                    .Where(x => !x.IsPaid && x.DueDate >= dueDateThreshold)
                    .Select(x => new { Id = x.Id, SubscriberId = x.SubscriberId })
                    .ToListAsync();

                return unpaidInvoiceData.Select(x => (x.Id, x.SubscriberId));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"bİR HATA OLUŞTU(GetUnpaidInvoices) : {ex.Message}");

                // Hata oluştuğunda bir sonuç döndürün veya null döndürün, işleyen kod buna göre işlem yapabilir
                return Enumerable.Empty<(int Id, int SubscriberId)>();
            }
        }



    }
}
