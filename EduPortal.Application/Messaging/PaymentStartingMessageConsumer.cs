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
        public MessageType ProccesType { get; set; }

    }

    public enum QueueuProccessTypes
    {

    }

    public class OutageNotificationStartingMessage
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
                    logger.LogInformation("Received payment starting message: {Id}", context.Message.Id, context.Message.ProccesType);
                    //MessageType messageType = context.Message.Id.Length < 6 ? MessageType.SubscriptionTermination : MessageType.InvoiceReminder;
                    await _subscriberNotification.SendEmailNotification(context.Message.ProccesType, context.Message.Id);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while processing payment starting message: {Id}", context.Message.Id);
                }
            }
        }
    }

    //public class OutageNotificationConsumer : IConsumer<OutageNotificationStartingMessage>
    //{
    //    private readonly IServiceProvider _serviceProvider;

    //    public OutageNotificationConsumer(IServiceProvider serviceProvider)
    //    {
    //        _serviceProvider = serviceProvider;
    //    }

    //    public async Task Consume(ConsumeContext<OutageNotificationStartingMessage> context)
    //    {
    //        using (var scope = _serviceProvider.CreateScope())
    //        {
    //            var logger = scope.ServiceProvider.GetRequiredService<ILogger<OutageNotificationConsumer>>();

    //            try
    //            {
    //                logger.LogInformation("Received outage notification message: {Id}", context.Message.Id);
    //                // Kesinti mesajlarını işleme kodu buraya gelir.
    //            }
    //            catch (Exception ex)
    //            {
    //                logger.LogError(ex, "An error occurred while processing outage notification message: {Id}", context.Message.Id);
    //            }
    //        }
    //    }
    //}

    public class PaymentStartingMessageConsssumer(IPublishEndpoint publishEndpoint, IServiceProvider serviceProvider)
       : IConsumer<OutageNotificationStartingMessage>
    {
        public Task Consume(ConsumeContext<OutageNotificationStartingMessage> context)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var logger = serviceProvider.GetService<ILogger<OutageNotificationStartingMessage>>();

            }


            Console.WriteLine($"payment :{context.Message.Id}");

            return Task.CompletedTask;
        }
    }

    //public class OutageNotificationConsumer : IConsumer<PaymentStartingMessage>
    //{
    //    private readonly IServiceProvider _serviceProvider;
    //    private readonly SubscriberNotificationService _subscriberNotification;

    //    public OutageNotificationConsumer(IServiceProvider serviceProvider, SubscriberNotificationService subscriberNotification)
    //    {
    //        _serviceProvider = serviceProvider;
    //        _subscriberNotification = subscriberNotification;
    //    }

    //    public async Task Consume(ConsumeContext<PaymentStartingMessage> context)
    //    {
    //        using (var scope = _serviceProvider.CreateScope())
    //        {
    //            var logger = scope.ServiceProvider.GetRequiredService<ILogger<PaymentStartingMessageConsumer>>();

    //            try
    //            {
    //                logger.LogInformation("Received payment starting message: {Id}", context.Message.Id);
    //                MessageType messageType = context.Message.Id.Length < 6 ? MessageType.SubscriptionTermination : MessageType.InvoiceReminder;
    //                await _subscriberNotification.SendEmailNotification(messageType, context.Message.Id);
    //            }
    //            catch (Exception ex)
    //            {
    //                logger.LogError(ex, "An error occurred while processing payment starting message: {Id}", context.Message.Id);
    //            }
    //        }
    //    }
    //}


    //public class PaymentStartingMessageConsumer : IConsumer<PaymentStartingMessage>
    //{
    //    private readonly IServiceProvider _serviceProvider;
    //    private readonly SubscriberNotificationService _subscriberNotification;

    //    public PaymentStartingMessageConsumer(IServiceProvider serviceProvider, SubscriberNotificationService subscriberNotification)
    //    {
    //        _serviceProvider = serviceProvider;
    //        _subscriberNotification = subscriberNotification;
    //    }

    //    public async Task Consume(ConsumeContext<PaymentStartingMessage> context)
    //    {
    //        using (var scope = _serviceProvider.CreateScope())
    //        {
    //            var logger = scope.ServiceProvider.GetRequiredService<ILogger<PaymentStartingMessageConsumer>>();

    //            try
    //            {
    //                logger.LogInformation("Received payment starting message: {Id}", context.Message.Id);
    //                MessageType messageType = context.Message.Id.Length < 6 ? MessageType.SubscriptionTermination : MessageType.InvoiceReminder;
    //                await _subscriberNotification.SendEmailNotification(messageType, context.Message.Id);
    //            }
    //            catch (Exception ex)
    //            {
    //                logger.LogError(ex, "An error occurred while processing payment starting message: {Id}", context.Message.Id);
    //            }
    //        }
    //    }
    //}
}
