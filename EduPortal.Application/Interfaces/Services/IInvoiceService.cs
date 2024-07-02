using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.MVC.Models.ViewModel;

namespace EduPortal.Application.Interfaces.Services
{
    public interface IInvoiceService
    {
        Task<Response<int>> PayInvoice(int Id);
        Task<Response<SubsIndividual>> PaymentIndividual(string identityNumber);
        Task<Response<SubsCorporate>> PaymentCorporate(string taxIdNumber);
        Task<Response<List<Invoice>>> GetInvoiceDetail(int id);
        Task<Response<InvoiceDetailView>> DetailPay(int id);
        Task<Response<InvoiceComplaint>> CreateComplaint(InvoiceComplaint model);
        Task<Subscriber> GetSubscriberAsync(Invoice invoice);
    }
}
