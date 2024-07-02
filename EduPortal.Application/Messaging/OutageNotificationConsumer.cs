using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Domain.Entities;
using MassTransit;
using Newtonsoft.Json;
using RabbitMQ.ESB.MassTransit.Shared.Messages;
using Microsoft.Extensions.Logging;

namespace EduPortal.Application.Messaging
{
    public class OutageNotificationConsumer : IConsumer<IMessage>
    {
        private readonly IAddressRepository _adMahalleRepository;
        private readonly IGenericRepository<OutageNotification, int> _outageNotificationRepository;
        private readonly ILogger<OutageNotificationConsumer> _logger;

        public OutageNotificationConsumer(IGenericRepository<OutageNotification, int> outageNotificationRepository, IAddressRepository adMahalleRepository, ILogger<OutageNotificationConsumer> logger)
        {
            _outageNotificationRepository = outageNotificationRepository;
            _adMahalleRepository = adMahalleRepository;
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<IMessage> context)
        {
            var message = context.Message;

            Console.WriteLine($"Gelen mesaj içeriği: {JsonConvert.SerializeObject(message)}");
            _logger.LogInformation($"Gelen mesaj içeriği: {JsonConvert.SerializeObject(message)}");

            // Mesaj işleme kodu...
        }

    }

}
