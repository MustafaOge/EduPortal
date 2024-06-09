

using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Application.Messaging;
using EduPortal.Domain.Entities;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EduPortal.Application.HangfireJobs.Managers.ReccurringJobs
{
    public class InvoiceReminderJob
    {
        private readonly IQueueService _queueService;
        private readonly IGenericRepository<OutboxMessage, int> _outboxMessageRepository;
        private readonly PublisherServiceMassTransit _publisherServiceMassTransit;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceReminderJob(
            IQueueService queueService,
            IUnitOfWork unitOfWork,
            IGenericRepository<OutboxMessage, int> outboxMessageRepository,
            PublisherServiceMassTransit publisherServiceMassTransit)
        {
            _unitOfWork = unitOfWork;
            _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
            _outboxMessageRepository = outboxMessageRepository ?? throw new ArgumentNullException(nameof(outboxMessageRepository));
            _publisherServiceMassTransit = publisherServiceMassTransit ?? throw new ArgumentNullException(nameof(publisherServiceMassTransit));
        }

        public async Task RunPaymentReminderJob()
        {
            try
            {
                var response = await _queueService.GetUpcomingPaymentInvoices();

                if (response.StatusCode == HttpStatusCode.OK && response.Data?.Any() == true)
                {
                    foreach (var invoice in response.Data)
                    {
                        await ProcessInvoice(invoice);
                    }
                }
                else
                {
                    Console.WriteLine("Failed to retrieve upcoming payment invoices.");
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Log any errors that occur
                Log.Warning($"An error occurred in RunPaymentReminderJob: {ex.Message}. A new error occurred - Class: RunPaymentReminderJob");
            }
        }


        private async Task ProcessInvoice(MailInvoice invoice)
        {
            var payload = $"{invoice.InvoiceId}-{invoice.SubscriberId}";
            var existingRecord = await _outboxMessageRepository.GetAllAsync(x => x.Payload == payload);

            if (!existingRecord.Any())
            {
                var outboxMessage = new OutboxMessage
                {
                    Type = "Subscription-Termination",
                    Payload = payload,
                    CreatedByUser = 10,
                    IsProcessed = false
                };

                await _outboxMessageRepository.AddAsync(outboxMessage);
                await SendMessage(outboxMessage); // Send message after adding
                await MarkMessageAsProcessed(outboxMessage); // Mark message as processed
            }
            else
            {
                Console.WriteLine($"A record with the same payload already exists: {payload}");
            }
        }

        private async Task SendMessage(OutboxMessage message)
        {
            await _publisherServiceMassTransit.SendMessageAsync(message.Payload, "payment2.order.created.event");
        }

        private async Task MarkMessageAsProcessed(OutboxMessage message)
        {
            message.IsProcessed = true;
            _outboxMessageRepository.Update(message);
            await _unitOfWork.CommitAsync(); // Assuming _unitOfWork is available and has a CommitAsync method
        }
    }
}




//using EduPortal.Application.Interfaces.Repositories;
//using EduPortal.Application.Interfaces.Services;
//using EduPortal.Application.Interfaces.UnitOfWorks;
//using EduPortal.Application.Messaging;
//using EduPortal.Application.Services;
//using EduPortal.Core.Responses;
//using EduPortal.Domain.Entities;
//using EduPortal.Persistence.Services;
//using Microsoft.AspNetCore.Mvc;
//using RabbitMQ.Client;
//using Serilog;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Reflection;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace EduPortal.Application.HangfireJobs.Managers.ReccurringJobs
//{
//    public class InvoiceReminderJob(
//        IQueueService _queueService,
//        IUnitOfWork _unitOfWork,
//        IGenericRepository<OutboxMessage, int> _outboxMessageRepository,
//        //RabbitMQPublisherService _rabbitMQPublisherService,
//        PublisherServiceMassTransit _publisherServiceMassTransit
//    //RabbitMQConsumerService _rabbitMQConsumerService
//        )
//    {
//        public async Task RunPaymentReminderJob()
//        {
//            try
//            {
//                Response<IEnumerable<MailInvoice>> response = await _queueService.GetUpcomingPaymentInvoices();


//                if (response.StatusCode == HttpStatusCode.OK)
//                {
//                    // response.Data değerini kontrol et
//                    if (response.Data != null && response.Data.Any())
//                    {
//                        foreach (var item in response.Data)
//                        {
//                            await ProcessInvoice(item);
//                        }
//                    }
//                    else
//                    {
//                        Console.WriteLine("No upcoming payment invoices retrieved.");
//                    }

//                    var result = await _outboxMessageRepository.GetAllAsync(x => x.IsProcessed == false);

//                    // result değerini kontrol et
//                    if (result != null && result.Any())
//                    {
//                        foreach (var item in result)
//                        {
//                            await _publisherServiceMassTransit.SendMessageAsync(item.Payload, "payment2.order.created.event");
//                            await ProccesSendMessageProcces(item);
//                        }
//                    }
//                    else
//                    {
//                        Console.WriteLine("No unprocessed outbox messages found.");
//                    }

//                    //await _rabbitMQPublisherService.StartPublishing();
//                    ////_rabbitMQConsumerService.StartConsuming();
//                }
//                else
//                {
//                    // Error occurred while retrieving upcoming payment invoices
//                    Console.WriteLine("Failed to retrieve upcoming payment invoices.");
//                    Console.WriteLine($"Error: {response.StatusCode}");
//                }
//            }
//            catch (Exception ex)
//            {
//                // Log any errors that occur
//                Log.Warning($"An error occurred in RunPaymentReminderJob: {ex.Message}. A new error occurred - Class: RunPaymentReminderJob");
//            }
//        }
//        private async Task ProccesSendMessageProcces(OutboxMessage outboxMessage)
//        {
//            var result = _outboxMessageRepository.Where(x => x.Id == outboxMessage.Id);
//            foreach (var item in result)
//            {
//                item.IsProcessed = true;
//                _outboxMessageRepository.Update(item);
//            }
//            _unitOfWork.Commit();

//        }


//        private async Task ProcessInvoice(MailInvoice item)
//        {
//            var existingRecord = await _outboxMessageRepository.GetAllAsync(x => x.Payload == $"{item.InvoiceId}-{item.SubscriberId}");

//            if (existingRecord.Count() == 0)
//            {
//                var outboxMessage = new OutboxMessage
//                {
//                    Type = "Subscription-Termination",
//                    Payload = $"{item.InvoiceId}-{item.SubscriberId}",
//                    CreatedByUser = 10,
//                    IsProcessed = false
//                };

//                await _outboxMessageRepository.AddAsync(outboxMessage);
//            }
//            else
//            {
//                Console.WriteLine($"A record with the same payload already exists: {item.InvoiceId}-{item.SubscriberId}");
//            }
//        }

//    }
//}



//using EduPortal.Application.Interfaces.Repositories;
//using EduPortal.Application.Interfaces.Services;
//using EduPortal.Application.Messaging;
//using EduPortal.Application.Services;
//using EduPortal.Core.Responses;
//using EduPortal.Domain.Entities;
//using EduPortal.Persistence.Services;
//using Microsoft.AspNetCore.Mvc;
//using RabbitMQ.Client;
//using Serilog;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Reflection;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace EduPortal.Application.HangfireJobs.Managers.ReccurringJobs
//{
//    public class InvoiceReminderJob(
//        IQueueService _queueService,
//        IGenericRepository<OutboxMessage, int> _outboxMessageRepository,
//        RabbitMQPublisherService _rabbitMQPublisherService,
//        RabbitMQConsumerService _rabbitMQConsumerService)
//    {
//        public async Task RunPaymentReminderJob()
//        {
//            try
//            {
//                Response<IEnumerable<MailInvoice>> response = await _queueService.GetUpcomingPaymentInvoices();

//                if (response.StatusCode == HttpStatusCode.OK)
//                {
//                    foreach (var item in response.Data)
//                    {
//                        await ProcessInvoice(item);
//                    }

//                    await _rabbitMQPublisherService.StartPublishing();
//                    _rabbitMQConsumerService.StartConsuming();
//                }
//                else
//                {
//                    // Error occurred while retrieving upcoming payment invoices
//                    Console.WriteLine("Failed to retrieve upcoming payment invoices.");
//                    Console.WriteLine($"Error: {response.StatusCode}");
//                }
//            }
//            catch (Exception ex)
//            {
//                // Log any errors that occur
//                Log.Warning($"An error occurred in RunPaymentReminderJob: {ex.Message}. A new error occurred - Class: RunPaymentReminderJob");
//            }
//        }

//        private async Task ProcessInvoice(MailInvoice item)
//        {
//            var existingRecord = await _outboxMessageRepository.GetAllAsync(x => x.Payload == $"{item.InvoiceId}-{item.SubscriberId}");

//            if (existingRecord.Count() == 0)
//            {
//                var outboxMessage = new OutboxMessage
//                {
//                    Type = "Subscription-Termination",
//                    Payload = $"{item.InvoiceId}-{item.SubscriberId}",
//                    CreatedByUser = 10,
//                    IsProcessed = false
//                };

//                await _outboxMessageRepository.AddAsync(outboxMessage);
//            }
//            else
//            {
//                Console.WriteLine($"A record with the same payload already exists: {item.InvoiceId}-{item.SubscriberId}");
//            }
//        }

//    }
//}
