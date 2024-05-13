using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface ISubscriberRepository :  IGenericRepository<Subscriber, int> 
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> HasUnpaidInvoices(int id);

        /// <summary>
        /// Retrieves subscriber information asynchronously based on the provided counter number.
        /// </summary>
        /// <param name="counterNumber">The counter number of the subscriber.</param>
        /// <returns>An object representing the subscriber information. Returns null if the subscriber is not found.</returns>
        Task<SubscriberInfo> GetSubscriberInfoAsync(int counterNumber);

        Task<string> FindSubscriberAsync(string counterOrIdentityOrTaxIdNumber);

        Task<string> CreateInvoiceReminderMessage(int invoiceId, int subscriberId);


    }
}
