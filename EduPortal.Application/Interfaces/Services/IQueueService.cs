using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;

namespace EduPortal.Application.Interfaces.Services
{
    public interface IQueueService 
    {
        Task<Response<IEnumerable<MailInvoice>>> GetUpcomingPaymentInvoices();
        Task MessageProccesed(OutboxMessage message);
    }
}
