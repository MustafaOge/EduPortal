using EduPortal.Domain.Entities;

namespace EduPortal.Application.Interfaces.Repositories
{
    public interface ISubscriberRepository : IGenericRepository<Subscriber, int>
    {
        /// <summary>
        /// Asynchronously checks if the subscriber with the given ID has unpaid invoices.
        /// </summary>
        /// <param name="id">The ID of the subscriber.</param>
        /// <returns>A task that represents the asynchronous operation, containing a boolean value indicating whether the subscriber has unpaid invoices.</returns>
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

        /// <summary>
        /// Asynchronously retrieves a list of subscribers based on the given counter number.
        /// </summary>
        /// <param name="counterNumber">The counter number of the subscribers.</param>
        /// <returns>A task that represents the asynchronous operation, containing a list of subscribers associated with the counter number.</returns>
        Task<List<Subscriber>> GetSubscribersByCounterNumberAsync(string counterNumber);

    }
}
