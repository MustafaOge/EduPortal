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
    public class InvoiceReminderJob(
        IQueueService _queueService,
        IGenericRepository<OutboxMessage, int> _outboxMessageRepository,
        RabbitMQPublisherService _rabbitMQPublisherService,
        RabbitMQConsumerService _rabbitMQConsumerService)
    {
        public async Task RunPaymentReminderJob()
        {
            try
            {
                Response<IEnumerable<MailInvoice>> response = await _queueService.GetUpcomingPaymentInvoices();

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    foreach (var item in response.Data)
                    {
                        await ProcessInvoice(item);
                    }

                    await _rabbitMQPublisherService.StartPublishing();
                    _rabbitMQConsumerService.StartConsuming();
                }
                else
                {
                    // Error occurred while retrieving upcoming payment invoices
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

        private async Task ProcessInvoice(MailInvoice item)
        {
            var existingRecord = await _outboxMessageRepository.GetAllAsync(x => x.Payload == $"{item.InvoiceId}-{item.SubscriberId}");

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

    }
}
