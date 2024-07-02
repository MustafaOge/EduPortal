using EduPortal.Application.Services;
using EduPortal.Domain.Enums;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace EduPortal.Application.Messaging
{
    public class PaymentStartingMessage
    {
        public string Id { get; set; }
        public MessageType ProccesType { get; set; }
    }

    public class OutageNotificationStartingMessage
    {
        public string Id { get; set; }
    }

    public class PaymentStartingMessageConsumer : IConsumer<PaymentStartingMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SubscriberNotificationService _subscriberNotification;
        private readonly ILogger<PaymentStartingMessageConsumer> _logger;

        public PaymentStartingMessageConsumer(IServiceProvider serviceProvider, SubscriberNotificationService subscriberNotification, ILogger<PaymentStartingMessageConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _subscriberNotification = subscriberNotification;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<PaymentStartingMessage> context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    _logger.LogInformation("Received payment starting message: {Id}", context.Message.Id);
                    MessageType messageType = context.Message.Id.Length < 6 ? MessageType.SubscriptionTermination : MessageType.InvoiceReminder;

                    await _subscriberNotification.SendEmailNotification(context.Message.ProccesType, context.Message.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing payment starting message: {Id}", context.Message.Id);
                }
            }
        }
    }

    public class OutageNotificationStartingMessageConsumer : IConsumer<OutageNotificationStartingMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<OutageNotificationStartingMessageConsumer> _logger;

        public OutageNotificationStartingMessageConsumer(IServiceProvider serviceProvider, ILogger<OutageNotificationStartingMessageConsumer> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task Consume(ConsumeContext<OutageNotificationStartingMessage> context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                try
                {
                    _logger.LogInformation("Received outage notification starting message: {Id}", context.Message.Id);
                    // İşlemlerinizi burada gerçekleştirin
                    Console.WriteLine($"payment :{context.Message.Id}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while processing outage notification starting message: {Id}", context.Message.Id);
                }
            }

            return Task.CompletedTask;
        }
    }
}
