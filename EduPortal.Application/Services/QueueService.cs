using AutoMapper;
using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Services
{ 
    public class QueueService(IGenericRepository<OutboxMessage, int> outboxMessageRepository, IQueueRepository queueRepository,IUnitOfWork unitOfWork, ISubscriberRepository subscriberRepository, IInvoiceRepository invoiceRepository) : IQueueService
    {
        public async Task<Response<IEnumerable<MailInvoice>>> GetUpcomingPaymentInvoices()
        {
            IEnumerable<(int Id, int SubscriberId)> invoices = await queueRepository.GetUnpaidInvoices();
            IEnumerable<Subscriber> subscribers = await subscriberRepository.GetAllAsync(x => x.IsActive);

            List<MailInvoice> mailInvoices = new List<MailInvoice>();
            foreach (var invoice in invoices)
            {
                // Her fatura için uygun aboneyi bulun
                var subscriber = subscribers.FirstOrDefault(s => s.Id == invoice.SubscriberId && s.Id < 4750);
                if (subscriber != null)
                {
                    mailInvoices.Add(new MailInvoice { InvoiceId = invoice.Id, SubscriberId = invoice.SubscriberId });
                }
            }

            // Eğer faturalar bulunamazsa, hata döndürün
            if (mailInvoices.Count == 0)
            {
                return Response<IEnumerable<MailInvoice>>.Fail("No upcoming payment invoices found", HttpStatusCode.NotFound);
            }

            // Faturalar başarıyla bulunduysa, başarılı yanıtı döndürün
            return Response<IEnumerable<MailInvoice>>.Success(mailInvoices, HttpStatusCode.OK);
        }

        public Task MessageProccesed(OutboxMessage message)
        {
            outboxMessageRepository.Update(message);
            return unitOfWork.CommitAsync();
            
        }
    }
}
