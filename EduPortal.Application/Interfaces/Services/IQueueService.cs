using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Interfaces.Services
{
    public interface IQueueService 
    {

        Task<Response<IEnumerable<MailInvoice>>> GetUpcomingPaymentInvoices();

        Task MessageProccesed(OutboxMessage message);
    }
}
