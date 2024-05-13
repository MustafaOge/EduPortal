using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Messaging;
using EduPortal.Application.Services;
using EduPortal.Core.Responses;
using EduPortal.Domain.Entities;
using EduPortal.Persistence.Services;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EduPortal.Application.HangfireJobs.Managers.ReccurringJobs
{
    public class InvoiceReminderJob
    {
        private readonly IQueueService _queueService;
        private readonly IGenericRepository<OutboxMessage, int> _outboxMessageRepository;
        private readonly RabbitMQPublisherService _rabbitMQPublisherService;
        private readonly RabbitMQConsumerService _rabbitMQConsumerService;

        public InvoiceReminderJob(RabbitMQConsumerService rabbitMQConsumerService, RabbitMQPublisherService rabbitMQPublisherService, IQueueService queueService, OutboxMessageProcessor outboxMessageProcessor, IGenericRepository<OutboxMessage, int> outboxMessageRepository)

        {
            _outboxMessageRepository = outboxMessageRepository;
            _queueService = queueService;
            _rabbitMQPublisherService = rabbitMQPublisherService;
            _rabbitMQConsumerService = rabbitMQConsumerService;
        }
        bool messageProcessingCompleted = false;

        public async Task RunPaymentReminderJob()
        {
            var response = await _queueService.GetUpcomingPaymentInvoices();

            try
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    foreach (var item in response.Data)
                    {
                        // Payload bilgisine sahip bir kayıt var mı kontrol et
                        var existingRecord = await _outboxMessageRepository.GetAllAsync(x => x.Payload == $"{item.InvoiceId}-{item.SubscriberId}");

                        // Eğer aynı payload bilgisine sahip bir kayıt yoksa, yeni bir kayıt ekle
                        if (existingRecord.Count() == 0)
                        {
                            var outboxMessage = new OutboxMessage
                            {
                                Type = "Subscription-Termination",
                                Payload = $"{item.InvoiceId}-{item.SubscriberId}",
                                CreatedByUser = 10,
                                IsProcessed = false
                            };
                            await _outboxMessageRepository.AddAsync(outboxMessage);
                        }
                        else
                        {
                            Console.WriteLine($"A record with the same payload already exists: {item.InvoiceId}-{item.SubscriberId}");
                        }
                    }
                    await _rabbitMQPublisherService.StartPublishing();
                    _rabbitMQConsumerService.StartConsuming();
                }

                else
                {
                    Console.WriteLine("Failed to retrieve upcoming payment invoices.");
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Log.Warning($"An error occurred in RunPaymentReminderJob: : {ex.Message} Yeni hata oluştu - Class: RunPaymentReminderJob");


            }
        }
    }
}
