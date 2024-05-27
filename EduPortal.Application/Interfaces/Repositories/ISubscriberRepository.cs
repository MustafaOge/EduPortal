using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface ISubscriberRepository : IGenericRepository<Subscriber, int>
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

        /// <summary>
        /// Asynchronously finds a subscriber based on the provided counter number, identity number, or tax ID number.
        /// </summary>
        /// <param name="counterOrIdentityOrTaxIdNumber">The counter number, identity number, or tax ID number of the subscriber.</param>
        /// <returns>A task that represents the asynchronous operation and returns the subscriber information.</returns>
        public Task<string> FindSubscriberAsync(string counterOrIdentityOrTaxIdNumber);

        /// <summary>
        /// Asynchronously creates an invoice reminder message for the specified invoice and subscriber.
        /// </summary>
        /// <param name="invoiceId">The ID of the invoice.</param>
        /// <param name="subscriberId">The ID of the subscriber.</param>
        /// <returns>A task that represents the asynchronous operation and returns the invoice reminder message.</returns>
        public Task<string> CreateInvoiceReminderMessage(int invoiceId, int subscriberId);

    }
}
