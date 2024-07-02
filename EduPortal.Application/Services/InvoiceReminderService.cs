using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Interfaces.UnitOfWorks;
using EduPortal.Application.Messaging;
using EduPortal.Domain.Entities;
using EduPortal.Domain.Enums;
using Serilog;
using System.Net;

namespace EduPortal.Application.Services
{
    public class InvoiceReminderService
    {
        private readonly IQueueService _queueService;
        private readonly IGenericRepository<OutboxMessage, int> _outboxMessageRepository;
        private readonly MessagePublisherService _publisherServiceMassTransit;
        private readonly IUnitOfWork _unitOfWork;

        public InvoiceReminderService(
            IQueueService queueService,
            IUnitOfWork unitOfWork,
            IGenericRepository<OutboxMessage, int> outboxMessageRepository,
            MessagePublisherService publisherServiceMassTransit)
        {
            _unitOfWork = unitOfWork;
            _queueService = queueService ?? throw new ArgumentNullException(nameof(queueService));
            _outboxMessageRepository = outboxMessageRepository ?? throw new ArgumentNullException(nameof(outboxMessageRepository));
            _publisherServiceMassTransit = publisherServiceMassTransit ?? throw new ArgumentNullException(nameof(publisherServiceMassTransit));
        }

        public async Task RunPaymentReminder()
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
            await _publisherServiceMassTransit.SendMessageAsync(message.Payload, MessageType.InvoiceReminder);
        }

        private async Task MarkMessageAsProcessed(OutboxMessage message)
        {
            message.IsProcessed = true;
            _outboxMessageRepository.Update(message);
            await _unitOfWork.CommitAsync(); // Assuming _unitOfWork is available and has a CommitAsync method
        }

    }
}
