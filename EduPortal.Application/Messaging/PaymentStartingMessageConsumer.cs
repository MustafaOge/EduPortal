using MassTransit;
using EduPortal.Application.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using EduPortal.Application.Services;
using EduPortal.Domain.Enums;

namespace EduPortal.Application.Messaging
{
    public class PaymentStartingMessage
    {
        public string Id { get; set; }
    }
    public class PaymentStartingMessageConsumer : IConsumer<PaymentStartingMessage>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly SubscriberNotificationService _subscriberNotification;

        public PaymentStartingMessageConsumer(IServiceProvider serviceProvider, SubscriberNotificationService subscriberNotification)
        {
            _serviceProvider = serviceProvider;
            _subscriberNotification = subscriberNotification;
        }

        public async Task Consume(ConsumeContext<PaymentStartingMessage> context)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var logger = scope.ServiceProvider.GetRequiredService<ILogger<PaymentStartingMessageConsumer>>();

                try
                {
                    logger.LogInformation("Received payment starting message: {Id}", context.Message.Id);
                    MessageType messageType = context.Message.Id.Length < 6 ? MessageType.SubscriptionTermination : MessageType.InvoiceReminder;
                    await _subscriberNotification.SendEmailNotification(messageType, context.Message.Id);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while processing payment starting message: {Id}", context.Message.Id);
                }
            }
        }
    }
}
