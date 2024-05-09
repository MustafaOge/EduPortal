using EduPortal.Application.Interfaces.Repositories;
using EduPortal.Application.Interfaces.Services;
using EduPortal.Application.Messaging;
using EduPortal.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EduPortal.Application.Services
{
    public class SubscriberTerminateService : ISubscriberTerminateService
    {
        public event EventHandler<int> SubscriptionTerminated;

        private readonly IGenericRepository<OutboxMessage, int> _outboxMessageRepository;
        private readonly RabbitMQPublisherService _rabbitMQPublisher;
        private readonly RabbitMQConsumerService _rabbitMqConsumer;

        public SubscriberTerminateService(IGenericRepository<OutboxMessage, int> outboxMessageRepository, RabbitMQPublisherService rabbitMQPublisher, RabbitMQConsumerService rabbitMQConsumer)
        {
            _outboxMessageRepository = outboxMessageRepository;
            _rabbitMQPublisher = rabbitMQPublisher;
            _rabbitMqConsumer = rabbitMQConsumer;
        }

        public async Task TerminateSubscriptionAndAddToOutbox(int subscriberId)
        {
            // Abonelik sonlandırma işlemleri burada gerçekleştirilir

            // Sonlandırılan abonelik bilgisini outbox tablosuna ekle
            var outboxMessage = new OutboxMessage
            {
                Type = "Subscription-Termination",
                Payload = subscriberId.ToString(),
                CreatedByUser = 10, // Kullanıcı kimliği buraya gelebilir
                IsProcessed = false
            };
            await _outboxMessageRepository.AddAsync(outboxMessage);
            await _rabbitMQPublisher.StartPublishing();
              _rabbitMqConsumer.StartConsuming(); //?


            //SubscriptionTerminated?.Invoke(this, subscriberId);

        }
    }


}
